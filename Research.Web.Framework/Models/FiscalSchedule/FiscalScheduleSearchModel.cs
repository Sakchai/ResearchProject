using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Research.Web.Models.FiscalSchedules
{
    /// <summary>
    /// Represents a topic search model
    /// </summary>
    public partial class FiscalScheduleSearchModel : BaseSearchModel
    {
        [Display(Name = "ชื่อทุนวิจัย")]
        public string ScholarName { get; set; }
    }
}