using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Research.Core;
using Research.Core.Caching;
using Research.Core.Domain.Common;
using Research.Services.Common;
using Research.Services.Logging;
using Research.Services.Messages;

namespace Research.Web.Controllers
{
    public partial class CommonController : BasePublicController
    {
        #region Fields


        private readonly IWorkContext _workContext;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IUserActivityService _userActivityService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly ILogger _logger;
        private readonly CommonSettings _commonSettings;
        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public CommonController(IWorkContext workContext,
            IGenericAttributeService genericAttributeService,
            IUserActivityService userActivityService,
            IWorkflowMessageService workflowMessageService,
            ILogger logger,
            CommonSettings commonSettings,
            IStaticCacheManager cacheManager)
        {
            this._workContext = workContext;
            this._genericAttributeService = genericAttributeService;
            this._userActivityService = userActivityService;
            this._workflowMessageService = workflowMessageService;
            this._logger = logger;
            this._commonSettings = commonSettings;
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Methods
        
        //page not found
        public virtual IActionResult PageNotFound()
        {
            if (_commonSettings.Log404Errors)
            {
                var statusCodeReExecuteFeature = HttpContext?.Features?.Get<IStatusCodeReExecuteFeature>();
                //TODO add locale resource
                _logger.Error($"Error 404. The requested page ({statusCodeReExecuteFeature?.OriginalPath}) was not found", 
                    user: _workContext.CurrentUser);
            }

            Response.StatusCode = 404;
            Response.ContentType = "text/html";

            return View();
        }

        [HttpPost]
        public virtual IActionResult ClearCache(string returnUrl = "")
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageMaintenance))
            //    return AccessDeniedView();

            _cacheManager.Clear();

            //home page
            //if (string.IsNullOrEmpty(returnUrl))
            //    return RedirectToAction("Index", "Home", new { area = AreaNames.Admin });

            ////prevent open redirection attack
            //if (!Url.IsLocalUrl(returnUrl))
            //    return RedirectToAction("Index", "Home", new { area = AreaNames.Admin });

            return Redirect(returnUrl);
        }

        public virtual IActionResult GenericUrl()
        {
            //seems that no entity was found
            return InvokeHttp404();
        }

        //store is closed
        //available even when a store is closed
     //   [CheckAccessClosedStore(true)]
        public virtual IActionResult StoreClosed()
        {
            return View();
        }

        //helper method to redirect users. Workaround for GenericPathRoute class where we're not allowed to do it
        public virtual IActionResult InternalRedirect(string url, bool permanentRedirect)
        {
            //ensure it's invoked from our GenericPathRoute class
            if (HttpContext.Items["nop.RedirectFromGenericPathRoute"] == null ||
                !Convert.ToBoolean(HttpContext.Items["nop.RedirectFromGenericPathRoute"]))
            {
                url = Url.RouteUrl("HomePage");
                permanentRedirect = false;
            }

            //home page
            if (string.IsNullOrEmpty(url))
            {
                url = Url.RouteUrl("HomePage");
                permanentRedirect = false;
            }

            //prevent open redirection attack
            if (!Url.IsLocalUrl(url))
            {
                url = Url.RouteUrl("HomePage");
                permanentRedirect = false;
            }

            if (permanentRedirect)
                return RedirectPermanent(url);

            return Redirect(url);
        }

        #endregion
    }
}