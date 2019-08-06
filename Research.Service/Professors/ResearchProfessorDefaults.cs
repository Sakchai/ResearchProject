using System;

namespace Research.Services.Professors
{
    /// <summary>
    /// Represents default values related to professor services
    /// </summary>
    public static partial class ResearchProfessorDefaults
    {
        #region Professors

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {1} : show hidden records?
        /// </remarks>
        public static string ProfessorsAllCacheKey => "Research.professor.all-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProfessorsPatternCacheKey => "Research.professor.";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : professor ID
        /// </remarks>
        public static string ProfessorsByIdCacheKey => "Research.professor.id-{0}";

        #endregion

    }
}