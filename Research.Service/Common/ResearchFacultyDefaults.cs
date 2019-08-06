using System;

namespace Research.Services.Common
{
    /// <summary>
    /// Represents default values related to faculty services
    /// </summary>
    public static partial class ResearchFacultyDefaults
    {
        #region Faculties

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {1} : show hidden records?
        /// </remarks>
        public static string FacultiesAllCacheKey => "Research.faculty.all-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string FacultiesPatternCacheKey => "Research.faculty.";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : faculty ID
        /// </remarks>
        public static string FacultiesByIdCacheKey => "Research.faculty.id-{0}";

        #endregion

    }
}