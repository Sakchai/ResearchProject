using Research.Data;
using System.Collections.Generic;

namespace Research.Services.Directory
{
    /// <summary>
    /// State province service interface
    /// </summary>
    public partial interface IProvinceService
    {
        /// <summary>
        /// Deletes a state/province
        /// </summary>
        /// <param name="province">The state/province</param>
        void DeleteProvince(Province province);

        /// <summary>
        /// Gets a state/province
        /// </summary>
        /// <param name="provinceId">The state/province identifier</param>
        /// <returns>State/province</returns>
        Province GetProvinceById(int provinceId);

        /// <summary>
        /// Gets a state/province by abbreviation
        /// </summary>
        /// <param name="abbreviation">The state/province abbreviation</param>
        /// <param name="countryId">Country identifier; pass null to load the state regardless of a country</param>
        /// <returns>State/province</returns>
        Province GetProvinceByAbbreviation(string abbreviation, int? countryId = null);
        
        /// <summary>
        /// Gets a state/province collection by country identifier
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <param name="languageId">Language identifier. It's used to sort states by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>States</returns>
        IList<Province> GetProvincesByCountryId(int countryId, int languageId = 0, bool showHidden = false);

        /// <summary>
        /// Gets all states/provinces
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>States</returns>
        IList<Province> GetProvinces(bool showHidden = false);

        /// <summary>
        /// Inserts a state/province
        /// </summary>
        /// <param name="province">State/province</param>
        void InsertProvince(Province province);

        /// <summary>
        /// Updates a state/province
        /// </summary>
        /// <param name="province">State/province</param>
        void UpdateProvince(Province province);
    }
}
