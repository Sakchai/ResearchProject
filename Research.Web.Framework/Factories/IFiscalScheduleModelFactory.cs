
using Research.Data;
using Research.Web.Models.FiscalSchedules;

namespace Research.Web.Factories
{
    /// <summary>
    /// Represents the fiscalSchedule model factory
    /// </summary>
    public partial interface IFiscalScheduleModelFactory
    {
        /// <summary>
        /// Prepare fiscalSchedule search model
        /// </summary>
        /// <param name="model">FiscalSchedule search model</param>
        /// <returns>FiscalSchedule search model</returns>
        FiscalScheduleSearchModel PrepareFiscalScheduleSearchModel(FiscalScheduleSearchModel searchModel);

        /// <summary>
        /// Prepare paged fiscalSchedule list model
        /// </summary>
        /// <param name="searchModel">FiscalSchedule search model</param>
        /// <returns>FiscalSchedule list model</returns>
        FiscalScheduleListModel PrepareFiscalScheduleListModel(FiscalScheduleSearchModel searchModel);

        /// <summary>
        /// Prepare fiscalSchedule model
        /// </summary>
        /// <param name="model">FiscalSchedule model</param>
        /// <param name="fiscalSchedule">FiscalSchedule</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>FiscalSchedule model</returns>
        FiscalScheduleModel PrepareFiscalScheduleModel(FiscalScheduleModel model, FiscalSchedule fiscalSchedule, bool excludeProperties = false);
    }
}