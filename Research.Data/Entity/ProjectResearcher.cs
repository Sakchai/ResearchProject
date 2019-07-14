using Research.Enum;
using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class ProjectResearcher : BaseEntity
    {
        public int ProjectId { get; set; }
        public int ResearcherId { get; set; }
        public string ResearcherName { get; set; }
        public int Portion { get; set; }
        public int ProjectRoleId { get; set; }

        public ProjectRole ProjectRole 
        {
            get => (ProjectRole)ProjectRoleId;
            set => ProjectRoleId = (int)value;
        }
        public virtual Project Project { get; set; }
        public virtual Researcher Researcher { get; set; }

    }


}