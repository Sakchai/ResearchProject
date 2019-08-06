namespace Research.Services.Users
{
    /// <summary>
    /// Represents default values related to user services
    /// </summary>
    public static partial class ResearchUserServiceDefaults
    {
        #region User attributes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string UserAttributesAllCacheKey => "Research.userattribute.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user attribute ID
        /// </remarks>
        public static string UserAttributesByIdCacheKey => "Research.userattribute.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user attribute ID
        /// </remarks>
        public static string UserAttributeValuesAllCacheKey => "Research.userattributevalue.all-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user attribute value ID
        /// </remarks>
        public static string UserAttributeValuesByIdCacheKey => "Research.userattributevalue.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string UserAttributesPatternCacheKey => "Research.userattribute.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string UserAttributeValuesPatternCacheKey => "Research.userattributevalue.";

        #endregion

        #region User roles

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static string UserRolesAllCacheKey => "Research.userrole.all-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        public static string UserRolesBySystemNameCacheKey => "Research.userrole.systemname-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string UserRolesPatternCacheKey => "Research.userrole.";

        #endregion

        /// <summary>
        /// Gets a key for caching current user password lifetime
        /// </summary>
        /// <remarks>
        /// {0} : user identifier
        /// </remarks>
        public static string UserPasswordLifetimeCacheKey => "Research.users.passwordlifetime-{0}";

        /// <summary>
        /// Gets a password salt key size
        /// </summary>
        public static int PasswordSaltKeySize => 5;
        
        /// <summary>
        /// Gets a max username length
        /// </summary>
        public static int UserUsernameLength => 100;
    }
}