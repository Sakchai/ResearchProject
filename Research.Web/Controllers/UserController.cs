using Microsoft.AspNetCore.Mvc;
using Research.Core;
using Research.Core.Domain.Users;
using Research.Enum;
using Research.Services;
using Research.Services.Authentication;
using Research.Services.Events;
using Research.Services.Users;
using Research.Web.Factories;
using Research.Web.Models.Users;

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

        #endregion

        #region Ctor

        public UserController(UserSettings userSettings,
            IAuthenticationService authenticationService,
            IUserModelFactory userModelFactory,
            IUserRegistrationService userRegistrationService,
            IUserService userService,
            IEventPublisher eventPublisher,
            IWebHelper webHelper)
        {
            this._userSettings = userSettings;
            this._authenticationService = authenticationService;
            this._userModelFactory = userModelFactory;
            this._userRegistrationService = userRegistrationService;
            this._userService = userService;
            this._eventPublisher = eventPublisher;
            this._webHelper = webHelper;
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

        [HttpPost]
        //[ValidateCaptcha]
        //available even when navigation is not allowed
        //[CheckAccessPublicStore(true)]
        //[PublicAntiForgery]
        public virtual IActionResult Login(LoginModel model, string returnUrl, bool captchaValid)
        {

            if (ModelState.IsValid)
            {
                if (_userSettings.UsernamesEnabled && model.UserName != null)
                {
                    model.UserName = model.UserName.Trim();
                }
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
                           // _userActivityService.InsertActivity(user, "PublicStore.Login","ActivityLog.PublicStore.Login", user);

                            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                                return RedirectToRoute("HomePage");

                            return Redirect(returnUrl);
                        }
                    case UserLoginResults.UserNotExist:
                        ModelState.AddModelError("", "UserNotExist");
                        break;
                    case UserLoginResults.Deleted:
                        ModelState.AddModelError("", "Account.Login.WrongCredentials.Deleted");
                        break;
                    case UserLoginResults.NotActive:
                        ModelState.AddModelError("", "Account.Login.WrongCredentials.NotActive");
                        break;
                    case UserLoginResults.NotRegistered:
                        ModelState.AddModelError("", "Account.Login.WrongCredentials.NotRegistered");
                        break;
                    case UserLoginResults.LockedOut:
                        ModelState.AddModelError("", "Account.Login.WrongCredentials.LockedOut");
                        break;
                    case UserLoginResults.WrongPassword:
                    default:
                        ModelState.AddModelError("", "Account.Login.WrongCredentials");
                        break;
                }
            }

            //If we got this far, something failed, redisplay form
            model = _userModelFactory.PrepareLoginModel(model.CheckoutAsGuest);
            return View(model);
        }

  

        #endregion



 

        #endregion
    }
}