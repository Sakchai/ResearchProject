using Research.Web.Framework.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.Dashboard
{
    /// <summary>
    /// Represents a log model
    /// </summary>
    public partial class NoOfProjectByFacultyModel
    {
        #region Properties

        public string FiscalYear { get; set; }
        

        public string NoOfProject { get; set; }

        public string Faculty { get; set; }
        #endregion
    }
}