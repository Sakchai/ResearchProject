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
using Research.Web.Models.ResearchIssues;
using Research.Services.Common;
using Research.Web.Framework.Mvc;

namespace Research.Web.Controllers
{ 
    public partial class ResearchIssueController : BaseAdminController
    {
        #region Fields

        private readonly IUserActivityService _userActivityService;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IResearchIssueModelFactory _researchIssueModelFactory;
        private readonly IResearchIssueService _researchIssueService;

        #endregion Fields

        #region Ctor

        public ResearchIssueController(IUserActivityService userActivityService,
            IUserService userService,
            IPermissionService permissionService,
            IResearchIssueModelFactory researchIssueModelFactory,
            IResearchIssueService researchIssueService)
        {
            this._userActivityService = userActivityService;
            this._userService = userService;
            this._permissionService = permissionService;
            this._researchIssueModelFactory = researchIssueModelFactory;
            this._researchIssueService = researchIssueService;
        }

        #endregion

        #region List

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchIssues))
            //    return AccessDeniedView();

            //prepare model
            var model = _researchIssueModelFactory.PrepareResearchIssueSearchModel(new ResearchIssueSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ResearchIssueSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchIssues))
            //    return AccessDeniedKendoGridJson();

            //prepare model
            var model = _researchIssueModelFactory.PrepareResearchIssueListModel(searchModel);

            return Json(model);
        }

        #endregion

        #region Create / Edit / Delete
        [HttpGet, ActionName("Create")]
        public virtual IActionResult Create()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchIssues))
            //    return AccessDeniedView();

            //prepare model
            var model = _researchIssueModelFactory.PrepareResearchIssueModel(new ResearchIssueModel(), null);

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ResearchIssueModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchIssues))
            //    return AccessDeniedView();

            if (ModelState.IsValid)
            {

                var researchIssue = model.ToEntity<ResearchIssue>();
                researchIssue.FiscalYear = model.FiscalYear;
                researchIssue.IssueCode = model.IssueCode;
                researchIssue.Name = model.Name;
                _researchIssueService.InsertResearchIssue(researchIssue);

                return continueEditing ? RedirectToAction("Edit", new { researchIssue.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _researchIssueModelFactory.PrepareResearchIssueModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }


        public virtual IActionResult Edit(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchIssues))
            //    return AccessDeniedView();

            //try to get a researchIssue with the specified id
            var researchIssue = _researchIssueService.GetResearchIssueById(id);
            if (researchIssue == null)
                return RedirectToAction("List");

            //prepare model
            var model = _researchIssueModelFactory.PrepareResearchIssueModel(null, researchIssue);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(ResearchIssueModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchIssues))
            //    return AccessDeniedView();

            //try to get a researchIssue with the specified id
            var researchIssue = _researchIssueService.GetResearchIssueById(model.Id);
            if (researchIssue == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                researchIssue = model.ToEntity(researchIssue);
                researchIssue.FiscalYear = model.FiscalYear;
                researchIssue.IssueCode = model.IssueCode;
                researchIssue.Name = model.Name;
                _researchIssueService.UpdateResearchIssue(researchIssue);

                return continueEditing ? RedirectToAction("Edit", new { researchIssue.Id }) : RedirectToAction("List");

            }

            //prepare model
            model = _researchIssueModelFactory.PrepareResearchIssueModel(model, researchIssue, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Info(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchIssues))
            //    return AccessDeniedView();

            //try to get a researchIssue with the specified id
            var researchIssue = _researchIssueService.GetResearchIssueById(id);
            if (researchIssue == null)
                return RedirectToAction("List");

            //prepare model
            var model = _researchIssueModelFactory.PrepareResearchIssueModel(null, researchIssue);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchIssues))
            //    return AccessDeniedView();

            //try to get a researchIssue with the specified id
            var researchIssue = _researchIssueService.GetResearchIssueById(id)
                ?? throw new ArgumentException("ไม่พบรายการประเด็นการวิจัย");

            researchIssue.Deleted = true;
            _researchIssueService.UpdateResearchIssue(researchIssue);

            return new NullJsonResult();
        }

        #endregion

    }
}