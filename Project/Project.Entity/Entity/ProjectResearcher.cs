using Project.Enum;
using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class ProjectResearcher : BaseEntity
    {
        public int ProjectId { get; set; }
        public int ResearcherId { get; set; }
        public int Portion { get; set; }
        public int? ProjectRoleId { get; set; }
        public virtual ProjectRole ProjectRole { get { return (ProjectRole)ProjectRoleId; } }
        public virtual Project Project { get; set; }
        public virtual Researcher Researcher { get; set; }
    }


}