using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Research.Core;
using Research.Data;
using Research.Services.Common;
using Research.Web.Extensions;
using Research.Web.Models.Factories;
using Research.Web.Models.ResearchIssues;

namespace Research.Web.Factories
{
    /// <summary>
    /// Represents the researchIssue model factory implementation
    /// </summary>
    public partial class ResearchIssueModelFactory : IResearchIssueModelFactory
    {
        #region Fields

        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IResearchIssueService _researchIssueService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public ResearchIssueModelFactory(IActionContextAccessor actionContextAccessor,
            IBaseAdminModelFactory baseAdminModelFactory,
            IResearchIssueService researchIssueService,
            IUrlHelperFactory urlHelperFactory,
            IWebHelper webHelper)
        {
            this._actionContextAccessor = actionContextAccessor;
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._researchIssueService = researchIssueService;
            this._urlHelperFactory = urlHelperFactory;
            this._webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare researchIssue search model
        /// </summary>
        /// <param name="searchModel">ResearchIssue search model</param>
        /// <returns>ResearchIssue search model</returns>
        public virtual ResearchIssueSearchModel PrepareResearchIssueSearchModel(ResearchIssueSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged researchIssue list model
        /// </summary>
        /// <param name="searchModel">ResearchIssue search model</param>
        /// <returns>ResearchIssue list model</returns>
        public virtual ResearchIssueListModel PrepareResearchIssueListModel(ResearchIssueSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get researchIssues
            var researchIssues = _researchIssueService.GetAllResearchIssues(researchIssueName: searchModel.Name);


            //prepare grid model
            var model = new ResearchIssueListModel
            {
                Data = researchIssues.PaginationByRequestModel(searchModel).Select(researchIssue =>
                {
                    //fill in model values from the entity
                    var researchIssueModel = researchIssue.ToModel<ResearchIssueModel>();

                    //little performance optimization: ensure that "Body" is not returned
                    researchIssueModel.Id = researchIssue.Id;
                    researchIssueModel.FiscalYear = researchIssue.FiscalYear;
                    researchIssueModel.IssueCode = researchIssue.IssueCode;
                    researchIssueModel.Name = researchIssue.Name;

                    return researchIssueModel;
                }),
                Total = researchIssues.Count
            };

            return model;
        }

        /// <summary>
        /// Prepare researchIssue model
        /// </summary>
        /// <param name="model">ResearchIssue model</param>
        /// <param name="researchIssue">ResearchIssue</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>ResearchIssue model</returns>
        public virtual ResearchIssueModel PrepareResearchIssueModel(ResearchIssueModel model, ResearchIssue researchIssue, bool excludeProperties = false)
        {
            if (researchIssue != null)
            {
                //fill in model values from the entity
                model = model ?? researchIssue.ToModel<ResearchIssueModel>();
                model.Id = researchIssue.Id;
                model.FiscalYear = researchIssue.FiscalYear;
                model.IssueCode = researchIssue.IssueCode;
                model.Name = researchIssue.Name;

            } else
            {
                model.IssueCode = _researchIssueService.GetNextNumber();
                model.FiscalYear = DateTime.Today.Year + 543;
            }

            _baseAdminModelFactory.PrepareFiscalYears(model.AvailableFiscalYears, true, "--ระบุปี--");

            return model;
        }


        #endregion
    }
}