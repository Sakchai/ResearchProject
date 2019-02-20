using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class Institute : BaseEntity
    {
        public Institute()
        {
            ResearcherEducation = new HashSet<ResearcherEducation>();
        }

        public string Name { get; set; }
        public virtual ICollection<ResearcherEducation> ResearcherEducation { get; set; }
    }
}