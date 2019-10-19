using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Research.Core;
using Research.Data;
using Research.Services.Common;
using Research.Services.FiscalSchedules;
using Research.Web.Extensions;
using Research.Web.Models.Factories;
using Research.Web.Models.FiscalSchedules;

namespace Research.Web.Factories
{
    /// <summary>
    /// Represents the fiscalSchedule model factory implementation
    /// </summary>
    public partial class FiscalScheduleModelFactory : IFiscalScheduleModelFactory
    {
        #region Fields

        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IFiscalScheduleService _fiscalScheduleService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public FiscalScheduleModelFactory(IActionContextAccessor actionContextAccessor,
            IBaseAdminModelFactory baseAdminModelFactory,
            IFiscalScheduleService fiscalScheduleService,
            IUrlHelperFactory urlHelperFactory,
            IWebHelper webHelper)
        {
            this._actionContextAccessor = actionContextAccessor;
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._fiscalScheduleService = fiscalScheduleService;
            this._urlHelperFactory = urlHelperFactory;
            this._webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare fiscalSchedule search model
        /// </summary>
        /// <param name="searchModel">FiscalSchedule search model</param>
        /// <returns>FiscalSchedule search model</returns>
        public virtual FiscalScheduleSearchModel PrepareFiscalScheduleSearchModel(FiscalScheduleSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged fiscalSchedule list model
        /// </summary>
        /// <param name="searchModel">FiscalSchedule search model</param>
        /// <returns>FiscalSchedule list model</returns>
        public virtual FiscalScheduleListModel PrepareFiscalScheduleListModel(FiscalScheduleSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get fiscalSchedules
            var fiscalSchedules = _fiscalScheduleService.GetAllFiscalSchedules(fiscalScheduleName: searchModel.ScholarName);


            //prepare grid model
            var model = new FiscalScheduleListModel
            {
                Data = fiscalSchedules.PaginationByRequestModel(searchModel).Select(fiscalSchedule =>
                {
                    //fill in model values from the entity
                    var fiscalScheduleModel = fiscalSchedule.ToModel<FiscalScheduleModel>();

                    //little performance optimization: ensure that "Body" is not returned
                    fiscalScheduleModel.Id = fiscalSchedule.Id;
                    fiscalScheduleModel.FiscalYear = fiscalSchedule.FiscalYear;
                    fiscalScheduleModel.OpeningDateName = CommonHelper.ConvertToThaiDate(fiscalSchedule.OpeningDate);
                    fiscalScheduleModel.ClosingDateName = CommonHelper.ConvertToThaiDate(fiscalSchedule.ClosingDate);
                    fiscalScheduleModel.FiscalCode = fiscalSchedule.FiscalCode;
                    fiscalScheduleModel.FiscalTimes = fiscalSchedule.FiscalTimes;
                    fiscalScheduleModel.ScholarName = fiscalSchedule.ScholarName;
                    return fiscalScheduleModel;
                }),
                Total = fiscalSchedules.Count
            };

            return model;
        }

        /// <summary>
        /// Prepare fiscalSchedule model
        /// </summary>
        /// <param name="model">FiscalSchedule model</param>
        /// <param name="fiscalSchedule">FiscalSchedule</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>FiscalSchedule model</returns>
        public virtual FiscalScheduleModel PrepareFiscalScheduleModel(FiscalScheduleModel model, FiscalSchedule fiscalSchedule, bool excludeProperties = false)
        {
            if (fiscalSchedule != null)
            {
                //fill in model values from the entity
                model = model ?? fiscalSchedule.ToModel<FiscalScheduleModel>();
                model.Id = fiscalSchedule.Id;
                model.FiscalYear = fiscalSchedule.FiscalYear;
                model.ClosingDate = fiscalSchedule.ClosingDate.AddYears(543);
                model.FiscalCode = fiscalSchedule.FiscalCode;
                model.FiscalTimes = fiscalSchedule.FiscalTimes;
                model.OpeningDate = fiscalSchedule.OpeningDate.AddYears(543);
            } else
            {
                model.FiscalCode = _fiscalScheduleService.GetNextNumber();
                model.FiscalYear = DateTime.Today.Year + 543;
                model.OpeningDate = DateTime.Today.AddYears(543);
                model.ClosingDate = DateTime.Today.AddYears(544);
                model.FiscalTimes = 1;
            }

            _baseAdminModelFactory.PrepareFiscalYears(model.AvailableFiscalYears, true, "--ระบุปี--");

            return model;
        }


        #endregion
    }
}