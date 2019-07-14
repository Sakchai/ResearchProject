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
using Research.Web.Models.Professors;
using Research.Services.Common;
using Research.Web.Framework.Mvc;
using Research.Web.Models.Common;
using Research.Services.Professors;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Research.Web.Controllers
{ 
    public partial class ProfessorController : BaseAdminController
    {
        #region Fields

        private readonly IUserActivityService _userActivityService;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IProfessorModelFactory _professorModelFactory;
        private readonly IProfessorService _professorService;
        private readonly IPictureService _pictureService;
        private readonly IAddressService _addressService;

        #endregion Fields

        #region Ctor

        public ProfessorController(IUserActivityService userActivityService,
            IUserService userService,
            IPermissionService permissionService,
            IProfessorModelFactory professorModelFactory,
            IProfessorService professorService,
            IPictureService pictureService,
            IAddressService addressService)
        {
            this._userActivityService = userActivityService;
            this._userService = userService;
            this._permissionService = permissionService;
            this._professorModelFactory = professorModelFactory;
            this._professorService = professorService;
            this._pictureService = pictureService;
            this._addressService = addressService;
        }

        #endregion

        #region List

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProfessors))
            //    return AccessDeniedView();

            //prepare model
            var model = _professorModelFactory.PrepareProfessorSearchModel(new ProfessorSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ProfessorsSelect()
        {
            //var model = new List<SelectListItem>();
            var model = new List<string>();
            var professors = _professorService.GetAllProfessors().ToList();
            foreach (var x in professors)
            {
                string title = (x.Title != null) ? x.Title.TitleNameTH : string.Empty;
                //model.Add(new SelectListItem(item.Id.ToString(), $"{item.FirstName} {item.LastName}"));
                model.Add($"{title}{x.FirstName} {x.LastName}");
            }
            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult List(ProfessorSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProfessors))
            //    return AccessDeniedKendoGridJson();

            //prepare model
            var model = _professorModelFactory.PrepareProfessorListModel(searchModel);

            return Json(model);
        }

        #endregion

        #region Create / Edit / Delete
        [HttpGet, ActionName("Create")]
        public virtual IActionResult Create()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProfessors))
            //    return AccessDeniedView();

            //prepare model
            var model = _professorModelFactory.PrepareProfessorModel(new ProfessorModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ProfessorModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProfessors))
            //    return AccessDeniedView();

            if (ModelState.IsValid)
            {

                var professor = model.ToEntity<Professor>();
                _professorService.InsertProfessor(professor);

                SuccessNotification("Admin.ContentManagement.Professors.Added");

                //activity log
                //_userActivityService.InsertActivity("AddNewProfessor", "ActivityLog.AddNewProfessor", professor);

                var address = model.AddressModel.ToEntity<Address>();
                SaveAddress(model.AddressModel, address);
                if (address.Id != 0)
                {
                    professor.AddressId = address.Id;
                    _professorService.UpdateProfessor(professor);
                }
                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = professor.Id });
            }

            //prepare model
            model = _professorModelFactory.PrepareProfessorModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        private void SaveAddress(AddressModel model, Address address)
        {
            if ((model != null) && (address.Id == 0))
            {
                if (!string.IsNullOrEmpty(address.Address1))
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
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProfessors))
            //    return AccessDeniedView();

            //try to get a professor with the specified id
            var professor = _professorService.GetProfessorById(id);
            if (professor == null)
                return RedirectToAction("List");

            //prepare model
            var model = _professorModelFactory.PrepareProfessorModel(null, professor);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(ProfessorModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProfessors))
            //    return AccessDeniedView();

            //try to get a professor with the specified id
            var professor = _professorService.GetProfessorById(model.Id);
            if (professor == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                professor = model.ToEntity(professor);
                _professorService.UpdateProfessor(professor);
                var address = model.AddressModel.ToEntity<Address>();
                SaveAddress(model.AddressModel, address);

                SuccessNotification("Professor Updated");

                //activity log
                //_userActivityService.InsertActivity("EditProfessor","ActivityLog EditProfessor", professor);

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = professor.Id });
            }

            //prepare model
            model = _professorModelFactory.PrepareProfessorModel(model, professor, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Info(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProfessors))
            //    return AccessDeniedView();

            //try to get a professor with the specified id
            var professor = _professorService.GetProfessorById(id);
            if (professor == null)
                return RedirectToAction("List");

            //prepare model
            var model = _professorModelFactory.PrepareProfessorModel(null, professor);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProfessors))
            //    return AccessDeniedView();

            //try to get a professor with the specified id
            var professor = _professorService.GetProfessorById(id);
            if (professor == null)
                return RedirectToAction("List");
            professor.Deleted = true;
            _professorService.UpdateProfessor(professor);

            SuccessNotification("Professors Deleted");

            //activity log
            //_userActivityService.InsertActivity("DeleteProfessor","ActivityLog.DeleteProfessor", professor);

            return RedirectToAction("List");
        }

        #endregion

    }
}