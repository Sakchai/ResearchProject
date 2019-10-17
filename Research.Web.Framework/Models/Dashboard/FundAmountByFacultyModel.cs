using Research.Web.Framework.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.Dashboard
{
    /// <summary>
    /// Represents a log model
    /// </summary>
    public partial class FundAmountByFacultyModel
    {
        #region Properties

        public string FiscalYear { get; set; }
        
        public string FundAmount { get; set; }

        public string Faculty { get; set; }
        #endregion
    }
}