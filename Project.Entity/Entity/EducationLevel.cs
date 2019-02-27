using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class EducationLevel : BaseEntity
    {
        public EducationLevel()
        {
            ResearcherEducation = new HashSet<ResearcherEducation>();
        }

        public string Name { get; set; }
        public int DisplayOrder { get; set; }

        public virtual ICollection<ResearcherEducation> ResearcherEducation { get; set; }
    }
}