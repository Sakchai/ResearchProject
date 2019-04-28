using System.Collections.Generic;
using Research.Core;
using Research.Data;

namespace Research.Services.AcademicRanks
{
    /// <summary>
    /// AcademicRank service interface
    /// </summary>
    public partial interface IAcademicRankService
    {
        /// <summary>
        /// Delete academicRank
        /// </summary>
        /// <param name="academicRank">AcademicRank</param>
        void DeleteAcademicRank(AcademicRank academicRank);

        /// <summary>
        /// Gets all faculties
        /// </summary>
        /// <param name="academicRankName">AcademicRank name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>AcademicRanks</returns>
        IPagedList<AcademicRank> GetAllAcademicRanks(string academicRankName, 
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Gets a academicRank
        /// </summary>
        /// <param name="academicRankId">AcademicRank identifier</param>
        /// <returns>AcademicRank</returns>
        AcademicRank GetAcademicRankById(int academicRankId);

        /// <summary>
        /// Inserts academicRank
        /// </summary>
        /// <param name="academicRank">AcademicRank</param>
        void InsertAcademicRank(AcademicRank academicRank);

        /// <summary>
        /// Updates the academicRank
        /// </summary>
        /// <param name="academicRank">AcademicRank</param>
        void UpdateAcademicRank(AcademicRank academicRank);

        /// <summary>
        /// Returns a list of names of not existing faculties
        /// </summary>
        /// <param name="academicRankIdsNames">The names and/or IDs of the faculties to check</param>
        /// <returns>List of names and/or IDs not existing faculties</returns>
        string[] GetNotExistingAcademicRanks(string[] academicRankIdsNames);


        /// <summary>
        /// Gets faculties by identifier
        /// </summary>
        /// <param name="academicRankIds">AcademicRank identifiers</param>
        /// <returns>AcademicRanks</returns>
        List<AcademicRank> GetAcademicRanksByIds(int[] academicRankIds);
        List<AcademicRank> GetAllAcademicRanks();
    }
}
