using System.Collections.Generic;
using Research.Core;
using Research.Data;

namespace Research.Services.Common
{
    /// <summary>
    /// Agency service interface
    /// </summary>
    public partial interface IAgencyService
    {
        /// <summary>
        /// Delete agency
        /// </summary>
        /// <param name="agency">Agency</param>
        void DeleteAgency(Agency agency);

        /// <summary>
        /// Gets all faculties
        /// </summary>
        /// <param name="agencyName">Agency name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Agencys</returns>
        IPagedList<Agency> GetAllAgencys(string agencyName, 
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Gets a agency
        /// </summary>
        /// <param name="agencyId">Agency identifier</param>
        /// <returns>Agency</returns>
        Agency GetAgencyById(int agencyId);

        /// <summary>
        /// Inserts agency
        /// </summary>
        /// <param name="agency">Agency</param>
        void InsertAgency(Agency agency);

        /// <summary>
        /// Updates the agency
        /// </summary>
        /// <param name="agency">Agency</param>
        void UpdateAgency(Agency agency);

        /// <summary>
        /// Returns a list of names of not existing faculties
        /// </summary>
        /// <param name="agencyIdsNames">The names and/or IDs of the faculties to check</param>
        /// <returns>List of names and/or IDs not existing faculties</returns>
        string[] GetNotExistingAgencys(string[] agencyIdsNames);


        /// <summary>
        /// Gets faculties by identifier
        /// </summary>
        /// <param name="agencyIds">Agency identifiers</param>
        /// <returns>Agencys</returns>
        List<Agency> GetAgencysByIds(int[] agencyIds);
        List<Agency> GetAllAgencies();
    }
}
