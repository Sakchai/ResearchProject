using System;

namespace Research.Services.Titles
{
    /// <summary>
    /// Represents default values related to title services
    /// </summary>
    public static partial class ResearchTitleDefaults
    {
        #region Titles

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {1} : show hidden records?
        /// </remarks>
        public static string TitlesAllCacheKey => "Research.title.all-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string TitlesPatternCacheKey => "Research.title.";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : title ID
        /// </remarks>
        public static string TitlesByIdCacheKey => "Research.title.id-{0}";

        #endregion

    }
}