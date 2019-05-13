using System.Collections.Generic;
using Research.Core;
using Research.Data;

namespace Research.Services.Common
{
    /// <summary>
    /// Faculty service interface
    /// </summary>
    public partial interface IFacultyService
    {
        /// <summary>
        /// Delete faculty
        /// </summary>
        /// <param name="faculty">Faculty</param>
        void DeleteFaculty(Faculty faculty);

        /// <summary>
        /// Gets all faculties
        /// </summary>
        /// <param name="facultyName">Faculty name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Faculties</returns>
        IPagedList<Faculty> GetAllFaculties(string facultyName, 
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Gets a faculty
        /// </summary>
        /// <param name="facultyId">Faculty identifier</param>
        /// <returns>Faculty</returns>
        Faculty GetFacultyById(int facultyId);

        /// <summary>
        /// Inserts faculty
        /// </summary>
        /// <param name="faculty">Faculty</param>
        void InsertFaculty(Faculty faculty);

        /// <summary>
        /// Updates the faculty
        /// </summary>
        /// <param name="faculty">Faculty</param>
        void UpdateFaculty(Faculty faculty);

        /// <summary>
        /// Returns a list of names of not existing faculties
        /// </summary>
        /// <param name="facultyIdsNames">The names and/or IDs of the faculties to check</param>
        /// <returns>List of names and/or IDs not existing faculties</returns>
        string[] GetNotExistingFaculties(string[] facultyIdsNames);


        /// <summary>
        /// Gets faculties by identifier
        /// </summary>
        /// <param name="facultyIds">Faculty identifiers</param>
        /// <returns>Faculties</returns>
        List<Faculty> GetFacultiesByIds(int[] facultyIds);
        List<Faculty> GetAllFaculties();
    }
}
