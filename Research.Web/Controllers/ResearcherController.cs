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
using Research.Web.Models.Researchers;
using Research.Services.Common;
using Research.Web.Framework.Mvc;
using Research.Web.Models.Common;
using Research.Services.Researchers;
using System.Collections.Generic;

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
        private readonly IPictureService _pictureService;
        private readonly IAddressService _addressService;
        private readonly IAcademicRankService _academicRankService;

        #endregion Fields

        #region Ctor

        public ResearcherController(IUserActivityService userActivityService,
            IUserService userService,
            IPermissionService permissionService,
            IResearcherModelFactory researcherModelFactory,
            IResearcherService researcherService,
            IPictureService pictureService,
            IAddressService addressService,
            IAcademicRankService academicRankService)
        {
            this._userActivityService = userActivityService;
            this._userService = userService;
            this._permissionService = permissionService;
            this._researcherModelFactory = researcherModelFactory;
            this._researcherService = researcherService;
            this._pictureService = pictureService;
            this._addressService = addressService;
            this._academicRankService = academicRankService;
        }

        #endregion

        #region Utilities

        [HttpPost]
        public virtual IActionResult ResearchersSelect()
        {
            //var model = new List<SelectListItem>();
            var model = new List<string>();
            var researchers = _researcherService.GetAllResearchers().ToList();
            foreach (var x in researchers)
            {
                string title = (x.Title != null) ? x.Title.TitleNameTH : string.Empty;
                //model.Add(new SelectListItem(item.Id.ToString(), $"{item.FirstName} {item.LastName}"));
                model.Add($"{title}{x.FirstName} {x.LastName}");
            }
            return Json(model);
        }

        [HttpGet, ActionName("GetAcademicRanksByPersonalTypeId")]
        public virtual IActionResult GetAcademicRanksByPersonalTypeId(string personalTypeId, bool? addSelectPersonalTypeItem)
        {
            //permission validation is not required here

            // This action method gets called via an ajax request
            if (string.IsNullOrEmpty(personalTypeId))
                throw new ArgumentNullException(nameof(personalTypeId));
            int id = Int32.Parse(personalTypeId);
            var result = _academicRankService.GetAcademicRanksByPersonalTypeId(id)
                                .Select(x => new { id = x.Id, name = x.NameTh }).ToList();

            return Json(result);
        }


        #endregion

        #region List

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
                return AccessDeniedView();

            //prepare model
            var model = _researcherModelFactory.PrepareResearcherSearchModel(new ResearcherSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ResearcherSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _researcherModelFactory.PrepareResearcherListModel(searchModel);

            return Json(model);
        }

        #endregion

        #region Create / Edit / Delete
        [HttpGet, ActionName("Create")]
        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
                return AccessDeniedView();

            //prepare model
            var model = _researcherModelFactory.PrepareResearcherModel(new ResearcherModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ResearcherModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {

                var researcher = model.ToEntity<Researcher>();

                if (model.ParseDateOfBirth() != null)
                    researcher.Birthdate = model.ParseDateOfBirth();
                researcher.IsActive = true;
                _researcherService.InsertResearcher(researcher);

                var address = model.AddressModel.ToEntity<Address>();
                SaveAddress(model.AddressModel, address);
                if (address.Id != 0)
                {
                    researcher.AddressId = address.Id;
                    _researcherService.UpdateResearcher(researcher);
                }


                SuccessNotification("Admin.ContentManagement.Researchers.Added");

                //activity log
                //_userActivityService.InsertActivity("AddNewResearcher", "ActivityLog.AddNewResearcher", researcher);

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

        private void SaveAddress(AddressModel model, Address address)
        {
            if ((model != null) && (address.Id == 0))
            {
                if (!string.IsNullOrEmpty(model.Address1) ||
                    !string.IsNullOrEmpty(model.Address2) ||
                    !string.IsNullOrEmpty(model.ZipCode) ||
                    model.ProvinceId != 0)
                _addressService.InsertAddress(address);
            }
            else
            {
                var addr = _addressService.GetAddressById(address.Id);
                addr.Address1 = model.Address1;
                addr.Address2 = model.Address2;
                addr.ProvinceId = model.ProvinceId;
                addr.ZipCode = model.ZipCode;
                _addressService.UpdateAddress(addr);
            }
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
                return AccessDeniedView();

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
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
                return AccessDeniedView();

            //try to get a researcher with the specified id
            var researcher = _researcherService.GetResearcherById(model.Id);
            bool active = researcher.IsActive;
            if (researcher == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                researcher = model.ToEntity(researcher);
                researcher.IsActive = active;
                if (model.ParseDateOfBirth() != null)
                    researcher.Birthdate = model.ParseDateOfBirth();

                _researcherService.UpdateResearcher(researcher);

                var address = model.AddressModel.ToEntity<Address>();
                SaveAddress(model.AddressModel, address);
                if (address.Id != 0)
                {
                    researcher.AddressId = address.Id;
                    _researcherService.UpdateResearcher(researcher);
                }

                SuccessNotification("Researcher Updated");

                //activity log
                //_userActivityService.InsertActivity("EditResearcher", "ActivityLog EditResearcher", researcher);

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
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
                return AccessDeniedView();

            //try to get a researcher with the specified id
            var researcher = _researcherService.GetResearcherById(id);
            if (researcher == null)
                return RedirectToAction("List");

            //prepare model
            var model = _researcherModelFactory.PrepareResearcherModel(null, researcher);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Delete(int Id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
                return AccessDeniedView();

            //try to get a researcher with the specified id
            if (Id == 0)
                return RedirectToAction("List");

            var researcher = _researcherService.GetResearcherById(Id);

            if (researcher == null)
                return RedirectToAction("List");
            researcher.Deleted = true;
            _researcherService.UpdateResearcher(researcher);

            SuccessNotification("Researchers Deleted");

            //activity log
            //_userActivityService.InsertActivity("DeleteResearcher", "ActivityLog.DeleteResearcher", researcher);

            return RedirectToAction("List");
        }

        #endregion

        #region Researcher educations

        [HttpPost]
        public virtual IActionResult ResearcherEducationsSelect(ResearcherEducationSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
                return AccessDeniedKendoGridJson();

            //try to get a researcher with the specified id
            var researcher = _researcherService.GetResearcherById(searchModel.ResearcherId);
            //?? throw new ArgumentException("No researcher found with the specified id");

            //prepare model
            if (researcher != null)
            {
                var model = _researcherModelFactory.PrepareResearcherEducationListModel(searchModel, researcher);

                return Json(model);
            }
            else
                return Json(new ResearcherEducationListModel());
        }

        public virtual IActionResult ResearcherEducationAdd(int researcherId, int degreeId,
            string educationLevelName, string institudeName, string countryName, int graduationYear)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
                return AccessDeniedView();

            //try to get a researcher with the specified id
            var researcher = _researcherService.GetResearcherById(researcherId);
            if (researcher == null)
                return Json(new { Result = false });

            var researcherEducation = new ResearcherEducation
            {
                ResearcherId = researcher.Id,
                DegreeId = degreeId,
                EducationLevelName = educationLevelName,
                InstituteName = institudeName,
                CountryName = countryName,
                GraduationYear = graduationYear
            };
            _researcherService.InsertResearcherEducation(researcherEducation);
            //  researcher.ResearcherEducations.Add(researcherEducation);
            //  _researcherService.UpdateResearcher(researcher);

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult ResearcherEducationDelete(int id, int researcherId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResearchers))
                return AccessDeniedView();

            //try to get a researcher with the specified id
            // var researcher = _researcherService.GetResearcherById(researcherId)
            //    ?? throw new ArgumentException("No researcher found with the specified id", nameof(researcherId));

            //try to get a researcher education with the specified id
            //var researcherEducation = researcher.ResearcherEducations.FirstOrDefault(vn => vn.Id == id)
            var researcherEducation = _researcherService.GetResearcherEducationById(id)
               ?? throw new ArgumentException("No researcher education found with the specified id", nameof(id));
            _researcherService.DeleteResearcherEducation(researcherEducation);
            //_researcherService.RemoveResearcherEducation(researcher,researcherEducation);

            return new NullJsonResult();
        }

        #endregion
    }
}