using System;

namespace Research.Services.StrategyGroups
{
    /// <summary>
    /// Represents default values related to strategyGroup services
    /// </summary>
    public static partial class ResearchStrategyGroupDefaults
    {
        #region StrategyGroups

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {1} : show hidden records?
        /// </remarks>
        public static string StrategyGroupsAllCacheKey => "Research.strategyGroup.all-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string StrategyGroupsPatternCacheKey => "Research.strategyGroup.";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : strategyGroup ID
        /// </remarks>
        public static string StrategyGroupsByIdCacheKey => "Research.strategyGroup.id-{0}";

        #endregion

    }
}