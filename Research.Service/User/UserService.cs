using System;
using System.Collections.Generic;
using System.Linq;
using Research.Services.Events;
using Research.Core;
using Research.Data;
using Research.Enum;
using Research.Core.Caching;
using Research.Core.Data;
using Research.Core.Data.Extensions;
using Research.Core.Domain.Users;

namespace Research.Services.Users
{
    /// <summary>
    /// User service
    /// </summary>
    public partial class UserService : IUserService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserPassword> _userPasswordRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly string _entityName;

        #endregion

        #region Ctor

        public UserService(ICacheManager cacheManager,
            IDataProvider dataProvider,
            IDbContext dbContext,
            IEventPublisher eventPublisher,
            IRepository<User> userRepository,
            IRepository<UserPassword> userPasswordRepository,
            IRepository<UserRole> userRoleRepository,
            IStaticCacheManager staticCacheManager)
        {
            this._cacheManager = cacheManager;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._eventPublisher = eventPublisher;
            this._userRepository = userRepository;
            this._userPasswordRepository = userPasswordRepository;
            this._userRoleRepository = userRoleRepository;
            this._staticCacheManager = staticCacheManager;
            this._entityName = typeof(User).Name;
        }

        #endregion

        #region Methods

        #region Users

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="researcherId">Researcher identifier</param>
        /// <param name="userRoleIds">A list of user role identifiers to filter by (at least one match); pass null or empty list in order to load all users; </param>
        /// <param name="email">Email; null to load all users</param>
        /// <param name="username">Username; null to load all users</param>
        /// <param name="firstName">First name; null to load all users</param>
        /// <param name="lastName">Last name; null to load all users</param>
        /// <param name="dayOfBirth">Day of birth; 0 to load all users</param>
        /// <param name="monthOfBirth">Month of birth; 0 to load all users</param>
        /// <param name="company">Company; null to load all users</param>
        /// <param name="phone">Phone; null to load all users</param>
        /// <param name="zipPostalCode">Phone; null to load all users</param>
        /// <param name="ipAddress">IP address; null to load all users</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="getOnlyTotalCount">A value in indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
        /// <returns>Users</returns>
        public virtual IPagedList<User> GetAllUsers(DateTime? createdFromUtc = null,
            DateTime? createdToUtc = null, int researcherId = 0,
            int[] userRoleIds = null, string email = null, string username = null,
            string firstName = null, string lastName = null,
            int dayOfBirth = 0, int monthOfBirth = 0,int agencyId = 0,
            string company = null, string phone = null, string zipPostalCode = null,
            string ipAddress = null, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var query = _userRepository.Table;
            query = query.Where(c => c.Roles != null);
            if (createdFromUtc.HasValue)
                query = query.Where(c => createdFromUtc.Value <= c.Created);
            if (createdToUtc.HasValue)
                query = query.Where(c => createdToUtc.Value >= c.Created);
            if (researcherId != 0)
                query = query.Where(c => researcherId == c.ResearcherId);
            if (agencyId != 0)
                query = query.Where(c => agencyId == c.AgencyId);
            query = query.Where(c => c.Deleted == false);

            //if (userRoleIds != null && userRoleIds.Length > 0)
            //{
            //    query = query.Join(_userRoleRepository.Table, x => x.Id, y => y.UserId,
            //            (x, y) => new { User = x, Mapping = y })
            //        .Where(z => userRoleIds.Contains(z.Mapping.UserRoleId))
            //        .Select(z => z.User)
            //        .Distinct();
            //}

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(c => c.Email.Contains(email));
            if (!string.IsNullOrWhiteSpace(username))
                query = query.Where(c => c.UserName.Contains(username));
            if (!string.IsNullOrWhiteSpace(firstName))
                query = query.Where(c => c.FirstName.Contains(firstName));

            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(c => c.LastName.Contains(lastName));

      

            query = query.OrderBy(c => c.FirstName);

            var users = new PagedList<User>(query, pageIndex, pageSize, getOnlyTotalCount);
            return users;
        }

        /// <summary>
        /// Gets online users
        /// </summary>
        /// <param name="lastActivityFromUtc">User last activity date (from)</param>
        /// <param name="userRoleIds">A list of user role identifiers to filter by (at least one match); pass null or empty list in order to load all users; </param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Users</returns>
        public virtual IPagedList<User> GetOnlineUsers(DateTime lastActivityFromUtc,
            int[] userRoleIds, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _userRepository.Table;
            query = query.Where(c => lastActivityFromUtc <= c.LastActivityDateUtc);
            query = query.Where(c => !c.Deleted);
            if (userRoleIds != null && userRoleIds.Length > 0)
                query = query.Where(c => c.UserUserRoleMappings.Select(mapping => mapping.UserRoleId).Intersect(userRoleIds).Any());

            query = query.OrderByDescending(c => c.LastActivityDateUtc);
            var users = new PagedList<User>(query, pageIndex, pageSize);
            return users;
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void DeleteUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (user.IsSystemAccount)
                throw new ResearchException($"System user account ({user.IsSystemAccount}) could not be deleted");

            user.Deleted = true;



            UpdateUser(user);

            //event notification
            _eventPublisher.EntityDeleted(user);
        }

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>A user</returns>
        public virtual User GetUserById(int userId)
        {
            if (userId == 0)
                return null;

            return _userRepository.GetById(userId);
        }

        /// <summary>
        /// Get users by identifiers
        /// </summary>
        /// <param name="userIds">User identifiers</param>
        /// <returns>Users</returns>
        public virtual IList<User> GetUsersByIds(int[] userIds)
        {
            if (userIds == null || userIds.Length == 0)
                return new List<User>();

            var query = from c in _userRepository.Table
                        where userIds.Contains(c.Id) && !c.Deleted
                        select c;
            var users = query.ToList();
            //sort by passed identifiers
            var sortedUsers = new List<User>();
            foreach (var id in userIds)
            {
                var user = users.Find(x => x.Id == id);
                if (user != null)
                    sortedUsers.Add(user);
            }

            return sortedUsers;
        }

        ///// <summary>
        ///// Gets a user by GUID
        ///// </summary>
        ///// <param name="userGuid">User GUID</param>
        ///// <returns>A user</returns>
        //public virtual User GetUserByGuid(Guid userGuid)
        //{
        //    if (userGuid == Guid.Empty)
        //        return null;

        //    var query = from c in _userRepository.Table
        //                where c.UserGuid == userGuid
        //                orderby c.Id
        //                select c;
        //    var user = query.FirstOrDefault();
        //    return user;
        //}

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User</returns>
        public virtual User GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            //var query = _userRepository.Table;
            //query = query.Where(x => x.Email.Contains(email));
            //var user = query.FirstOrDefault();
            
            var query = from c in _userRepository.Table
                        orderby c.Id
                        where c.Email == email
                        select c;
            var user = query.FirstOrDefault();

            return user;
        }

        /// <summary>
        /// Get user by system name
        /// </summary>
        /// <param name="roleName">System name</param>
        /// <returns>User</returns>
        public virtual User GetUserByRoleId(int roleId)
        {
            if (roleId == 0)
                return null;

            var query = from c in _userRepository.Table
                        orderby c.Id
                        where c.UserUserRoleMappings.Any(x=> x.UserRoleId == roleId)
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User</returns>
        public virtual User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var query = from c in _userRepository.Table
                        orderby c.Id
                        where c.UserName == username
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Insert a guest user
        /// </summary>
        /// <returns>User</returns>
        public virtual User InsertGuestUser()
        {
            var user = new User
            {
                UserGuid = Guid.NewGuid(),
                IsActive = true,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };

            //add to 'Guests' role
            var guestRole = GetUserRoleBySystemName(ResearchUserDefaults.GuestsRoleName);
            if (guestRole == null)
                throw new ResearchException("'Guests' role could not be loaded");
            //user.UserRoles.Add(guestRole);
            user.UserUserRoleMappings.Add(new UserUserRoleMapping { UserRole = guestRole });

            _userRepository.Insert(user);

            return user;
        }

        /// <summary>
        /// Insert a user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void InsertUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _userRepository.Insert(user);

            //event notification
            _eventPublisher.EntityInserted(user);
        }

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _userRepository.Update(user);

            //event notification
            _eventPublisher.EntityUpdated(user);
        }

    

        /// <summary>
        /// Delete guest user records
        /// </summary>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="onlyWithoutShoppingCart">A value indicating whether to delete users only without shopping cart</param>
        /// <returns>Number of deleted users</returns>
        public virtual int DeleteGuestUsers(DateTime? createdFromUtc, DateTime? createdToUtc, bool onlyWithoutShoppingCart)
        {
            //prepare parameters
            var pOnlyWithoutShoppingCart = _dataProvider.GetBooleanParameter("OnlyWithoutShoppingCart", onlyWithoutShoppingCart);
            var pCreatedFromUtc = _dataProvider.GetDateTimeParameter("CreatedFromUtc", createdFromUtc);
            var pCreatedToUtc = _dataProvider.GetDateTimeParameter("CreatedToUtc", createdToUtc);
            var pTotalRecordsDeleted = _dataProvider.GetOutputInt32Parameter("TotalRecordsDeleted");

            //invoke stored procedure
            _dbContext.ExecuteSqlCommand(
                "EXEC [DeleteGuests] @OnlyWithoutShoppingCart, @CreatedFromUtc, @CreatedToUtc, @TotalRecordsDeleted OUTPUT",
                false, null,
                pOnlyWithoutShoppingCart,
                pCreatedFromUtc,
                pCreatedToUtc,
                pTotalRecordsDeleted);

            var totalRecordsDeleted = pTotalRecordsDeleted.Value != DBNull.Value ? Convert.ToInt32(pTotalRecordsDeleted.Value) : 0;
            return totalRecordsDeleted;
            return 0;
        }

        /// <summary>
        /// Get full name
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>User full name</returns>
        public virtual string GetUserFullName(User user)
        {
            return $"{user.FirstName} {user.LastName}";
        }

        /// <summary>
        /// Formats the user name
        /// </summary>
        /// <param name="user">Source</param>
        /// <param name="stripTooLong">Strip too long user name</param>
        /// <param name="maxLength">Maximum user name length</param>
        /// <returns>Formatted text</returns>
        public virtual string FormatUserName(User user, bool stripTooLong = false, int maxLength = 0)
        {
            if (user == null)
                return string.Empty;

            return this.GetUserFullName(user);
        }

   

        #endregion

        #region User roles

        /// <summary>
        /// Delete a user role
        /// </summary>
        /// <param name="userRole">User role</param>
        public virtual void DeleteUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException(nameof(userRole));

            if (userRole.IsSystemRole)
                throw new ResearchException("System role could not be deleted");

            _userRoleRepository.Delete(userRole);

            _cacheManager.RemoveByPattern(ResearchUserServiceDefaults.UserRolesPatternCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(userRole);
        }

        /// <summary>
        /// Gets a user role
        /// </summary>
        /// <param name="userRoleId">User role identifier</param>
        /// <returns>User role</returns>
        public virtual UserRole GetUserRoleById(int userRoleId)
        {
            if (userRoleId == 0)
                return null;

            return _userRoleRepository.GetById(userRoleId);
        }

        /// <summary>
        /// Gets a user role
        /// </summary>
        /// <param name="roleName">User role system name</param>
        /// <returns>User role</returns>
        public virtual UserRole GetUserRoleByRoleId(int roleId)
        {
            if (roleId == 0)
                return null;

            //var key = string.Format(ResearchUserServiceDefaults.UserRolesByRoleNameCacheKey, roleName);
            //return _cacheManager.Get(key, () =>
            //{
                var query = from cr in _userRoleRepository.Table
                            orderby cr.Id
                            where cr.Id == roleId
                            select cr;
                var userRole = query.FirstOrDefault();
                return userRole;
            //});
        }

        /// <summary>
        /// Gets all user roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>User roles</returns>
        public virtual IList<UserRole> GetAllUserRoles(bool showHidden = false)
        {
            var key = string.Format(ResearchUserServiceDefaults.UserRolesAllCacheKey, showHidden);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _userRoleRepository.Table
                            orderby cr.Name
                            where showHidden || cr.IsActive
                            select cr;
                var userRoles = query.ToList();
                return userRoles;
            });
        }

        /// <summary>
        /// Inserts a user role
        /// </summary>
        /// <param name="userRole">User role</param>
        public virtual void InsertUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException(nameof(userRole));

            _userRoleRepository.Insert(userRole);

            //_cacheManager.RemoveByPattern(ResearchUserServiceDefaults.UserRolesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(userRole);
        }

        /// <summary>
        /// Updates the user role
        /// </summary>
        /// <param name="userRole">User role</param>
        public virtual void UpdateUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException(nameof(userRole));

            _userRoleRepository.Update(userRole);

            //_cacheManager.RemoveByPattern(ResearchUserServiceDefaults.UserRolesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(userRole);
        }

        #endregion

        #region User passwords

        /// <summary>
        /// Gets user passwords
        /// </summary>
        /// <param name="userId">User identifier; pass null to load all records</param>
        /// <param name="passwordFormat">Password format; pass null to load all records</param>
        /// <param name="passwordsToReturn">Number of returning passwords; pass null to load all records</param>
        /// <returns>List of user passwords</returns>
        public virtual IList<UserPassword> GetUserPasswords(int? userId = null,
            PasswordFormat? passwordFormat = null, int? passwordsToReturn = null)
        {
            var query = _userPasswordRepository.Table;

            //filter by user
            if (userId.HasValue)
                query = query.Where(password => password.UserId == userId.Value);

            //filter by password format
            if (passwordFormat.HasValue)
                query = query.Where(password => password.PasswordFormatId == (int)passwordFormat.Value);

            //get the latest passwords
            if (passwordsToReturn.HasValue)
                query = query.OrderByDescending(password => password.CreatedOnUtc).Take(passwordsToReturn.Value);

            return query.ToList();
        }

        /// <summary>
        /// Get current user password
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>User password</returns>
        public virtual UserPassword GetCurrentPassword(int userId)
        {
            if (userId == 0)
                return null;

            //return the latest password
            return GetUserPasswords(userId, passwordsToReturn: 1).FirstOrDefault();
        }

        /// <summary>
        /// Insert a user password
        /// </summary>
        /// <param name="userPassword">User password</param>
        public virtual void InsertUserPassword(UserPassword userPassword)
        {
            if (userPassword == null)
                throw new ArgumentNullException(nameof(userPassword));

            _userPasswordRepository.Insert(userPassword);

            //event notification
            _eventPublisher.EntityInserted(userPassword);
        }

        /// <summary>
        /// Update a user password
        /// </summary>
        /// <param name="userPassword">User password</param>
        public virtual void UpdateUserPassword(UserPassword userPassword)
        {
            if (userPassword == null)
                throw new ArgumentNullException(nameof(userPassword));

            _userPasswordRepository.Update(userPassword);

            //event notification
            _eventPublisher.EntityUpdated(userPassword);
        }

        /// <summary>
        /// Check whether password recovery token is valid
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="token">Token to validate</param>
        /// <returns>Result</returns>
        public virtual bool IsPasswordRecoveryTokenValid(User user, string token)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //var cPrt = _genericAttributeService.GetAttribute<string>(user, ResearchUserDefaults.PasswordRecoveryTokenAttribute);
            //if (string.IsNullOrEmpty(cPrt))
            //    return false;

            //if (!cPrt.Equals(token, StringComparison.InvariantCultureIgnoreCase))
            //    return false;

            return true;
        }

        /// <summary>
        /// Check whether password recovery link is expired
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Result</returns>
        public virtual bool IsPasswordRecoveryLinkExpired(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //if (_userSettings.PasswordRecoveryLinkDaysValid == 0)
            //    return false;

            //var geneatedDate = _genericAttributeService.GetAttribute<DateTime?>(user, ResearchUserDefaults.PasswordRecoveryTokenDateGeneratedAttribute);
            //if (!geneatedDate.HasValue)
            //    return false;

            //var daysPassed = (DateTime.UtcNow - geneatedDate.Value).TotalDays;
            //if (daysPassed > _userSettings.PasswordRecoveryLinkDaysValid)
            //    return true;

            return false;
        }

        /// <summary>
        /// Check whether user password is expired 
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>True if password is expired; otherwise false</returns>
        public virtual bool PasswordIsExpired(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //the guests don't have a password
            //if (user.IsGuest())
            //    return false;

            ////password lifetime is disabled for user
            //if (!user.UserRoles.Any(role => role.Active && role.EnablePasswordLifetime))
            //    return false;

            ////setting disabled for all
            //if (_userSettings.PasswordLifetime == 0)
            //    return false;

            ////cache result between HTTP requests
            //var cacheKey = string.Format(ResearchUserServiceDefaults.UserPasswordLifetimeCacheKey, user.Id);

            ////get current password usage time
            //var currentLifetime = _staticCacheManager.Get(cacheKey, () =>
            //{
            //    var userPassword = this.GetCurrentPassword(user.Id);
            //    //password is not found, so return max value to force user to change password
            //    if (userPassword == null)
            //        return int.MaxValue;

            //    return (DateTime.UtcNow - userPassword.CreatedOnUtc).Days;
            //});

            //return currentLifetime >= _userSettings.PasswordLifetime;

            return false;
        }

        public User GetUserByGuid(Guid userGuid)
        {
            if (userGuid == Guid.Empty)
                return null;

            var query = from c in _userRepository.Table
                        where c.UserGuid == userGuid
                        orderby c.Id
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        public User GetUserByIDCard(string idCard)
        {
            if (string.IsNullOrWhiteSpace(idCard))
                return null;

            var query = from c in _userRepository.Table
                        orderby c.Id
                        where c.Researcher.IDCard == idCard
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        public string GetNextNumber()
        {
            var query = _userRepository.Table;
            int maxNumber = query.LastOrDefault() != null ? query.LastOrDefault().Id : 0;
            //int? maxNumber = query.Max(e => (int?)e.Id);
            maxNumber += 1;
            return $"stf-{maxNumber.ToString("D4")}";
        }

        public UserRole GetUserRoleBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            //var key = string.Format(ResearchUserServiceDefaults.UserRolesBySystemNameCacheKey, systemName);
            //return _cacheManager.Get(key, () =>
            //{
                var query = from cr in _userRoleRepository.Table
                            orderby cr.Id
                            where cr.SystemName == systemName
                            select cr;
                var userRole = query.FirstOrDefault();
                return userRole;
            //});
        }

        public UserRole GetRoleById(int roleId)
        {
           if (roleId == 0)
                return null;
            return _userRoleRepository.GetById(roleId);
        }

        #endregion

        #endregion
    }
}