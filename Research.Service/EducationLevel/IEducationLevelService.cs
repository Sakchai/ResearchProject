using System.Collections.Generic;
using Research.Core;
using Research.Data;

namespace Research.Services.EducationLevels
{
    /// <summary>
    /// EducationLevel service interface
    /// </summary>
    public partial interface IEducationLevelService
    {
        /// <summary>
        /// Delete educationLevel
        /// </summary>
        /// <param name="educationLevel">EducationLevel</param>
        void DeleteEducationLevel(EducationLevel educationLevel);

        /// <summary>
        /// Gets all faculties
        /// </summary>
        /// <param name="educationLevelName">EducationLevel name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>EducationLevels</returns>
        IPagedList<EducationLevel> GetAllEducationLevels(string educationLevelName, 
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Gets a educationLevel
        /// </summary>
        /// <param name="educationLevelId">EducationLevel identifier</param>
        /// <returns>EducationLevel</returns>
        EducationLevel GetEducationLevelById(int educationLevelId);

        /// <summary>
        /// Inserts educationLevel
        /// </summary>
        /// <param name="educationLevel">EducationLevel</param>
        void InsertEducationLevel(EducationLevel educationLevel);

        /// <summary>
        /// Updates the educationLevel
        /// </summary>
        /// <param name="educationLevel">EducationLevel</param>
        void UpdateEducationLevel(EducationLevel educationLevel);

        /// <summary>
        /// Returns a list of names of not existing faculties
        /// </summary>
        /// <param name="educationLevelIdsNames">The names and/or IDs of the faculties to check</param>
        /// <returns>List of names and/or IDs not existing faculties</returns>
        string[] GetNotExistingEducationLevels(string[] educationLevelIdsNames);


        /// <summary>
        /// Gets faculties by identifier
        /// </summary>
        /// <param name="educationLevelIds">EducationLevel identifiers</param>
        /// <returns>EducationLevels</returns>
        List<EducationLevel> GetEducationLevelsByIds(int[] educationLevelIds);
        List<EducationLevel> GetAllEducationLevels();

    }
}
