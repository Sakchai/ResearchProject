using System.Collections.Generic;
using Research.Core;
using Research.Data;

namespace Research.Services.Common
{
    /// <summary>
    /// Institute service interface
    /// </summary>
    public partial interface IInstituteService
    {
        /// <summary>
        /// Delete institute
        /// </summary>
        /// <param name="institute">Institute</param>
        void DeleteInstitute(Institute institute);

        /// <summary>
        /// Gets all faculties
        /// </summary>
        /// <param name="instituteName">Institute name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Institutes</returns>
        IPagedList<Institute> GetAllInstitutes(string instituteName, 
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Gets a institute
        /// </summary>
        /// <param name="instituteId">Institute identifier</param>
        /// <returns>Institute</returns>
        Institute GetInstituteById(int instituteId);

        /// <summary>
        /// Inserts institute
        /// </summary>
        /// <param name="institute">Institute</param>
        void InsertInstitute(Institute institute);

        /// <summary>
        /// Updates the institute
        /// </summary>
        /// <param name="institute">Institute</param>
        void UpdateInstitute(Institute institute);

        /// <summary>
        /// Returns a list of names of not existing faculties
        /// </summary>
        /// <param name="instituteIdsNames">The names and/or IDs of the faculties to check</param>
        /// <returns>List of names and/or IDs not existing faculties</returns>
        string[] GetNotExistingInstitutes(string[] instituteIdsNames);


        /// <summary>
        /// Gets faculties by identifier
        /// </summary>
        /// <param name="instituteIds">Institute identifiers</param>
        /// <returns>Institutes</returns>
        List<Institute> GetInstitutesByIds(int[] instituteIds);
        List<Institute> GetAllInstitutes();

    }
}
