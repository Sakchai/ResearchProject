using System;
using System.Collections.Generic;
using Research.Core;
using Research.Data;

namespace Research.Services.FiscalSchedules
{
    /// <summary>
    /// FiscalSchedule service interface
    /// </summary>
    public partial interface IFiscalScheduleService
    {
        /// <summary>
        /// Delete fiscalSchedule
        /// </summary>
        /// <param name="fiscalSchedule">FiscalSchedule</param>
        void DeleteFiscalSchedule(FiscalSchedule fiscalSchedule);

        /// <summary>
        /// Gets all faculties
        /// </summary>
        /// <param name="fiscalScheduleName">FiscalSchedule name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>FiscalSchedules</returns>

        IPagedList<FiscalSchedule> GetAllFiscalSchedules(string fiscalScheduleName,DateTime? openingDate = null, DateTime? closingDate = null, int fiscalYear = 0,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Gets a fiscalSchedule
        /// </summary>
        /// <param name="fiscalScheduleId">FiscalSchedule identifier</param>
        /// <returns>FiscalSchedule</returns>
        FiscalSchedule GetFiscalScheduleById(int fiscalScheduleId);

        /// <summary>
        /// Inserts fiscalSchedule
        /// </summary>
        /// <param name="fiscalSchedule">FiscalSchedule</param>
        void InsertFiscalSchedule(FiscalSchedule fiscalSchedule);

        /// <summary>
        /// Updates the fiscalSchedule
        /// </summary>
        /// <param name="fiscalSchedule">FiscalSchedule</param>
        void UpdateFiscalSchedule(FiscalSchedule fiscalSchedule);

        /// <summary>
        /// Returns a list of names of not existing faculties
        /// </summary>
        /// <param name="fiscalScheduleIdsNames">The names and/or IDs of the faculties to check</param>
        /// <returns>List of names and/or IDs not existing faculties</returns>
        string[] GetNotExistingFiscalSchedules(string[] fiscalScheduleIdsNames);


        /// <summary>
        /// Gets faculties by identifier
        /// </summary>
        /// <param name="fiscalScheduleIds">FiscalSchedule identifiers</param>
        /// <returns>FiscalSchedules</returns>
        List<FiscalSchedule> GetFiscalSchedulesByIds(int[] fiscalScheduleIds);
        List<FiscalSchedule> GetAllFiscalSchedules();
        string GetNextNumber();
    }
}
