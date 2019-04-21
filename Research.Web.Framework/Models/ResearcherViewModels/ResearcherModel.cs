using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Enum;
using System;
using System.Collections.Generic;

namespace Research.Web.Models.Researchers
{
    public partial class ResearcherModel
    {
        public ResearcherModel()
        {
            AvailableProjectRoles = new List<SelectListItem>();
            AvailableResearchers = new List<SelectListItem>();
        }
        public int ProjectId { get; set; }
        public int ResearcherId { get; set; }
        public int Portion { get; set; }
        public int ProjectRoleId { get; set; }
        public virtual IList<SelectListItem> AvailableProjectRoles { get; set; }
        public virtual IList<SelectListItem> AvailableResearchers { get; set; }
    }


}