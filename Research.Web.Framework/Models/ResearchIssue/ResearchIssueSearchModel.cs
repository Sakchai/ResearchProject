using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Research.Web.Models.ResearchIssues
{
    /// <summary>
    /// Represents a topic search model
    /// </summary>
    public partial class ResearchIssueSearchModel : BaseSearchModel
    {
        [Display(Name = "ประเด็นการวิจัย")]
        public string Name { get; set; }
    }
}