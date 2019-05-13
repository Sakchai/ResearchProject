using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Research.Core;
using Research.Core.Domain;
using Research.Core.Http;
using Research.Data;
using Research.Services;
using Research.Services.Authentication;
using Research.Services.Helpers;
using Research.Services.Researchers;
using Research.Services.Tasks;

namespace Research.Web.Framework
{
    /// <summary>
    /// Represents work context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region Fields

        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserAgentHelper _userAgentHelper;
        private readonly IResearcherService _researcherService;

        private User _cachedUser;
        private User _originalUserIfImpersonated;
        private Researcher _cachedResearcher;

        #endregion

        #region Ctor

        public WebWorkContext(IAuthenticationService authenticationService,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor,
            IUserAgentHelper userAgentHelper,
            IResearcherService researcherService)
        {
            this._authenticationService = authenticationService;
            this._userService = userService;
            this._httpContextAccessor = httpContextAccessor;
            this._userAgentHelper = userAgentHelper;
            this._researcherService = researcherService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get nop user cookie
        /// </summary>
        /// <returns>String value of cookie</returns>
        protected virtual string GetUserCookie()
        {
            var cookieName = $"{ResearchCookieDefaults.Prefix}{ResearchCookieDefaults.UserCookie}";
            return _httpContextAccessor.HttpContext?.Request?.Cookies[cookieName];
        }

        /// <summary>
        /// Set nop user cookie
        /// </summary>
        /// <param name="userGuid">Guid of the user</param>
        protected virtual void SetUserCookie(Guid userGuid)
        {
            if (_httpContextAccessor.HttpContext?.Response == null)
                return;

            //delete current cookie value
            var cookieName = $"{ResearchCookieDefaults.Prefix}{ResearchCookieDefaults.UserCookie}";
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

            //get date of cookie expiration
            var cookieExpires = 24 * 365; //TODO make configurable
            var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

            //if passed guid is empty set cookie as expired
            if (userGuid == Guid.Empty)
                cookieExpiresDate = DateTime.Now.AddMonths(-1);

            //set new cookie value
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpiresDate
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, userGuid.ToString(), options);
        }

 

 
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        public virtual User CurrentUser
        {
            get
            {
                //whether there is a cached value
                if (_cachedUser != null)
                    return _cachedUser;

                User user = null;

                //check whether request is made by a background (schedule) task
                if (_httpContextAccessor.HttpContext == null ||
                    _httpContextAccessor.HttpContext.Request.Path.Equals(new PathString($"/{ResearchTaskDefaults.ScheduleTaskPath}"), StringComparison.InvariantCultureIgnoreCase))
                {
                    //in this case return built-in user record for background task
                    user = _userService.GetUserByUsername(ResearchUserDefaults.BackgroundTaskUserName);
                }

                if (user == null || user.Deleted || !user.IsActive || user.RequireReLogin)
                {
                    //check whether request is made by a search engine, in this case return built-in user record for search engines
                    if (_userAgentHelper.IsSearchEngine())
                        user = _userService.GetUserByUsername(ResearchUserDefaults.SearchEngineUserName);
                }

                if (user == null || user.Deleted || !user.IsActive || user.RequireReLogin)
                {
                    //try to get registered user
                    user = _authenticationService.GetAuthenticatedUser();
                }

                //if (user != null && !user.Deleted && user.IsActive && !user.RequireReLogin)
                //{
                //    //get impersonate user if required
                //    var impersonatedUserId = _genericAttributeService
                //        .GetAttribute<int?>(user, ResearchUserDefaults.ImpersonatedUserIdAttribute);
                //    if (impersonatedUserId.HasValue && impersonatedUserId.Value > 0)
                //    {
                //        var impersonatedUser = _userService.GetUserById(impersonatedUserId.Value);
                //        if (impersonatedUser != null && !impersonatedUser.Deleted && impersonatedUser.IsActive && !impersonatedUser.RequireReLogin)
                //        {
                //            //set impersonated user
                //            _originalUserIfImpersonated = user;
                //            user = impersonatedUser;
                //        }
                //    }
                //}

                if (user == null || user.Deleted || !user.IsActive || user.RequireReLogin)
                {
                    //get guest user
                    var userCookie = GetUserCookie();
                    if (!string.IsNullOrEmpty(userCookie))
                    {
                        if (Guid.TryParse(userCookie, out Guid userGuid))
                        {
                            //get user from cookie (should not be registered)
                            var userByCookie = _userService.GetUserByGuid(userGuid);
                            if (userByCookie != null && !userByCookie.IsRegistered())
                                user = userByCookie;
                        }
                    }
                }

                if (user == null || user.Deleted || !user.IsActive || user.RequireReLogin)
                {
                    //create guest if not exists
                    user = _userService.InsertGuestUser();
                }

                if (!user.Deleted && user.IsActive && !user.RequireReLogin)
                {
                    //set user cookie
                    SetUserCookie(user.UserGuid);

                    //cache the found user
                    _cachedUser = user;
                }

                return _cachedUser;
            }
            set
            {
                SetUserCookie(value.UserGuid);
                _cachedUser = value;
            }
        }

        /// <summary>
        /// Gets the original user (in case the current one is impersonated)
        /// </summary>
        public virtual User OriginalUserIfImpersonated
        {
            get { return _originalUserIfImpersonated; }
        }

        /// <summary>
        /// Gets the current researcher (logged-in manager)
        /// </summary>
        public virtual Researcher CurrentResearcher
        {
            get
            {
                //whether there is a cached value
                if (_cachedResearcher != null)
                    return _cachedResearcher;

                if (this.CurrentUser == null)
                    return null;

                //try to get researcher
                var researcher = _researcherService.GetResearcherById(this.CurrentUser.ResearcherId);

                //check researcher availability
                if (researcher == null || researcher.Deleted || !researcher.IsActive)
                    return null;

                //cache the found researcher
                _cachedResearcher = researcher;

                return _cachedResearcher;
            }
        }

  

        /// <summary>
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        public virtual bool IsAdmin { get; set; }

        #endregion
    }
}