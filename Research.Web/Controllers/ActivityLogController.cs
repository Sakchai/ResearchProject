using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Research.Services.Logging;
using Research.Services.Security;
using Research.Web.Areas.Admin.Factories;
using Research.Web.Framework.Mvc;
using Research.Web.Models.Logging;

namespace Research.Web.Controllers
{
    public partial class ActivityLogController : BaseAdminController
    {
        #region Fields

        private readonly IActivityLogModelFactory _activityLogModelFactory;
        private readonly IUserActivityService _userActivityService;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public ActivityLogController(IActivityLogModelFactory activityLogModelFactory,
            IUserActivityService userActivityService,
            IPermissionService permissionService)
        {
            this._activityLogModelFactory = activityLogModelFactory;
            this._userActivityService = userActivityService;
            this._permissionService = permissionService;
        }

        #endregion

        #region Methods

        public virtual IActionResult ListTypes()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
            //    return AccessDeniedView();

            //prepare model
            var model = _activityLogModelFactory.PrepareActivityLogTypeModels();

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult SaveTypes(IFormCollection form)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
            //    return AccessDeniedView();

            //activity log
            _userActivityService.InsertActivity("EditActivityLogTypes", "ActivityLog.EditActivityLogTypes");

            //get identifiers of selected activity types
            var selectedActivityTypesIds = form["checkbox_activity_types"]
                .SelectMany(value => value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(idString => int.TryParse(idString, out var id) ? id : 0)
                .Distinct().ToList();

            //update activity types
            var activityTypes = _userActivityService.GetAllActivityTypes();
            foreach (var activityType in activityTypes)
            {
                activityType.Enabled = selectedActivityTypesIds.Contains(activityType.Id);
                _userActivityService.UpdateActivityType(activityType);
            }

            SuccessNotification("Admin.Configuration.ActivityLog.ActivityLogType.Updated");

            return RedirectToAction("ListTypes");
        }

        public virtual IActionResult ListLogs()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
            //    return AccessDeniedView();

            //prepare model
            var model = _activityLogModelFactory.PrepareActivityLogSearchModel(new ActivityLogSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ListLogs(ActivityLogSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
            //    return AccessDeniedKendoGridJson();

            //prepare model
            var model = _activityLogModelFactory.PrepareActivityLogListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult AcivityLogDelete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
            //    return AccessDeniedView();

            //try to get a log item with the specified id
            var logItem = _userActivityService.GetActivityById(id)
                ?? throw new ArgumentException("No activity log found with the specified id", nameof(id));

            _userActivityService.DeleteActivity(logItem);

            //activity log
            _userActivityService.InsertActivity("DeleteActivityLog",
                "ActivityLog.DeleteActivityLog", logItem);

            return new NullJsonResult();
        }

        public virtual IActionResult ClearAll()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
            //    return AccessDeniedView();

            _userActivityService.ClearAllActivities();

            //activity log
            _userActivityService.InsertActivity("DeleteActivityLog", "ActivityLog.DeleteActivityLog");

            return RedirectToAction("ListLogs");
        }

        #endregion
    }
}