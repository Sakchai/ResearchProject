using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Research.Web.Models.Logging
{
    /// <summary>
    /// Represents a log search model
    /// </summary>
    public partial class LogSearchModel : BaseSearchModel
    {
        #region Ctor

        public LogSearchModel()
        {
            AvailableLogLevels = new List<SelectListItem>();
        }

        #endregion

        #region Properties


        [Display(Name = "วันที่จาก")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnFrom { get; set; }

        [Display(Name = "ถึงวันที่")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnTo { get; set; }

        [Display(Name = "รายละเอียด")]
        public string Message { get; set; }

        [Display(Name = "ประเภท Log")]
        public int LogLevelId { get; set; }

        public IList<SelectListItem> AvailableLogLevels { get; set; }

        #endregion
    }
}