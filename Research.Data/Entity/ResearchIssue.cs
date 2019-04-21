using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class ResearchIssue : BaseEntity
    {
        ICollection<Project> _projects;

        public int FiscalYear { get; set; }
        public string IssueCode { get; set; }
        public string Name { get; set; }
        public string LastUpdateBy { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<Project> Projects {
            get => _projects ?? (_projects = new List<Project>());
            set => _projects = value;
        }
    }
}