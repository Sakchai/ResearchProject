using System;

namespace Research.Services.Agencies
{
    /// <summary>
    /// Represents default values related to agency services
    /// </summary>
    public static partial class ResearchAgencyDefaults
    {
        #region Agencies

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {1} : show hidden records?
        /// </remarks>
        public static string AgenciesAllCacheKey => "Research.agency.all-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AgenciesPatternCacheKey => "Research.agency.";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : agency ID
        /// </remarks>
        public static string AgenciesByIdCacheKey => "Research.agency.id-{0}";

        #endregion

    }
}