namespace Research.Services.Logging
{
    /// <summary>
    /// Represents default values related to logging services
    /// </summary>
    public static partial class ResearchLoggingDefaults
    {
        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string ActivityTypeAllCacheKey => "Research.activitytype.all";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ActivityTypePatternCacheKey => "Research.activitytype.";
    }
}