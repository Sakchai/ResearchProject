using Project.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain
{
    public class ProjectResearcherViewModel : BaseDomain
    {
        public int ProjectId { get; set; }
        public int ResearcherId { get; set; }
        public int Portion { get; set; }
        public int ProjectRoleId { get; set; }
        public string ResearcherName { get; set; }

        public virtual string ProjectRole { get { return ((ProjectRole)ProjectRoleId).ToString(); } }
    }
}
