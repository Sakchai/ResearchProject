using System.Collections.Generic;
using Research.Core;
using Research.Data;

namespace Research.Services.Common
{
    /// <summary>
    /// Title service interface
    /// </summary>
    public partial interface ITitleService
    {
        /// <summary>
        /// Delete title
        /// </summary>
        /// <param name="title">Title</param>
        void DeleteTitle(Title title);

        /// <summary>
        /// Gets all faculties
        /// </summary>
        /// <param name="titleName">Title name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Titles</returns>
        IPagedList<Title> GetAllTitles(string titleName, 
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Gets a title
        /// </summary>
        /// <param name="titleId">Title identifier</param>
        /// <returns>Title</returns>
        Title GetTitleById(int titleId);

        /// <summary>
        /// Inserts title
        /// </summary>
        /// <param name="title">Title</param>
        void InsertTitle(Title title);

        /// <summary>
        /// Updates the title
        /// </summary>
        /// <param name="title">Title</param>
        void UpdateTitle(Title title);

        /// <summary>
        /// Returns a list of names of not existing faculties
        /// </summary>
        /// <param name="titleIdsNames">The names and/or IDs of the faculties to check</param>
        /// <returns>List of names and/or IDs not existing faculties</returns>
        string[] GetNotExistingTitles(string[] titleIdsNames);


        /// <summary>
        /// Gets faculties by identifier
        /// </summary>
        /// <param name="titleIds">Title identifiers</param>
        /// <returns>Titles</returns>
        List<Title> GetTitlesByIds(int[] titleIds);
        List<Title> GetAllTitles();
        string GetGender(int titleId);
    }
}
