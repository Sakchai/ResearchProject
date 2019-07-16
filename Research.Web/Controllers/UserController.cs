using Microsoft.AspNetCore.Mvc;
using Research.Core;
using Research.Core.Domain.Users;
using Research.Data;
using Research.Enum;
using Research.Services;
using Research.Services.Authentication;
using Research.Services.Common;
using Research.Services.Events;
using Research.Services.Messages;
using Research.Services.Researchers;
using Research.Services.Security;
using Research.Services.Tasks;
using Research.Services.Users;
using Research.Web.Extensions;
using Research.Web.Factories;
using Research.Web.Framework.Controllers;
using Research.Web.Framework.Mvc;
using Research.Web.Framework.Mvc.Filters;
using Research.Web.Models.Users;
using System;
using System.Linq;

namespace Research.Web.Controllers
{
    public partial class UserController : BasePublicController
    {
        #region Fields

        private readonly UserSettings _userSettings;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserModelFactory _userModelFactory;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IUserService _userService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IResearcherService _researcherService;
        private readonly IAuthenticationService _cookieAuthenticationService;
        private readonly IPermissionService _permissionService;
        private readonly IScheduleTaskService _scheduleTaskService;
        #endregion

        #region Ctor

        public UserController(UserSettings userSettings,
            IAuthenticationService authenticationService,
            IUserModelFactory userModelFactory,
            IUserRegistrationService userRegistrationService,
            IUserService userService,
            IEventPublisher eventPublisher,
            IWebHelper webHelper,
            IWorkContext workContext,
            IWorkflowMessageService workflowMessageService,
            IGenericAttributeService genericAttributeService,
            IResearcherService researcherService,
            IAuthenticationService cookieAuthenticationService,
            IPermissionService permissionService,
            IScheduleTaskService scheduleTaskService)
        {
            this._userSettings = userSettings;
            this._authenticationService = authenticationService;
            this._userModelFactory = userModelFactory;
            this._userRegistrationService = userRegistrationService;
            this._userService = userService;
            this._eventPublisher = eventPublisher;
            this._webHelper = webHelper;
            this._workContext = workContext;
            this._workflowMessageService = workflowMessageService;
            this._genericAttributeService = genericAttributeService;
            this._researcherService = researcherService;
            this._cookieAuthenticationService = cookieAuthenticationService;
            this._permissionService = permissionService;
            this._scheduleTaskService = scheduleTaskService;
        }

        #endregion

        #region Methods
        #region Password recovery

        // [HttpsRequirement(SslRequirement.Yes)]
        //available even when navigation is not allowed
        // [CheckAccessPublicStore(true)]
        [HttpGet, ActionName("PasswordRecovery")]
        public virtual IActionResult PasswordRecovery()
        {
            var model = _userModelFactory.PreparePasswordRecoveryModel();
            return View(model);
        }

        [HttpPost, ActionName("PasswordRecovery")]
    //    [PublicAntiForgery]
        [FormValueRequired("send-email")]
        //available even when navigation is not allowed
      //  [CheckAccessPublicStore(true)]
        public virtual IActionResult PasswordRecovery(PasswordRecoveryModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByEmail(model.Email);
                if (user != null && user.IsActive && !user.Deleted)
                {
                    //save token and current date
                    var passwordRecoveryToken = Guid.NewGuid();
                    _genericAttributeService.SaveAttribute(user, ResearchUserDefaults.PasswordRecoveryTokenAttribute,
                        passwordRecoveryToken.ToString());
                    DateTime? generatedDateTime = DateTime.UtcNow;
                    _genericAttributeService.SaveAttribute(user,
                        ResearchUserDefaults.PasswordRecoveryTokenDateGeneratedAttribute, generatedDateTime);

                    //send email
                    _workflowMessageService.SendUserPasswordRecoveryMessage(user,0);
                    var scheduleTask = _scheduleTaskService.GetTaskById(1);
                    var task = new Task(scheduleTask) { Enabled = true };
                    task.Execute(true, false);
                    model.Result = "ระบบได้ส่งอีเมลเพื่อดำเนินการรีเซตรหัสผ่านเรียบร้อยแล้ว";
                }
                else
                {
                    model.Result = "ไม่พบอีเมลของท่านในระบบ";
                }

                return View(model);
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        //  [HttpsRequirement(SslRequirement.Yes)]
        //available even when navigation is not allowed
        //  [CheckAccessPublicStore(true)]
        [HttpGet, ActionName("PasswordRecoveryConfirm")]
        public virtual IActionResult PasswordRecoveryConfirm(string token, string email)
        {
            var user = _userService.GetUserByEmail(email);
            if (user == null)
                return RedirectToRoute("HomePage");

            if (string.IsNullOrEmpty(user.GetAttribute<string>(ResearchUserDefaults.PasswordRecoveryTokenAttribute)))
            {
                return View(new PasswordRecoveryConfirmModel
                {
                    DisablePasswordChanging = true,
                    Result = "Password already has been changed"
                });
            }

            var model = _userModelFactory.PreparePasswordRecoveryConfirmModel();

            //validate token
            if (!user.IsPasswordRecoveryTokenValid(token))
            {
                model.DisablePasswordChanging = true;
                model.Result = "Password reset wrong token";
            }

            //validate token expiration date
            if (user.IsPasswordRecoveryLinkExpired(_userSettings))
            {
                model.DisablePasswordChanging = true;
                model.Result = "Password reset Link Expired";
            }

            return View(model);
        }

        [HttpPost, ActionName("PasswordRecoveryConfirm")]
       // [PublicAntiForgery]
        [FormValueRequired("set-password")]
        //available even when navigation is not allowed
       // [CheckAccessPublicStore(true)]
        public virtual IActionResult PasswordRecoveryConfirmPOST(string token, string email, PasswordRecoveryConfirmModel model)
        {
            var user = _userService.GetUserByEmail(email);
            if (user == null)
                return RedirectToRoute("HomePage");

            //validate token
            if (!user.IsPasswordRecoveryTokenValid(token))
            {
                model.DisablePasswordChanging = true;
                model.Result = "Password reset wrong token";
                return View(model);
            }

            //validate token expiration date
            if (user.IsPasswordRecoveryLinkExpired(_userSettings))
            {
                model.DisablePasswordChanging = true;
                model.Result = "Password reset Link Expired";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var response = _userRegistrationService.ChangePassword(new ChangePasswordRequest(email,
                    false, _userSettings.DefaultPasswordFormat, model.NewPassword));
                if (response.Success)
                {
                    _genericAttributeService.SaveAttribute(user, ResearchUserDefaults.PasswordRecoveryTokenAttribute, "");

                    model.DisablePasswordChanging = true;
                    model.Result = "Password has Been Changed";
                }
                else
                {
                    model.Result = response.Errors.FirstOrDefault();
                }

                return View(model);
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion    


        #region Login / logout

        //[HttpsRequirement(SslRequirement.Yes)]
        //available even when navigation is not allowed
        //[CheckAccessPublicStore(true)]
        public virtual IActionResult Login(bool? checkoutAsGuest)
        {
            var model = _userModelFactory.PrepareLoginModel(checkoutAsGuest);
            return View(model);
        }

        public virtual IActionResult SignOut()
        {
            _cookieAuthenticationService.SignOut();
            return RedirectToAction("Login", "User");
        }

        public virtual IActionResult Register()
        {
            var model = new RegisterModel();
            var registerModel = _userModelFactory.PrepareRegisterModel(model);
            return View(registerModel);
        }

        [HttpGet, ActionName("RegisterResult")]
        public virtual IActionResult RegisterResult(int resultId)
        {
            var model = _userModelFactory.PrepareRegisterResultModel(resultId);
            return View(model);
        }

        public virtual IActionResult AccountActivation(string token, string email)
        {
            var user = _userService.GetUserByEmail(email);
            if (user == null)
                return RedirectToAction("Login", "User");

            var userAccountActivationAttribute = _genericAttributeService.GetAttributesForEntityByToken(user.Id, nameof(user), ResearchUserDefaults.AccountActivationTokenAttribute)
                                                .Where(x => x.Value.Contains(token)).FirstOrDefault();
            string cToken = userAccountActivationAttribute != null ? userAccountActivationAttribute.Value : string.Empty;
            var researcher = _researcherService.GetResearcherByEmail(email);
            if (researcher.IsCompleted)
                return
                    View(new AccountActivationModel
                    {
                        Result = "ระบบยืนยันอีเมลของเท่านเรียบร้อยแล้ว โปรดล็อคอินเข้าสู่ระบบ !"
                    });

            //if (!cToken.Equals(token, StringComparison.InvariantCultureIgnoreCase))
            if (string.IsNullOrEmpty(cToken))
                return RedirectToAction("Login", "User");

            //activate researcher account
            researcher.IsCompleted = true;
            researcher.Modified = DateTime.UtcNow;
            _researcherService.UpdateResearcher(researcher);
            user.Modified = DateTime.UtcNow;
            _userService.UpdateUser(user);
            _genericAttributeService.SaveAttribute(user, ResearchUserDefaults.AccountActivationTokenAttribute, "");
            
            //send welcome message
            _workflowMessageService.SendUserWelcomeMessage(user, 0);

            var model = new AccountActivationModel
            {
                Result = "ยินดีต้อนรับเข้าสู่ระบบสารสนเทศเพื่อการบริหารงานวิจัย ระบบได้ยืนยันอีเมลของเท่าน โปรดล็อคอินเข้าสู่ระบบ !"
            };
            return View(model);
        }

        //public virtual IActionResult PasswordRecoveryConfirm(string token, string email)
        //{
        //    var user = _userService.GetUserByEmail(email);
        //    if (user == null)
        //        return RedirectToRoute("HomePage");

        //    if (string.IsNullOrEmpty(user.PasswordRecoveryToken))
        //    {
        //        return View(new PasswordRecoveryConfirmModel
        //        {
        //            DisablePasswordChanging = true,
        //            Result = "Account.PasswordRecovery.PasswordAlreadyHasBeenChanged"
        //        });
        //    }

        //    var model = _userModelFactory.PreparePasswordRecoveryConfirmModel();

        //    //validate token
        //    if (!user.IsPasswordRecoveryTokenValid(token))
        //    {
        //        model.DisablePasswordChanging = true;
        //        model.Result = "Account.PasswordRecovery.WrongToken";
        //    }

        //    //validate token expiration date
        //    if (user.IsPasswordRecoveryLinkExpired(_userSettings))
        //    {
        //        model.DisablePasswordChanging = true;
        //        model.Result = "Account.PasswordRecovery.LinkExpired";
        //    }

        //    return View(model);
        //}


        [HttpPost]
        public virtual IActionResult RegisterResult(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                //return RedirectToRoute("User");
                return RedirectToAction("Login", "User");

            return Redirect(returnUrl);
            //return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public virtual IActionResult Register(RegisterModel model, string returnUrl)
        {
            var form = model.Form;
            //check whether registration is allowed
            if (_userSettings.UserRegistrationType == UserRegistrationType.Disabled)
                return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled });

            if (_workContext.CurrentUser.IsRegistered())
            {
                //Already registered user. 
                _authenticationService.SignOut();

                //raise logged out event       
                _eventPublisher.Publish(new UserLoggedOutEvent(_workContext.CurrentUser));

                //Save a new record
                _workContext.CurrentUser = _userService.InsertGuestUser();
            }
            var user = _workContext.CurrentUser;

            if (ModelState.IsValid)
            {
               // user.UserGuid = Guid.NewGuid();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.TitleId = model.TitleId;
                user.AgencyId = model.AgencyId;
                user.UserTypeId = (int) UserType.Researchers;
                user.Roles = UserType.Researchers.ToString();
                var isApproved = _userSettings.UserRegistrationType == UserRegistrationType.EmailValidation;
                var registrationRequest = new UserRegistrationRequest(user,
                    model.Email,
                    model.IDCard,
                    model.Password,
                    _userService.GetNextNumber(),
                    model.Gender,
                    _userSettings.DefaultPasswordFormat,
                    isApproved);
                var registrationResult = _userRegistrationService.RegisterUser(registrationRequest);
                if (registrationResult.Success)
                {
                    //login user now
                    if (isApproved)
                        _authenticationService.SignIn(user, true);

 
                    //raise event       
                    _eventPublisher.Publish(new UserRegisteredEvent(user));

                    switch (_userSettings.UserRegistrationType)
                    {
                        case UserRegistrationType.EmailValidation:
                            {
                                //email validation message
                                _genericAttributeService.SaveAttribute(user, ResearchUserDefaults.AccountActivationTokenAttribute, Guid.NewGuid().ToString());

                                _workflowMessageService.SendUserEmailValidationMessage(user, 0);
                                var scheduleTask = _scheduleTaskService.GetTaskById(1);
                                var task = new Task(scheduleTask) { Enabled = true };
                                task.Execute(true, false);
                                //result
                                return RedirectToAction("RegisterResult", "User", new { resultId = (int)UserRegistrationType.EmailValidation });
                                //return RedirectToRoute("RegisterResult",
                                //    new { resultId = (int)UserRegistrationType.EmailValidation });
                            }
                        case UserRegistrationType.AdminApproval:
                            {
                                return RedirectToRoute("RegisterResult",
                                    new { resultId = (int)UserRegistrationType.AdminApproval });
                            }
                        case UserRegistrationType.Standard:
                            {
                                //send user welcome message
                                //chai
                                _workflowMessageService.SendUserWelcomeMessage(user, 0);

                                var redirectUrl = Url.RouteUrl("RegisterResult", new { resultId = (int)UserRegistrationType.EmailValidation }, _webHelper.CurrentRequestProtocol);
                                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                                    redirectUrl = _webHelper.ModifyQueryString(redirectUrl, "returnurl", returnUrl);
                                //return Redirect(redirectUrl);

    
                                    return RedirectToAction("Login", "User");
                            }
                        default:
                            {
                                return RedirectToAction("Login", "User");
                                //return RedirectToRoute("HomePage");
                            }
                    }
                }

                //errors
                foreach (var error in registrationResult.Errors)
                    ModelState.AddModelError("", error);
            }

            //If we got this far, something failed, redisplay form
            model = _userModelFactory.PrepareRegisterModel(model);
            return View(model);
        }

        public virtual IActionResult UserAgreement(Guid userId)
        {
            var user = _userService.GetUserByGuid(userId);
            if (user == null)
                return RedirectToRoute("HomePage");

            var model = _userModelFactory.PrepareUserAgreementModel(userId);
            return View(model);
        }


        [HttpPost]
        //[ValidateCaptcha]
        //available even when navigation is not allowed
        //[CheckAccessPublicStore(true)]
        //[PublicAntiForgery]
        public virtual IActionResult Login(LoginModel model, string returnUrl, bool captchaValid)
        {

            if (ModelState.IsValid)
            {

                var loginResult = _userRegistrationService.ValidateUser(_userSettings.UsernamesEnabled ? 
                                    model.UserName : model.Email, model.Password);
                switch (loginResult)
                {
                    case UserLoginResults.Successful:
                        {
                            var user = _userSettings.UsernamesEnabled
                                ? _userService.GetUserByUsername(model.UserName)
                                : _userService.GetUserByEmail(model.Email);

                            //sign in new user
                            _authenticationService.SignIn(user, model.RememberMe);

                            //raise event       
                            _eventPublisher.Publish(new UserLoggedinEvent(user));

                            //activity log
                            //_userActivityService.InsertActivity(user, "PublicStore.Login","ActivityLog.PublicStore.Login", user);

                            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                                //return RedirectToRoute("Project");
                                return RedirectToAction("List","Project");

                            return Redirect(returnUrl);
                        }
                    case UserLoginResults.UserNotExist:
                        ModelState.AddModelError("", "ไม่มีผู้ใช้นี้ในระบบ");
                        break;
                    case UserLoginResults.Deleted:
                        ModelState.AddModelError("", "ผู้ใช้ถูกลบออกระบบไปแล้ว กรุณาลงทะเบียนใหม่");
                        break;
                    case UserLoginResults.NotActive:
                        ModelState.AddModelError("", "ผู้ใช้ไม่ Active");
                        break;
                    case UserLoginResults.NotRegistered:
                        ModelState.AddModelError("", "ผู้ใช้ยังไม่ได้ลงทะเบียน");
                        break;
                    case UserLoginResults.LockedOut:
                        ModelState.AddModelError("", "ผู้ใช้ Locked Out");
                        break;
                    case UserLoginResults.WrongPassword:
                    default:
                        ModelState.AddModelError("", "รหัสผ่านไม่ถูกต้องการ");
                        break;
                }
            }

            //If we got this far, something failed, redisplay form
            model = _userModelFactory.PrepareLoginModel(model.CheckoutAsGuest);
            return View(model);
        }


        //available even when a store is closed
      //  [CheckAccessClosedStore(true)]
        //available even when navigation is not allowed
      //  [CheckAccessPublicStore(true)]
        public virtual IActionResult Logout()
        {
            if (_workContext.OriginalUserIfImpersonated != null)
            {
                //activity log
                //_userActivityService.InsertActivity(_workContext.OriginalUserIfImpersonated, "Impersonation.Finished",
                //    string.Format(_localizationService.GetResource("ActivityLog.Impersonation.Finished.StoreOwner"),
                //        _workContext.CurrentUser.Email, _workContext.CurrentUser.Id),
                //    _workContext.CurrentUser);

                //_userActivityService.InsertActivity("Impersonation.Finished",
                //    string.Format("ActivityLog.Impersonation.Finished.User",
                //        _workContext.OriginalUserIfImpersonated.Email, _workContext.OriginalUserIfImpersonated.Id),
                //    _workContext.OriginalUserIfImpersonated);

                //logout impersonated user
                _genericAttributeService
                    .SaveAttribute<int?>(_workContext.OriginalUserIfImpersonated, ResearchUserDefaults.ImpersonatedUserIdAttribute, null);

                //redirect back to user details page (admin area)
                //return this.RedirectToAction("Edit", "User", new { id = _workContext.CurrentUser.Id, area = string.Empty });
                return this.RedirectToAction("Login", "User");
            }

            //activity log
            //_userActivityService.InsertActivity(_workContext.CurrentUser, "PublicStore.Logout",
            //    ("ActivityLog.PublicStore.Logout", _workContext.CurrentUser);

            //standard logout 
            _authenticationService.SignOut();

            //raise logged out event       
            _eventPublisher.Publish(new UserLoggedOutEvent(_workContext.CurrentUser));

  
            return RedirectToRoute("HomePage");
        }

        #endregion





        #endregion


        #region List

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //prepare model
            var model = _userModelFactory.PrepareUserSearchModel(new UserSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(UserSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _userModelFactory.PrepareUserListModel(searchModel);

            return Json(model);
        }

        #endregion

        #region Create / Edit / Delete
        [HttpGet, ActionName("Create")]
        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //prepare model
            var model = _userModelFactory.PrepareUserModel(new UserModel(), null);

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(UserModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var role = _userService.GetRoleById(model.UserTypeId);
                var user = model.ToEntity<User>();
                user.UserGuid = Guid.NewGuid();
                user.UserName = model.UserName;
                user.TitleId = model.TitleId;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.AgencyId = model.AgencyId;
                user.MobileNumber = model.MobileNumber;
                user.Email = model.Email;
                user.UserTypeId = model.UserTypeId;
                user.IsActive = model.IsActive;
                user.Description = model.Description;
                user.Roles = role.SystemName;
                _userService.InsertUser(user);

                return continueEditing ? RedirectToAction("Edit", new { user.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _userModelFactory.PrepareUserModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }


        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(id);
            if (user == null)
                return RedirectToAction("List");

            //prepare model
            var model = _userModelFactory.PrepareUserModel(null, user);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(UserModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(model.Id);
            
            if (user == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var role = _userService.GetRoleById(model.UserTypeId);
                user = model.ToEntity(user);
                user.UserName = model.UserName;
                user.TitleId = model.TitleId;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.AgencyId = model.AgencyId;
                user.MobileNumber = model.MobileNumber;
                user.Email = model.Email;
                user.UserTypeId = model.UserTypeId;
                user.IsActive = model.IsActive;
                user.Description = model.Description;
                user.Roles = role.SystemName;
                _userService.UpdateUser(user);

                return continueEditing ? RedirectToAction("Edit", new { user.Id }) : RedirectToAction("List");

            }

            //prepare model
            model = _userModelFactory.PrepareUserModel(model, user, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Info(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(id);
            if (user == null)
                return RedirectToAction("List");

            //prepare model
            var model = _userModelFactory.PrepareUserModel(null, user);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(id)
                ?? throw new ArgumentException("ไม่พบรายการผู้ใช้");

            user.Deleted = true;
            _userService.UpdateUser(user);

            return new NullJsonResult();
        }

        #endregion
    }
}