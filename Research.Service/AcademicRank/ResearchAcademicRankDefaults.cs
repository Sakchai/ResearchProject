using System;

namespace Research.Services.AcademicRanks
{
    /// <summary>
    /// Represents default values related to academicRank services
    /// </summary>
    public static partial class ResearchAcademicRankDefaults
    {
        #region AcademicRanks

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {1} : show hidden records?
        /// </remarks>
        public static string AcademicRanksAllCacheKey => "Research.academicRank.all-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AcademicRanksPatternCacheKey => "Research.academicRank.";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : academicRank ID
        /// </remarks>
        public static string AcademicRanksByIdCacheKey => "Research.academicRank.id-{0}";

        #endregion

    }
}