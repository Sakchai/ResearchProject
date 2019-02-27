using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class ResearchIssue : BaseEntity
    {
        public ResearchIssue()
        {
            Project = new HashSet<Project>();
        }

        public int FiscalYear { get; set; }
        public string IssueCode { get; set; }
        public string Name { get; set; }
        public string LastUpdateBy { get; set; }

        public virtual ICollection<Project> Project { get; set; }
    }
}