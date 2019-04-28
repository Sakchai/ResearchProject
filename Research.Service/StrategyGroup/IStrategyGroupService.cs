using System.Collections.Generic;
using Research.Core;
using Research.Data;

namespace Research.Services.StrategyGroups
{
    /// <summary>
    /// StrategyGroup service interface
    /// </summary>
    public partial interface IStrategyGroupService
    {
        /// <summary>
        /// Delete strategyGroup
        /// </summary>
        /// <param name="strategyGroup">StrategyGroup</param>
        void DeleteStrategyGroup(StrategyGroup strategyGroup);

        /// <summary>
        /// Gets all faculties
        /// </summary>
        /// <param name="strategyGroupName">StrategyGroup name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>StrategyGroups</returns>
        IPagedList<StrategyGroup> GetAllStrategyGroups(string strategyGroupName, 
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Gets a strategyGroup
        /// </summary>
        /// <param name="strategyGroupId">StrategyGroup identifier</param>
        /// <returns>StrategyGroup</returns>
        StrategyGroup GetStrategyGroupById(int strategyGroupId);

        /// <summary>
        /// Inserts strategyGroup
        /// </summary>
        /// <param name="strategyGroup">StrategyGroup</param>
        void InsertStrategyGroup(StrategyGroup strategyGroup);

        /// <summary>
        /// Updates the strategyGroup
        /// </summary>
        /// <param name="strategyGroup">StrategyGroup</param>
        void UpdateStrategyGroup(StrategyGroup strategyGroup);

        /// <summary>
        /// Returns a list of names of not existing faculties
        /// </summary>
        /// <param name="strategyGroupIdsNames">The names and/or IDs of the faculties to check</param>
        /// <returns>List of names and/or IDs not existing faculties</returns>
        string[] GetNotExistingStrategyGroups(string[] strategyGroupIdsNames);


        /// <summary>
        /// Gets faculties by identifier
        /// </summary>
        /// <param name="strategyGroupIds">StrategyGroup identifiers</param>
        /// <returns>StrategyGroups</returns>
        List<StrategyGroup> GetStrategyGroupsByIds(int[] strategyGroupIds);
        List<StrategyGroup> GetAllStrategyGroups();

    }
}
