using System;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Research.Core;
using Research.Data;
using Research.Enum;
using Research.Services.Professors;
using Research.Web.Extensions;
using Research.Web.Models.Common;
using Research.Web.Models.Factories;
using Research.Web.Models.Professors;

namespace Research.Web.Factories
{
    /// <summary>
    /// Represents the professor model factory implementation
    /// </summary>
    public partial class ProfessorModelFactory : IProfessorModelFactory
    {
        #region Fields

        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IProfessorService _professorService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public ProfessorModelFactory(IActionContextAccessor actionContextAccessor,
            IBaseAdminModelFactory baseAdminModelFactory,
            IProfessorService professorService,
            IUrlHelperFactory urlHelperFactory,
            IWebHelper webHelper)
        {
            this._actionContextAccessor = actionContextAccessor;
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._professorService = professorService;
            this._urlHelperFactory = urlHelperFactory;
            this._webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare professor search model
        /// </summary>
        /// <param name="searchModel">Professor search model</param>
        /// <returns>Professor search model</returns>
        public virtual ProfessorSearchModel PrepareProfessorSearchModel(ProfessorSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare available stores
            _baseAdminModelFactory.PrepareTitles(searchModel.AvailableTitles, true, "--รหัสคำนำหน้าชื่อ--");
            _baseAdminModelFactory.PrepareProfessorTypes(searchModel.AvailableProfessorTypes, true, "--รหัสประเภทผู้ทรงวุฒิ--");

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged professor list model
        /// </summary>
        /// <param name="searchModel">Professor search model</param>
        /// <returns>Professor list model</returns>
        public virtual ProfessorListModel PrepareProfessorListModel(ProfessorSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            string professorType = string.Empty;
            if (searchModel.ProfessorTypeId == 1)
                professorType = ProfessorType.InternalExpert.ToString();
            else if (searchModel.ProfessorTypeId == 2)
                professorType = ProfessorType.ExternalExpert.ToString();
            //get professors
            var professors = _professorService.GetAllProfessors(titleId: searchModel.TitleId,
                                                         firstName: searchModel.FirstName,
                                                         lastName: searchModel.LastName,
                                                         professorType: professorType);


            //prepare grid model
            var model = new ProfessorListModel
            {
                Data = professors.PaginationByRequestModel(searchModel).Select(professor =>
                {
                    //fill in model values from the entity
                    var professorModel = professor.ToModel<ProfessorModel>();

                    //little performance optimization: ensure that "Body" is not returned
                    professorModel.TitleName = professor.Title != null ? professor.Title.TitleNameTH : string.Empty;
                    professorModel.FirstName = professor.FirstName;
                    professorModel.LastName = professor.LastName;
                    professorModel.Telephone = professor.Telephone;
                    professorModel.Email = professor.Email;
                    professorModel.ProfessorTypeName = professor.ProfessorType.Equals("InternalExpert") ? "ผู้ทรงคุณวุฒิภายใน" : "ผู้ทรงคุณวุฒิภายนอก";

                    return professorModel;
                }),
                Total = professors.Count
            };

            return model;
        }

        /// <summary>
        /// Prepare professor model
        /// </summary>
        /// <param name="model">Professor model</param>
        /// <param name="professor">Professor</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Professor model</returns>
        public virtual ProfessorModel PrepareProfessorModel(ProfessorModel model, Professor professor, bool excludeProperties = false)
        {
            if (professor != null)
            {
                //fill in model values from the entity
                model = model ?? professor.ToModel<ProfessorModel>();
                model.Id = professor.Id;
                model.ProfessorCode = professor.ProfessorCode;
                model.TitleId = professor.TitleId;
                model.FirstName = professor.FirstName;
                model.LastName = professor.LastName;
                model.Telephone = professor.Telephone;
                model.Email = professor.Email;
                model.ProfessorType = professor.ProfessorType;
                model.IsActive = professor.IsActive;
                PrepareAddressModel(model.AddressModel, professor);

            }
            else
            {
                model.ProfessorCode = _professorService.GetNextNumber();
                model.ProfessorType = ProfessorType.InternalExpert.ToString();
                model.IsActive = true;
            }
            PrepareAddressModel(model.AddressModel, professor);
            _baseAdminModelFactory.PrepareTitles(model.AvailableTitles, true, "--ระบุคำนำหน้าชื่อ--");
            //Default Thailand
            return model;
        }


        /// <summary>
        /// Prepare address model
        /// </summary>
        /// <param name="model">Address model</param>
        /// <param name="address">Address</param>
        protected virtual void PrepareAddressModel(AddressModel model, Professor professor)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if ((professor != null) && (professor.Address != null))
            {
                model.Id = professor.Address.Id;
                model.Address1 = professor.Address.Address1;
                model.Address2 = professor.Address.Address2;
                model.ProvinceId = professor.Address.ProvinceId;
                model.ZipCode = professor.Address.ZipCode;
            }

            //prepare available Provinces
            _baseAdminModelFactory.PrepareProvinces(model.AvailableProvinces, true, "--ระบุจังหวัด--");


        }

        #endregion
    }
}