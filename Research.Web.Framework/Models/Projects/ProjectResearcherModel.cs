
using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Data;
using Research.Enum;
using Research.Web.Framework.Models;
using System.Collections.Generic;

namespace Research.Web.Models.Projects
{
    public class ProjectResearcherModel : BaseEntityModel
    {
        public ProjectResearcherModel()
        {
            AvailableResearchers = new List<SelectListItem>();
            AvailableResearcherRoles = new List<SelectListItem>();
        }
        public int ProjectId { get; set; }
        public int ResearcherId { get; set; }
        public int Portion { get; set; }
        public int ProjectRoleId { get; set; }
        public string RoleName { get; set; }
        public string ResearcherName { get; set; }

        public IList<SelectListItem> AvailableResearchers { get; set; }
        public IList<SelectListItem> AvailableResearcherRoles { get; set; }
    }
}
