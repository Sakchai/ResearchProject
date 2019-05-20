using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Web.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.ResearchIssues
{
    /// <summary>
    /// A ... attached to an Researcher
    /// </summary>
    public class ResearchIssueModel : BaseEntityModel
    {
        public ResearchIssueModel()
        {
            AvailableFiscalYears = new List<SelectListItem>();
        }
        [Display(Name = "ปี")]
        public int FiscalYear { get; set; }
        [Display(Name = "รหัส")]
        public string IssueCode { get; set; }
        public IList<SelectListItem> AvailableFiscalYears { get; set; }
        [Display(Name = "ประเด็นการวิจัย")]
        public string Name { get; set; }

    }
}
