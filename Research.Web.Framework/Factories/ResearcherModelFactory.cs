using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Research.Core;
using Research.Data;
using Research.Services;
using Research.Web.Factories;
using Research.Web.Framework.Extensions;
using Research.Web.Framework.Mapper.Extensions;
using Research.Web.Models.Factories;
using Research.Web.Models.Researchers;

namespace Research.Web.Factories
{
    /// <summary>
    /// Represents the researcher model factory implementation
    /// </summary>
    public partial class ResearcherModelFactory : IResearcherModelFactory
    {
        #region Fields

        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IResearcherService _researcherService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public ResearcherModelFactory(IActionContextAccessor actionContextAccessor,
            IBaseAdminModelFactory baseAdminModelFactory,
            IResearcherService researcherService,
            IUrlHelperFactory urlHelperFactory,
            IWebHelper webHelper)
        {
            this._actionContextAccessor = actionContextAccessor;
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._researcherService = researcherService;
            this._urlHelperFactory = urlHelperFactory;
            this._webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare researcher search model
        /// </summary>
        /// <param name="searchModel">Researcher search model</param>
        /// <returns>Researcher search model</returns>
        public virtual ResearcherSearchModel PrepareResearcherSearchModel(ResearcherSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare available stores
            _baseAdminModelFactory.PrepareAgencies(searchModel.AvailableAgencies);
            _baseAdminModelFactory.PreparePersonalTypes(searchModel.AvailablePersonTypes);

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged researcher list model
        /// </summary>
        /// <param name="searchModel">Researcher search model</param>
        /// <returns>Researcher list model</returns>
        public virtual ResearcherListModel PrepareResearcherListModel(ResearcherSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get researchers
            var researchers = _researcherService.GetAllResearchers();

            //filter researchers
            //TODO: move filter to researcher service
            //if (!string.IsNullOrEmpty(searchModel.FirstName))
            //{
            //    researchers = researchers.Where(researcher => researcher.FirstName.Contains(searchModel.FirstName)).ToList();
            //}

            //prepare grid model
            var model = new ResearcherListModel
            {
                Data = researchers.PaginationByRequestModel(searchModel).Select(researcher =>
                {
                    //fill in model values from the entity
                    var researcherModel = researcher.ToModel<ResearcherModel>();

                    //little performance optimization: ensure that "Body" is not returned
                    researcherModel.TitleId = researcher.TitleId;
                    researcherModel.FirstName = researcher.FirstName;
                    researcherModel.LastName = researcher.LastName;
                    researcherModel.PersonalTypeName = researcher.PersonalType.ToString();
                    researcherModel.AgencyName = researcher.Agency != null ? researcher.Agency.Name : string.Empty;

                    return researcherModel;
                }),
                Total = researchers.Count
            };

            return model;
        }

        /// <summary>
        /// Prepare researcher model
        /// </summary>
        /// <param name="model">Researcher model</param>
        /// <param name="researcher">Researcher</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Researcher model</returns>
        public virtual ResearcherModel PrepareResearcherModel(ResearcherModel model, Researcher researcher, bool excludeProperties = false)
        {
            if (researcher != null)
            {
                //fill in model values from the entity
                model = model ?? researcher.ToModel<ResearcherModel>();
                model.TitleId = researcher.TitleId;
                model.FirstName = researcher.FirstName;
                model.LastName = researcher.LastName;
                model.FirstNameEN = researcher.FirstNameEN;
                model.LastNameEN = researcher.LastNameEN;
                model.DateOfBirthDay = researcher.Birthdate?.Day;
                model.DateOfBirthMonth = researcher.Birthdate?.Month;
                model.DateOfBirthYear = researcher.Birthdate?.Year;
                model.IDCard = researcher.IDCard;
                model.Telephone = researcher.Telephone;
                model.Email = researcher.Email;
                model.PictureId = researcher.PictureId;
                model.PersonalTypeId = researcher.PersonalTypeId;
                model.AgencyId = researcher.AgencyId;
                model.AcademicRankId = researcher.AcademicRankId;

                _baseAdminModelFactory.PrepareTitles(model.AvailableTitles);
                _baseAdminModelFactory.PrepareAgencies(model.AvailableAgencies);
                _baseAdminModelFactory.PrepareAcademicRanks(model.AvailableAcademicRanks);
                _baseAdminModelFactory.PreparePersonalTypes(model.AvailablePersonalTypes);
            }




            return model;
        }

        #endregion
    }
}