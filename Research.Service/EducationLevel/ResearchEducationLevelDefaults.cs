using System;

namespace Research.Services.EducationLevels
{
    /// <summary>
    /// Represents default values related to educationLevel services
    /// </summary>
    public static partial class ResearchEducationLevelDefaults
    {
        #region EducationLevels

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {1} : show hidden records?
        /// </remarks>
        public static string EducationLevelsAllCacheKey => "Research.educationLevel.all-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string EducationLevelsPatternCacheKey => "Research.educationLevel.";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : educationLevel ID
        /// </remarks>
        public static string EducationLevelsByIdCacheKey => "Research.educationLevel.id-{0}";

        #endregion

    }
}