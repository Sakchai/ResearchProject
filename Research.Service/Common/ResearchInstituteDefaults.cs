using System;

namespace Research.Services.Common
{
    /// <summary>
    /// Represents default values related to institute services
    /// </summary>
    public static partial class ResearchInstituteDefaults
    {
        #region Institutes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {1} : show hidden records?
        /// </remarks>
        public static string InstitutesAllCacheKey => "Research.institute.all-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string InstitutesPatternCacheKey => "Research.institute.";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : institute ID
        /// </remarks>
        public static string InstitutesByIdCacheKey => "Research.institute.id-{0}";

        #endregion

    }
}