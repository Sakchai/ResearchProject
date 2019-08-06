using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Research.Web.Models;
using Research.Infrastructure;
using Research.Core;
using Research.Services.Logging;
using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Mvc.Razor;
using Research.Web.Framework.UI;
using Microsoft.Extensions.DependencyInjection;
using Research.Web.Framework.Kendoui;

namespace Research.Web.Framework.Controllers
{
    public class BaseController : Controller
    {
        internal void AddBreadcrumb(string displayName, string urlPath)
        {
            List<Message> messages;

            if (ViewBag.Breadcrumb == null)
            {
                messages = new List<Message>();
            }
            else
            {
                messages = ViewBag.Breadcrumb as List<Message>;
            }

            messages.Add(new Message { DisplayName = displayName, URLPath = urlPath });
            ViewBag.Breadcrumb = messages;
        }

        internal void AddPageHeader(string pageHeader = "", string pageDescription = "")
        {
            ViewBag.PageHeader = Tuple.Create(pageHeader, pageDescription);
        }

        internal enum PageAlertType
        {
            Error,
            Info,
            Warning,
            Success
        }

        internal void AddPageAlerts(PageAlertType pageAlertType, string description)
        {
            List<Message> messages;

            if (ViewBag.PageAlerts == null)
            {
                messages = new List<Message>();
            }
            else
            {
                messages = ViewBag.PageAlerts as List<Message>;
            }

            messages.Add(new Message { Type = pageAlertType.ToString().ToLower(), ShortDesc = description });
            ViewBag.PageAlerts = messages;
        }

        #region Rendering

        /// <summary>
        /// Render component to string
        /// </summary>
        /// <param name="componentName">Component name</param>
        /// <param name="arguments">Arguments</param>
        /// <returns>Result</returns>
        protected virtual string RenderViewComponentToString(string componentName, object arguments = null)
        {
            //original implementation: https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.ViewFeatures/Internal/ViewComponentResultExecutor.cs
            //we customized it to allow running from controllers

            //TODO add support for parameters (pass ViewComponent as input parameter)
            if (string.IsNullOrEmpty(componentName))
                throw new ArgumentNullException(nameof(componentName));

            var actionContextAccessor = HttpContext.RequestServices.GetService(typeof(IActionContextAccessor)) as IActionContextAccessor;
            if (actionContextAccessor == null)
                throw new Exception("IActionContextAccessor cannot be resolved");

            var context = actionContextAccessor.ActionContext;

            var viewComponentResult = ViewComponent(componentName, arguments);

            var viewData = this.ViewData;
            if (viewData == null)
            {
                throw new NotImplementedException();
                //TODO viewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState);
            }

            var tempData = this.TempData;
            if (tempData == null)
            {
                throw new NotImplementedException();
                //TODO tempData = _tempDataDictionaryFactory.GetTempData(context.HttpContext);
            }

            using (var writer = new StringWriter())
            {
                var viewContext = new ViewContext(
                    context,
                    NullView.Instance,
                    viewData,
                    tempData,
                    writer,
                    new HtmlHelperOptions());

                // IViewComponentHelper is stateful, we want to make sure to retrieve it every time we need it.
                var viewComponentHelper = context.HttpContext.RequestServices.GetRequiredService<IViewComponentHelper>();
                (viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);

                var result = viewComponentResult.ViewComponentType == null ?
                    viewComponentHelper.InvokeAsync(viewComponentResult.ViewComponentName, viewComponentResult.Arguments) :
                    viewComponentHelper.InvokeAsync(viewComponentResult.ViewComponentType, viewComponentResult.Arguments);

                result.Result.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <returns>Result</returns>
        protected virtual string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }

        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <returns>Result</returns>
        protected virtual string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }

        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Result</returns>
        protected virtual string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }

        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <param name="model">Model</param>
        /// <returns>Result</returns>
        protected virtual string RenderPartialViewToString(string viewName, object model)
        {
            //get Razor view engine
            var razorViewEngine = EngineContext.Current.Resolve<IRazorViewEngine>();

            //create action context
            var actionContext = new ActionContext(this.HttpContext, this.RouteData, this.ControllerContext.ActionDescriptor, this.ModelState);

            //set view name as action name in case if not passed
            if (string.IsNullOrEmpty(viewName))
                viewName = this.ControllerContext.ActionDescriptor.ActionName;

            //set model
            ViewData.Model = model;

            //try to get a view by the name
            var viewResult = razorViewEngine.FindView(actionContext, viewName, false);
            if (viewResult.View == null)
            {
                //or try to get a view by the path
                viewResult = razorViewEngine.GetView(null, viewName, false);
                if (viewResult.View == null)
                    throw new ArgumentNullException($"{viewName} view was not found");
            }
            using (var stringWriter = new StringWriter())
            {
                var viewContext = new ViewContext(actionContext, viewResult.View, ViewData, TempData, stringWriter, new HtmlHelperOptions());

                var t = viewResult.View.RenderAsync(viewContext);
                t.Wait();
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        #endregion

        #region Notifications

        /// <summary>
        /// Display success notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Display warning notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void WarningNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Warning, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="logException">A value indicating whether exception should be logged</param>
        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true, bool logException = true)
        {
            if (logException)
                LogException(exception);

            AddNotification(NotifyType.Error, exception.Message, persistForTheNextRequest);
        }

        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="exception">Exception</param>
        protected void LogException(Exception exception)
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var logger = EngineContext.Current.Resolve<ILogger>();

            var customer = workContext.CurrentUser;
            logger.Error(exception.Message, exception, customer);
        }

        /// <summary>
        /// Display notification
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            var dataKey = $"research.notifications.{type}";

            if (persistForTheNextRequest)
            {
                //1. Compare with null (first usage)
                //2. For some unknown reasons sometimes List<string> is converted to string[]. And it throws exceptions. That's why we reset it
                if (TempData[dataKey] == null || !(TempData[dataKey] is List<string>))
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                //1. Compare with null (first usage)
                //2. For some unknown reasons sometimes List<string> is converted to string[]. And it throws exceptions. That's why we reset it
                if (ViewData[dataKey] == null || !(ViewData[dataKey] is List<string>))
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }

        /// <summary>
        /// Error's JSON data for kendo grid
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <returns>Error's JSON data</returns>
        protected JsonResult ErrorForKendoGridJson(string errorMessage)
        {
            var gridModel = new DataSourceResult
            {
                Errors = errorMessage
            };

            return Json(gridModel);
        }

        /// <summary>
        /// Display "Edit" (manage) link (in public store)
        /// </summary>
        /// <param name="editPageUrl">Edit page URL</param>
        protected virtual void DisplayEditLink(string editPageUrl)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();

            pageHeadBuilder.AddEditPageUrl(editPageUrl);
        }

        #endregion

        #region Security

        /// <summary>
        /// Access denied view
        /// </summary>
        /// <returns>Access denied view</returns>
        protected virtual IActionResult AccessDeniedView()
        {
            var webHelper = EngineContext.Current.Resolve<IWebHelper>();

            //return Challenge();
            return RedirectToAction("AccessDenied", "Security", new { pageUrl = webHelper.GetRawUrl(this.Request) });
        }

        /// <summary>
        /// Access denied JSON data for kendo grid
        /// </summary>
        /// <returns>Access denied JSON data</returns>
        protected JsonResult AccessDeniedKendoGridJson()
        {
            return ErrorForKendoGridJson("Admin.AccessDenied.Description");
        }

        #endregion
    }
}