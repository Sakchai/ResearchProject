using System;

namespace Research.Services.Common
{
    /// <summary>
    /// Represents default values related to researchIssue services
    /// </summary>
    public static partial class ResearchResearchIssueDefaults
    {
        #region ResearchIssues

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {1} : show hidden records?
        /// </remarks>
        public static string ResearchIssuesAllCacheKey => "Research.researchIssue.all-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ResearchIssuesPatternCacheKey => "Research.researchIssue.";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : researchIssue ID
        /// </remarks>
        public static string ResearchIssuesByIdCacheKey => "Research.researchIssue.id-{0}";

        #endregion

    }
}