
namespace Research.Services.Common
{
    /// <summary>
    /// Represents default values related to common services
    /// </summary>
    public static partial class ResearchCommonDefaults
    {
        #region Address attributes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string AddressAttributesAllCacheKey => "Research.addressattribute.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : address attribute ID
        /// </remarks>
        public static string AddressAttributesByIdCacheKey => "Research.addressattribute.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : address attribute ID
        /// </remarks>
        public static string AddressAttributeValuesAllCacheKey => "Research.addressattributevalue.all-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : address attribute value ID
        /// </remarks>
        public static string AddressAttributeValuesByIdCacheKey => "Research.addressattributevalue.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AddressAttributesPatternCacheKey => "Research.addressattribute.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AddressAttributeValuesPatternCacheKey => "Research.addressattributevalue.";

        #endregion

        #region Addresses

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : address ID
        /// </remarks>
        public static string AddressesByIdCacheKey => "Research.address.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AddressesPatternCacheKey => "Research.address.";

        #endregion

        #region Generic attributes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : key group
        /// </remarks>
        public static string GenericAttributeCacheKey => "Research.genericattribute.{0}-{1}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string GenericAttributePatternCacheKey => "Research.genericattribute.";

        #endregion

        #region Maintenance

        /// <summary>
        /// Gets a path to the database backup files
        /// </summary>
        public static string DbBackupsPath => "db_backups\\";

        /// <summary>
        /// Gets a database backup file extension
        /// </summary>
        public static string DbBackupFileExtension => "bak";

        #endregion
    }
}