using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class Country : BaseEntity
    {
        public Country()
        {
            ResearcherEducation = new HashSet<ResearcherEducation>();
        }

        public string Name { get; set; }
        public virtual ICollection<ResearcherEducation> ResearcherEducation { get; set; }
    }
}