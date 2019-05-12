﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Research.Core;
using Research.Data;
using Research.Services;
using Research.Web.Extensions;
using Research.Web.Models.Common;
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
            _baseAdminModelFactory.PrepareAgencies(searchModel.AvailableAgencies,true,"--รหัสหน่วยงาน--");
            _baseAdminModelFactory.PreparePersonalTypes(searchModel.AvailablePersonTypes,true,"--รหัสประเภทบุคลากร--");

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
            var researchers = _researcherService.GetAllResearchers(agency:searchModel.AgencyId,
                                                             personalType:searchModel.PersonTypeId,
                                                             firstName:searchModel.FirstName,
                                                             lastName:searchModel.LastName,
                                                             isActive:searchModel.IsCompleted);


            //prepare grid model
            var model = new ResearcherListModel
            {
                Data = researchers.PaginationByRequestModel(searchModel).Select(researcher =>
                {
                    //fill in model values from the entity
                    var researcherModel = researcher.ToModel<ResearcherModel>();

                    //little performance optimization: ensure that "Body" is not returned
                    researcherModel.ResearcherCode   = researcher.ResearcherCode;
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
                model.ResearcherCode = researcher.ResearcherCode;
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
                if (researcher.Birthdate.HasValue)
                {
                    model.DateOfBirthDay = researcher.Birthdate.Value.Day;
                    model.DateOfBirthMonth = researcher.Birthdate.Value.Month;
                    model.DateOfBirthYear = researcher.Birthdate.Value.Year + 543;
                }
                if (researcher.Address != null)
                {
                    var addr = new AddressModel {
                        Id = researcher.Address.Id,
                        Address1 = researcher.Address.Address1,
                        Address2 = researcher.Address.Address2,
                        ProvinceId = researcher.Address.ProvinceId,
                        ZipCode = researcher.Address.ZipCode
                    };
                    model.Address = addr;
                }
                else
                {
                    model.Address = new AddressModel();
                }
            }

            _baseAdminModelFactory.PrepareProvinces(model.Address.AvailableProvinces,true,"--โปรดระบุจังหวัด--");
            _baseAdminModelFactory.PrepareTitles(model.AvailableTitles,true,"--โปรดระบุคำนำหน้าชื่อ--");
            _baseAdminModelFactory.PrepareAgencies(model.AvailableAgencies,true, "--โปรดระบุประเภทหน่วยงาน--");
            _baseAdminModelFactory.PrepareAcademicRanks(model.AvailableAcademicRanks, true, "--โปรดระบุตำแหน่งวิชาการ--");
            _baseAdminModelFactory.PreparePersonalTypes(model.AvailablePersonalTypes, true, "--โปรดระบุประเภทบุคลากร--");

            _baseAdminModelFactory.PrepareDegrees(model.AvailableAddEducationDegrees, true, "--โปรดระบุระดับปริญญา--");
            _baseAdminModelFactory.PrepareEducationLevels(model.AvailableAddEducationEducationLevels, true, "--โปรดระบุวุฒิการศึกษา--");
            _baseAdminModelFactory.PrepareInstitutes(model.AvailableAddEducationInstitutes, true, "--โปรดระบุสถาบันการศึกษา--");
            _baseAdminModelFactory.PrepareCountries(model.AvailableAddEducationCountries, true, "--โปรดระบุประเทศ--");
            return model;
        }

        /// <summary>
        /// Prepare paged researcher education list model
        /// </summary>
        /// <param name="searchModel">Researcher education search model</param>
        /// <param name="researcher">Researcher</param>
        /// <returns>Researcher education list model</returns>
        public ResearcherEducationListModel PrepareResearcherEducationListModel(ResearcherEducationSearchModel searchModel, Researcher researcher)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (researcher == null)
                throw new ArgumentNullException(nameof(researcher));

            //get researcher educations
            var researcherEducations = researcher.ResearcherEducations.OrderByDescending(edu => edu.Degree).ToList();

            //prepare list model
            var model = new ResearcherEducationListModel
            {
                Data = researcherEducations.PaginationByRequestModel(searchModel).Select(education =>
                {
                    //fill in model values from the entity        
                    var researcherEducationModel = new ResearcherEducationModel
                    {
                        Id = education.Id,
                        ResearcherId = education.ResearcherId,
                        Degress = education.Degree.ToString(),
                        EducationLevelName = education.EducationLevel.Name,
                        InstituteName = education.Institute.Name,
                        CountryName = education.Country.Name,
                        GraduationYear = education.GraduationYear
                    };



                    return researcherEducationModel;
                }),
                Total = researcherEducations.Count
            };

            return model;
        }

        #endregion
    }
}