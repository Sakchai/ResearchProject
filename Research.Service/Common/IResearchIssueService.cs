using System.Collections.Generic;
using Research.Core;
using Research.Data;

namespace Research.Services.Common
{
    /// <summary>
    /// ResearchIssue service interface
    /// </summary>
    public partial interface IResearchIssueService
    {
        /// <summary>
        /// Delete researchIssue
        /// </summary>
        /// <param name="researchIssue">ResearchIssue</param>
        void DeleteResearchIssue(ResearchIssue researchIssue);

        /// <summary>
        /// Gets all faculties
        /// </summary>
        /// <param name="researchIssueName">ResearchIssue name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>ResearchIssues</returns>
        IPagedList<ResearchIssue> GetAllResearchIssues(string researchIssueName, 
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Gets a researchIssue
        /// </summary>
        /// <param name="researchIssueId">ResearchIssue identifier</param>
        /// <returns>ResearchIssue</returns>
        ResearchIssue GetResearchIssueById(int researchIssueId);

        /// <summary>
        /// Inserts researchIssue
        /// </summary>
        /// <param name="researchIssue">ResearchIssue</param>
        void InsertResearchIssue(ResearchIssue researchIssue);

        /// <summary>
        /// Updates the researchIssue
        /// </summary>
        /// <param name="researchIssue">ResearchIssue</param>
        void UpdateResearchIssue(ResearchIssue researchIssue);

        /// <summary>
        /// Returns a list of names of not existing faculties
        /// </summary>
        /// <param name="researchIssueIdsNames">The names and/or IDs of the faculties to check</param>
        /// <returns>List of names and/or IDs not existing faculties</returns>
        string[] GetNotExistingResearchIssues(string[] researchIssueIdsNames);


        /// <summary>
        /// Gets faculties by identifier
        /// </summary>
        /// <param name="researchIssueIds">ResearchIssue identifiers</param>
        /// <returns>ResearchIssues</returns>
        List<ResearchIssue> GetResearchIssuesByIds(int[] researchIssueIds);
        List<ResearchIssue> GetAllResearchIssues();

        string GetNextNumber();
    }
}
