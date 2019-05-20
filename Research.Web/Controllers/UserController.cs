﻿using Microsoft.AspNetCore.Mvc;
using Research.Core;
using Research.Core.Domain.Users;
using Research.Data;
using Research.Enum;
using Research.Services;
using Research.Services.Authentication;
using Research.Services.Events;
using Research.Services.Messages;
using Research.Services.Users;
using Research.Web.Extensions;
using Research.Web.Factories;
using Research.Web.Framework.Mvc;
using Research.Web.Framework.Mvc.Filters;
using Research.Web.Models.Users;
using System;

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
            IWorkflowMessageService workflowMessageService)
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
        }

        #endregion

        

        #region Methods

        #region Login / logout

        //[HttpsRequirement(SslRequirement.Yes)]
        //available even when navigation is not allowed
        //[CheckAccessPublicStore(true)]
        public virtual IActionResult Login(bool? checkoutAsGuest)
        {
            var model = _userModelFactory.PrepareLoginModel(checkoutAsGuest);
            return View(model);
        }

        public virtual IActionResult Register()
        {
            var model = new RegisterModel();
            var registerModel = _userModelFactory.PrepareRegisterModel(model);
            return View(registerModel);
        }

        public virtual IActionResult RegisterResult(int resultId)
        {
            var model = _userModelFactory.PrepareRegisterResultModel(resultId);
            return View(model);
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


                var isApproved = _userSettings.UserRegistrationType == UserRegistrationType.Standard;
                var registrationRequest = new UserRegistrationRequest(user,
                    model.Email,
                    model.Email,
                    model.Password,
                    _userSettings.DefaultPasswordFormat,
                    0,
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
                                _workflowMessageService.SendUserEmailValidationMessage(user, 0);

                                //result
                                return RedirectToRoute("RegisterResult",
                                    new { resultId = (int)UserRegistrationType.EmailValidation });
                            }
                        case UserRegistrationType.AdminApproval:
                            {
                                return RedirectToRoute("RegisterResult",
                                    new { resultId = (int)UserRegistrationType.AdminApproval });
                            }
                        case UserRegistrationType.Standard:
                            {
                                //send user welcome message
                                _workflowMessageService.SendUserWelcomeMessage(user, 0);

                                var redirectUrl = Url.RouteUrl("RegisterResult", new { resultId = (int)UserRegistrationType.Standard }, _webHelper.CurrentRequestProtocol);
                                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                                    redirectUrl = _webHelper.ModifyQueryString(redirectUrl, "returnurl", returnUrl);
                                return Redirect(redirectUrl);
                            }
                        default:
                            {
                                return RedirectToRoute("HomePage");
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

                var loginResult = _userRegistrationService.ValidateUser(_userSettings.UsernamesEnabled ? model.UserName : model.Email, model.Password);
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



        #endregion





        #endregion


        #region List

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
            //    return AccessDeniedView();

            //prepare model
            var model = _userModelFactory.PrepareUserSearchModel(new UserSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(UserSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
            //    return AccessDeniedKendoGridJson();

            //prepare model
            var model = _userModelFactory.PrepareUserListModel(searchModel);

            return Json(model);
        }

        #endregion

        #region Create / Edit / Delete
        [HttpGet, ActionName("Create")]
        public virtual IActionResult Create()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
            //    return AccessDeniedView();

            //prepare model
            var model = _userModelFactory.PrepareUserModel(new UserModel(), null);

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(UserModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
            //    return AccessDeniedView();

            if (ModelState.IsValid)
            {

                var user = model.ToEntity<User>();
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
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
            //    return AccessDeniedView();

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
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
            //    return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(model.Id);
            if (user == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
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
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
            //    return AccessDeniedView();

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
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
            //    return AccessDeniedView();

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