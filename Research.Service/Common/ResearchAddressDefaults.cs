using System;

namespace Research.Services.Common
{
    /// <summary>
    /// Represents default values related to address services
    /// </summary>
    public static partial class ResearchAddressDefaults
    {
        #region Addresses

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {1} : show hidden records?
        /// </remarks>
        public static string AddressesAllCacheKey => "Research.address.all-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AddressesPatternCacheKey => "Research.address.";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : address ID
        /// </remarks>
        public static string AddressesByIdCacheKey => "Research.address.id-{0}";

        #endregion

    }
}