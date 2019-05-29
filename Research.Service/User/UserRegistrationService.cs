using Research.Core;
using Research.Core.Domain;
using Research.Core.Domain.Users;
using Research.Data;
using Research.Enum;
using Research.Services.Events;
using Research.Services.Messages;
using Research.Services.Roles;
using Research.Services.Security;
using System;
using System.Linq;


namespace Research.Services.Users
{
    /// <summary>
    /// User registration service
    /// </summary>
    public partial class UserRegistrationService : IUserRegistrationService
    {
        #region Fields

        private readonly UserSettings _userSettings;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IEncryptionService _encryptionService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWorkContext _workContext;
        private readonly IWorkflowMessageService _workflowMessageService;


        #endregion

        #region Ctor

        public UserRegistrationService(UserSettings userSettings,
            IUserService userService,
            IRoleService roleService,
            IEncryptionService encryptionService,
            IEventPublisher eventPublisher,
            IWorkContext workContext,
            IWorkflowMessageService workflowMessageService)
        {
            this._userSettings = userSettings;
            this._userService = userService;
            this._roleService = roleService;
            this._encryptionService = encryptionService;
            this._eventPublisher = eventPublisher;
            this._workContext = workContext;
            this._workflowMessageService = workflowMessageService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Check whether the entered password matches with a saved one
        /// </summary>
        /// <param name="userPassword">User password</param>
        /// <param name="enteredPassword">The entered password</param>
        /// <returns>True if passwords match; otherwise false</returns>
        protected bool PasswordsMatch(UserPassword userPassword, string enteredPassword)
        {
            if (userPassword == null || string.IsNullOrEmpty(enteredPassword))
                return false;

            var savedPassword = string.Empty;
            switch (userPassword.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    savedPassword = enteredPassword;
                    break;
                case PasswordFormat.Encrypted:
                    savedPassword = _encryptionService.EncryptText(enteredPassword);
                    break;
                case PasswordFormat.Hashed:
                    savedPassword = _encryptionService.CreatePasswordHash(enteredPassword, userPassword.PasswordSalt, _userSettings.HashedPasswordFormat);
                    break;
            }

            if (userPassword.Password == null)
                return false;

            return userPassword.Password.Equals(savedPassword);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="usernameOrEmail">Username or email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        public virtual UserLoginResults ValidateUser(string usernameOrEmail, string password)
        {
            var user = _userSettings.UsernamesEnabled ?
                _userService.GetUserByUsername(usernameOrEmail) :
                _userService.GetUserByEmail(usernameOrEmail);

            if (user == null)
                return UserLoginResults.UserNotExist;
            if (user.Deleted)
                return UserLoginResults.Deleted;
            if (!user.IsActive)
                return UserLoginResults.NotActive;
            //only registered can login
            if (!user.IsRegistered())
                return UserLoginResults.NotRegistered;
            //check whether a user is locked out
            if (user.CannotLoginUntilDateUtc.HasValue && user.CannotLoginUntilDateUtc.Value > DateTime.UtcNow)
                return UserLoginResults.LockedOut;

            if (!PasswordsMatch(_userService.GetCurrentPassword(user.Id), password))
            {
                //wrong password
                user.FailedLoginAttempts++;
                if (_userSettings.FailedPasswordAllowedAttempts > 0 &&
                    user.FailedLoginAttempts >= _userSettings.FailedPasswordAllowedAttempts)
                {
                    //lock out
                    user.CannotLoginUntilDateUtc = DateTime.UtcNow.AddMinutes(_userSettings.FailedPasswordLockoutMinutes);
                    //reset the counter
                    user.FailedLoginAttempts = 0;
                }

                _userService.UpdateUser(user);

                return UserLoginResults.WrongPassword;
            }

            //update login details
            user.FailedLoginAttempts = 0;
            user.CannotLoginUntilDateUtc = null;
            user.RequireReLogin = false;
            _userService.UpdateUser(user);

            return UserLoginResults.Successful;
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual UserRegistrationResult RegisterUser(UserRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.User == null)
                throw new ArgumentException("Can't load current user");

            var result = new UserRegistrationResult();
            if (request.User.IsSearchEngineAccount())
            {
                result.AddError("Search engine can't be registered");
                return result;
            }

            if (request.User.IsBackgroundTaskAccount())
            {
                result.AddError("Background task account can't be registered");
                return result;
            }

            if (request.User.IsRegistered())
            {
                result.AddError("Current user is already registered");
                return result;
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                result.AddError("EmailIsNotProvided");
                return result;
            }

            if (!CommonHelper.IsValidEmail(request.Email))
            {
                result.AddError("WrongEmail");
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError("Password Is Not Provided");
                return result;
            }

            if (_userSettings.UsernamesEnabled && string.IsNullOrEmpty(request.Username))
            {
                result.AddError("Username Is Not Provided");
                return result;
            }

            //validate unique user
            if (_userService.GetUserByEmail(request.Email) != null)
            {
                result.AddError("Email Already Exists");
                return result;
            }

            if (_userSettings.UsernamesEnabled && _userService.GetUserByUsername(request.Username) != null)
            {
                result.AddError("Username Already Exists");
                return result;
            }

            //at this point request is valid
            request.User.UserName = request.Username;
            request.User.Email = request.Email;

            var userPassword = new UserPassword
            {
                User = request.User,
                PasswordFormat = request.PasswordFormat,
                CreatedOnUtc = DateTime.UtcNow
            };
            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    userPassword.Password = request.Password;
                    break;
                case PasswordFormat.Encrypted:
                    userPassword.Password = _encryptionService.EncryptText(request.Password);
                    break;
                case PasswordFormat.Hashed:
                    var saltKey = _encryptionService.CreateSaltKey(ResearchUserServiceDefaults.PasswordSaltKeySize);
                    userPassword.PasswordSalt = saltKey;
                    userPassword.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey, _userSettings.HashedPasswordFormat);
                    break;
            }

            _userService.InsertUserPassword(userPassword);
            //var userRole = new UserRole
            //{
            //    User = request.User,
            //    RoleId = (int)UserType.Researcher
            //};
            //_userService.InsertUserRole(userRole);
            request.User.IsActive = request.IsApproved;

            //add to 'Researcher' role
            var registeredRole = _userService.GetUserByRoleId(ResearchUserDefaults.ResearcherRoleId);
            if (registeredRole == null)
                throw new ResearchException("'Researcher' role could not be loaded");
            var role = _roleService.GetRoleById((int) UserType.Researcher); // researcher
            //request.User.UserRoles.Add(registeredRole);
            request.User.UserRoles.Add(new UserRole { User = registeredRole,Role = role });
            //remove from 'Guests' role
            var guestRole = request.User.UserRoles.FirstOrDefault(cr => cr.RoleId == ResearchUserDefaults.GuestsRoleId);
            if (guestRole != null)
            {
                //request.User.UserRoles.Remove(guestRole);
                request.User.UserRoles
                    .Remove(request.User.UserRoles.FirstOrDefault(mapping => mapping.RoleId == guestRole.Id));
            }



            _userService.UpdateUser(request.User);

            return result;
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual ChangePasswordResult ChangePassword(ChangePasswordRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new ChangePasswordResult();
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                result.AddError("EmailIsNotProvided");
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                result.AddError("PasswordIsNotProvided");
                return result;
            }

            var user = _userService.GetUserByEmail(request.Email);
            if (user == null)
            {
                result.AddError("EmailNotFound");
                return result;
            }
          
            //request isn't valid
            if (request.ValidateRequest && !PasswordsMatch(_userService.GetCurrentPassword(user.Id), request.OldPassword))
            {
                result.AddError("OldPasswordDoesntMatch");
                return result;
            }

            //check for duplicates
            if (_userSettings.UnduplicatedPasswordsNumber > 0)
            {
                //get some of previous passwords
                var previousPasswords = _userService.GetUserPasswords(user.Id, passwordsToReturn: _userSettings.UnduplicatedPasswordsNumber);

                var newPasswordMatchesWithPrevious = previousPasswords.Any(password => PasswordsMatch(password, request.NewPassword));
                if (newPasswordMatchesWithPrevious)
                {
                    result.AddError("PasswordMatchesWithPrevious");
                    return result;
                }
            }

            //at this point request is valid
            var userPassword = new UserPassword
            {
                User = user,
                PasswordFormat = request.NewPasswordFormat,
                CreatedOnUtc = DateTime.UtcNow
            };
            switch (request.NewPasswordFormat)
            {
                case PasswordFormat.Clear:
                    userPassword.Password = request.NewPassword;
                    break;
                case PasswordFormat.Encrypted:
                    userPassword.Password = _encryptionService.EncryptText(request.NewPassword);
                    break;
                case PasswordFormat.Hashed:
                    var saltKey = _encryptionService.CreateSaltKey(ResearchUserServiceDefaults.PasswordSaltKeySize);
                    userPassword.PasswordSalt = saltKey;
                    userPassword.Password = _encryptionService.CreatePasswordHash(request.NewPassword, saltKey, _userSettings.HashedPasswordFormat);
                    break;
            }

            _userService.InsertUserPassword(userPassword);

            //publish event
            _eventPublisher.Publish(new UserPasswordChangedEvent(userPassword));

            return result;
        }

        /// <summary>
        /// Sets a user email
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newEmail">New email</param>
        /// <param name="requireValidation">Require validation of new email address</param>
        public virtual void SetEmail(User user, string newEmail, bool requireValidation)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (newEmail == null)
                throw new ResearchException("Email cannot be null");

            newEmail = newEmail.Trim();
            var oldEmail = user.Email;

            if (!CommonHelper.IsValidEmail(newEmail))
                throw new ResearchException("NewEmailIsNotValid");

            if (newEmail.Length > 100)
                throw new ResearchException("EmailTooLong");

            var user2 = _userService.GetUserByEmail(newEmail);
            if (user2 != null && user.Id != user2.Id)
                throw new ResearchException("EmailAlreadyExists");

            if (requireValidation)
            {
                //re-validate email
                //user.EmailToRevalidate = newEmail;
                user.Email = newEmail;
                _userService.UpdateUser(user);

                //email re-validation message
                //_genericAttributeService.SaveAttribute(user, ResearchUserDefaults.EmailRevalidationTokenAttribute, Guid.NewGuid().ToString());
                //_workflowMessageService.SendUserEmailRevalidationMessage(user, _workContext.WorkingLanguage.Id);
                _workflowMessageService.SendUserEmailRevalidationMessage(user, 0);
            }
            //else
            //{
            //    user.Email = newEmail;
            //    _userService.UpdateUser(user);
                
            //    if (string.IsNullOrEmpty(oldEmail) || oldEmail.Equals(newEmail, StringComparison.InvariantCultureIgnoreCase)) 
            //        return;

            //    //update newsletter subscription (if required)
            //    foreach (var store in _storeService.GetAllStores())
            //    {
            //        var subscriptionOld = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(oldEmail, store.Id);
                    
            //        if (subscriptionOld == null) 
            //            continue;

            //        subscriptionOld.Email = newEmail;
            //        _newsLetterSubscriptionService.UpdateNewsLetterSubscription(subscriptionOld);
            //    }
            //}
        }

        /// <summary>
        /// Sets a user username
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newUsername">New Username</param>
        public virtual void SetUsername(User user, string newUsername)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!_userSettings.UsernamesEnabled)
                throw new ResearchException("Usernames are disabled");

            newUsername = newUsername.Trim();

            if (newUsername.Length > ResearchUserServiceDefaults.UserUsernameLength)
                throw new ResearchException("UsernameTooLong");

            var user2 = _userService.GetUserByUsername(newUsername);
            if (user2 != null && user.Id != user2.Id)
                throw new ResearchException("UsernameAlreadyExists");

            user.UserName = newUsername;
            _userService.UpdateUser(user);
        }

        #endregion
    }
}