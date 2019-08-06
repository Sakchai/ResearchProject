using System;

namespace Research.Services.FiscalSchedules
{
    /// <summary>
    /// Represents default values related to fiscalSchedule services
    /// </summary>
    public static partial class ResearchFiscalScheduleDefaults
    {
        #region FiscalSchedules

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {1} : show hidden records?
        /// </remarks>
        public static string FiscalSchedulesAllCacheKey => "Research.fiscalSchedule.all-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string FiscalSchedulesPatternCacheKey => "Research.fiscalSchedule.";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : fiscalSchedule ID
        /// </remarks>
        public static string FiscalSchedulesByIdCacheKey => "Research.fiscalSchedule.id-{0}";

        #endregion

    }
}