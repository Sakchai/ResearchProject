using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class Title : BaseEntity
    {
        public Title()
        {
            Researcher = new HashSet<Researcher>();
            ResearcherHistory = new HashSet<ResearcherHistory>();
        }

        public string TitleName { get; set; }

        public virtual ICollection<Researcher> Researcher { get; set; }
        public virtual ICollection<ResearcherHistory> ResearcherHistory { get; set; }
    }
}