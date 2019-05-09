using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Research.Data;
using Research.Services;
using Research.Services.Logging;
using Research.Services.Security;
using Research.Web.Factories;
using Research.Web.Framework.Mapper.Extensions;
using Research.Web.Framework.Mvc.Filters;
using Research.Web.Models.Researchers;

namespace Research.Web.Controllers
{ 
    public partial class ResearcherController : BaseAdminController
    {
        #region Fields

        private readonly IUserActivityService _userActivityService;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IResearcherModelFactory _researcherModelFactory;
        private readonly IResearcherService _researcherService;

        #endregion Fields

        #region Ctor

        public ResearcherController(IUserActivityService userActivityService,
            IUserService userService,
            IPermissionService permissionService,
            IResearcherModelFactory researcherModelFactory,
            IResearcherService researcherService)
        {
            this._userActivityService = userActivityService;
            this._userService = userService;
            this._permissionService = permissionService;
            this._researcherModelFactory = researcherModelFactory;
            this._researcherService = researcherService;
        }

        #endregion

        #region Utilities
        
        //protected virtual void UpdateLocales(Researcher researcher, ResearcherModel model)
        //{
        //    foreach (var localized in model.Locales)
        //    {
        //        _localizedEntityService.SaveLocalizedValue(researcher,
        //            x => x.Title,
        //            localized.Title,
        //            localized.LanguageId);

        //        _localizedEntityService.SaveLocalizedValue(researcher,
        //            x => x.Body,
        //            localized.Body,
        //            localized.LanguageId);

        //        _localizedEntityService.SaveLocalizedValue(researcher,
        //            x => x.MetaKeywords,
        //            localized.MetaKeywords,
        //            localized.LanguageId);

        //        _localizedEntityService.SaveLocalizedValue(researcher,
        //            x => x.MetaDescription,
        //            localized.MetaDescription,
        //            localized.LanguageId);

        //        _localizedEntityService.SaveLocalizedValue(researcher,
        //            x => x.MetaTitle,
        //            localized.MetaTitle,
        //            localized.LanguageId);

        //        //search engine name
        //        var seName = researcher.ValidateSeName(localized.SeName, localized.Title, false);
        //        _urlRecordService.SaveSlug(researcher, seName, localized.LanguageId);
        //    }
        //}

        //protected virtual void SaveResearcherAcl(Researcher researcher, ResearcherModel model)
        //{
        //    researcher.SubjectToAcl = model.SelectedUserRoleIds.Any();

        //    var existingAclRecords = _aclService.GetAclRecords(researcher);
        //    var allUserRoles = _userService.GetAllUserRoles(true);
        //    foreach (var userRole in allUserRoles)
        //    {
        //        if (model.SelectedUserRoleIds.Contains(userRole.Id))
        //        {
        //            //new role
        //            if (existingAclRecords.Count(acl => acl.UserRoleId == userRole.Id) == 0)
        //                _aclService.InsertAclRecord(researcher, userRole.Id);
        //        }
        //        else
        //        {
        //            //remove role
        //            var aclRecordToDelete = existingAclRecords.FirstOrDefault(acl => acl.UserRoleId == userRole.Id);
        //            if (aclRecordToDelete != null)
        //                _aclService.DeleteAclRecord(aclRecordToDelete);
        //        }
        //    }
        //}

        //protected virtual void SaveStoreMappings(Researcher researcher, ResearcherModel model)
        //{
        //    researcher.LimitedToStores = model.SelectedStoreIds.Any();

        //    var existingStoreMappings = _storeMappingService.GetStoreMappings(researcher);
        //    var allStores = _storeService.GetAllStores();
        //    foreach (var store in allStores)
        //    {
        //        if (model.SelectedStoreIds.Contains(store.Id))
        //        {
        //            //new store
        //            if (existingStoreMappings.Count(sm => sm.StoreId == store.Id) == 0)
        //                _storeMappingService.InsertStoreMapping(researcher, store.Id);
        //        }
        //        else
        //        {
        //            //remove store
        //            var storeMappingToDelete = existingStoreMappings.FirstOrDefault(sm => sm.StoreId == store.Id);
        //            if (storeMappingToDelete != null)
        //                _storeMappingService.DeleteStoreMapping(storeMappingToDelete);
        //        }
        //    }
        //}

        #endregion

        #region List

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
            //    return AccessDeniedView();

            //prepare model
            var model = _researcherModelFactory.PrepareResearcherSearchModel(new ResearcherSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ResearcherSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
            //    return AccessDeniedKendoGridJson();

            //prepare model
            var model = _researcherModelFactory.PrepareResearcherListModel(searchModel);

            return Json(model);
        }

        #endregion

        #region Create / Edit / Delete

        public virtual IActionResult Create()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
            //    return AccessDeniedView();

            //prepare model
            var model = _researcherModelFactory.PrepareResearcherModel(new ResearcherModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ResearcherModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
            //    return AccessDeniedView();

            if (ModelState.IsValid)
            {

                var researcher = model.ToEntity<Researcher>();
                _researcherService.InsertResearcher(researcher);

                SuccessNotification("Admin.ContentManagement.Researchers.Added");

                //activity log
                _userActivityService.InsertActivity("AddNewResearcher", "ActivityLog.AddNewResearcher", researcher);

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = researcher.Id });
            }

            //prepare model
            model = _researcherModelFactory.PrepareResearcherModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
            //    return AccessDeniedView();

            //try to get a researcher with the specified id
            var researcher = _researcherService.GetResearcherById(id);
            if (researcher == null)
                return RedirectToAction("List");

            //prepare model
            var model = _researcherModelFactory.PrepareResearcherModel(null, researcher);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(ResearcherModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
            //    return AccessDeniedView();

            //try to get a researcher with the specified id
            var researcher = _researcherService.GetResearcherById(model.Id);
            if (researcher == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                researcher = model.ToEntity(researcher);
                _researcherService.UpdateResearcher(researcher);

                SuccessNotification("Researcher Updated");

                //activity log
                _userActivityService.InsertActivity("EditResearcher","ActivityLog EditResearcher", researcher);

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = researcher.Id });
            }

            //prepare model
            model = _researcherModelFactory.PrepareResearcherModel(model, researcher, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Info(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
            //    return AccessDeniedView();

            //try to get a researcher with the specified id
            var researcher = _researcherService.GetResearcherById(id);
            if (researcher == null)
                return RedirectToAction("List");

            //prepare model
            var model = _researcherModelFactory.PrepareResearcherModel(null, researcher);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
            //    return AccessDeniedView();

            //try to get a researcher with the specified id
            var researcher = _researcherService.GetResearcherById(id);
            if (researcher == null)
                return RedirectToAction("List");

            _researcherService.DeleteResearcher(researcher);

            SuccessNotification("Researchers Deleted");

            //activity log
            _userActivityService.InsertActivity("DeleteResearcher","ActivityLog.DeleteResearcher", researcher);

            return RedirectToAction("List");
        }

        #endregion
    }
}