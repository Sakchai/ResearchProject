using System;
using Microsoft.AspNetCore.Mvc;
using Research.Services.Logging;
using Research.Services.Security;
using Research.Services.Tasks;
using Research.Web.Controllers;
using Research.Web.Factories;
using Research.Web.Framework.Kendoui;
using Research.Web.Framework.Mvc;
using Research.Web.Models.Tasks;

namespace Research.Web.Controllers
{
    public partial class ScheduleTaskController : BaseAdminController
    {
        #region Fields

        private readonly IUserActivityService _userActivityService;
        private readonly IPermissionService _permissionService;
        private readonly IScheduleTaskModelFactory _scheduleTaskModelFactory;
        private readonly IScheduleTaskService _scheduleTaskService;

        #endregion

        #region Ctor

        public ScheduleTaskController(IUserActivityService userActivityService,
            IPermissionService permissionService,
            IScheduleTaskModelFactory scheduleTaskModelFactory,
            IScheduleTaskService scheduleTaskService)
        {
            this._userActivityService = userActivityService;
            this._permissionService = permissionService;
            this._scheduleTaskModelFactory = scheduleTaskModelFactory;
            this._scheduleTaskService = scheduleTaskService;
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageScheduleTasks))
            //    return AccessDeniedView();

            //prepare model
            var model = _scheduleTaskModelFactory.PrepareScheduleTaskSearchModel(new ScheduleTaskSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ScheduleTaskSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageScheduleTasks))
            //    return AccessDeniedKendoGridJson();

            //prepare model
            var model = _scheduleTaskModelFactory.PrepareScheduleTaskListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult TaskUpdate(ScheduleTaskModel model)
        {
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageScheduleTasks))
        //        return AccessDeniedView();

            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });

            //try to get a schedule task with the specified id
            var scheduleTask = _scheduleTaskService.GetTaskById(model.Id)
                               ?? throw new ArgumentException("Schedule task cannot be loaded");

            scheduleTask.Name = model.Name;
            scheduleTask.Seconds = model.Seconds;
            scheduleTask.Enabled = model.Enabled;
            scheduleTask.StopOnError = model.StopOnError;
            _scheduleTaskService.UpdateTask(scheduleTask);

            //activity log
            _userActivityService.InsertActivity("EditTask",
                string.Format("ActivityLog.EditTask {0}", scheduleTask.Id), scheduleTask);

            return new NullJsonResult();
        }

        public virtual IActionResult RunNow(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageScheduleTasks))
            //    return AccessDeniedView();

            try
            {
                //try to get a schedule task with the specified id
                var scheduleTask = _scheduleTaskService.GetTaskById(id)
                                   ?? throw new ArgumentException("Schedule task cannot be loaded", nameof(id));

                //ensure that the task is enabled
                var task = new Task(scheduleTask) { Enabled = true };
                task.Execute(true, false);

                SuccessNotification("Admin.System.ScheduleTasks.RunNow.Done");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
            }

            return RedirectToAction("List");
        }

        #endregion
    }
}