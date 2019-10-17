using Research.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.Dashboard
{
    /// <summary>
    /// Represents a log model
    /// </summary>
    public partial class DashboardModel 
    {

        public DashboardModel()
        {
            FundAmountByFaculties = new List<FundAmountByFacultyModel>();
            NoOfProjectByFaculties = new List<NoOfProjectByFacultyModel>();
        }

        public string FiscalYearList { get; set; }
        public string ProjectList { get; set; }
        public string FundAmountList { get; set; }
        public string FacultyList { get; set; }
        public IList<FundAmountByFacultyModel> FundAmountByFaculties { get; set; }
        public IList<NoOfProjectByFacultyModel> NoOfProjectByFaculties { get; set; }
        #region Properties

        public string FiscalYear { get; set; }
        
        public string FundAmount { get; set; }

        public string NoOfProject { get; set; }

        public string NoOfReseacher { get; set; }

        public string NoOfProfessor { get; set; }
        #endregion
    }
}