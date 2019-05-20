using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Research.Data;
using Research.Services;
using Research.Services.Logging;
using Research.Services.Media;
using Research.Services.Security;
using Research.Web.Factories;
using Research.Web.Extensions;
using Research.Web.Framework.Mvc.Filters;
using Research.Web.Models.FiscalSchedules;
using Research.Services.Common;
using Research.Web.Framework.Mvc;
using Research.Services.FiscalSchedules;

namespace Research.Web.Controllers
{ 
    public partial class FiscalScheduleController : BaseAdminController
    {
        #region Fields

        private readonly IUserActivityService _userActivityService;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IFiscalScheduleModelFactory _fiscalScheduleModelFactory;
        private readonly IFiscalScheduleService _fiscalScheduleService;

        #endregion Fields

        #region Ctor

        public FiscalScheduleController(IUserActivityService userActivityService,
            IUserService userService,
            IPermissionService permissionService,
            IFiscalScheduleModelFactory fiscalScheduleModelFactory,
            IFiscalScheduleService fiscalScheduleService)
        {
            this._userActivityService = userActivityService;
            this._userService = userService;
            this._permissionService = permissionService;
            this._fiscalScheduleModelFactory = fiscalScheduleModelFactory;
            this._fiscalScheduleService = fiscalScheduleService;
        }

        #endregion

        #region List

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageFiscalSchedules))
            //    return AccessDeniedView();

            //prepare model
            var model = _fiscalScheduleModelFactory.PrepareFiscalScheduleSearchModel(new FiscalScheduleSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(FiscalScheduleSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageFiscalSchedules))
            //    return AccessDeniedKendoGridJson();

            //prepare model
            var model = _fiscalScheduleModelFactory.PrepareFiscalScheduleListModel(searchModel);

            return Json(model);
        }

        #endregion

        #region Create / Edit / Delete
        [HttpGet, ActionName("Create")]
        public virtual IActionResult Create()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageFiscalSchedules))
            //    return AccessDeniedView();

            //prepare model
            var model = _fiscalScheduleModelFactory.PrepareFiscalScheduleModel(new FiscalScheduleModel(), null);

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(FiscalScheduleModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageFiscalSchedules))
            //    return AccessDeniedView();

            if (ModelState.IsValid)
            {

                var fiscalSchedule = model.ToEntity<FiscalSchedule>();
                fiscalSchedule.FiscalYear = model.FiscalYear;
                fiscalSchedule.FiscalCode = model.FiscalCode;
                fiscalSchedule.FiscalTimes = model.FiscalTimes;
                fiscalSchedule.ScholarName = model.ScholarName;
                fiscalSchedule.ClosingDate = Convert.ToDateTime(model.ClosingDate);
                fiscalSchedule.OpeningDate = Convert.ToDateTime(model.OpeningDate);
                _fiscalScheduleService.InsertFiscalSchedule(fiscalSchedule);

                return continueEditing ? RedirectToAction("Edit", new { fiscalSchedule.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _fiscalScheduleModelFactory.PrepareFiscalScheduleModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }


        public virtual IActionResult Edit(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageFiscalSchedules))
            //    return AccessDeniedView();

            //try to get a fiscalSchedule with the specified id
            var fiscalSchedule = _fiscalScheduleService.GetFiscalScheduleById(id);
            if (fiscalSchedule == null)
                return RedirectToAction("List");

            //prepare model
            var model = _fiscalScheduleModelFactory.PrepareFiscalScheduleModel(null, fiscalSchedule);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(FiscalScheduleModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageFiscalSchedules))
            //    return AccessDeniedView();

            //try to get a fiscalSchedule with the specified id
            var fiscalSchedule = _fiscalScheduleService.GetFiscalScheduleById(model.Id);
            if (fiscalSchedule == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                fiscalSchedule = model.ToEntity(fiscalSchedule);
                fiscalSchedule.FiscalYear = model.FiscalYear;
                fiscalSchedule.FiscalCode = model.FiscalCode;
                fiscalSchedule.FiscalTimes = model.FiscalTimes;
                fiscalSchedule.ScholarName = model.ScholarName;
                fiscalSchedule.ClosingDate = Convert.ToDateTime(model.ClosingDate);
                fiscalSchedule.OpeningDate = Convert.ToDateTime(model.OpeningDate);

                _fiscalScheduleService.UpdateFiscalSchedule(fiscalSchedule);

                return continueEditing ? RedirectToAction("Edit", new { fiscalSchedule.Id }) : RedirectToAction("List");

            }

            //prepare model
            model = _fiscalScheduleModelFactory.PrepareFiscalScheduleModel(model, fiscalSchedule, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Info(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageFiscalSchedules))
            //    return AccessDeniedView();

            //try to get a fiscalSchedule with the specified id
            var fiscalSchedule = _fiscalScheduleService.GetFiscalScheduleById(id);
            if (fiscalSchedule == null)
                return RedirectToAction("List");

            //prepare model
            var model = _fiscalScheduleModelFactory.PrepareFiscalScheduleModel(null, fiscalSchedule);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageFiscalSchedules))
            //    return AccessDeniedView();

            //try to get a fiscalSchedule with the specified id
            var fiscalSchedule = _fiscalScheduleService.GetFiscalScheduleById(id)
                ?? throw new ArgumentException("ไม่พบวันเปิดรับข้อเสนอโครงการวิจัย");

            fiscalSchedule.Deleted = true;
            _fiscalScheduleService.UpdateFiscalSchedule(fiscalSchedule);

            return new NullJsonResult();
        }

        #endregion

    }
}