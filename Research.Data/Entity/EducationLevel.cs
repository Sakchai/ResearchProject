using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class EducationLevel : BaseEntity
    {
        //private ICollection<ResearcherEducation> _researcherEducations;

        public string Name { get; set; }
        public int DisplayOrder { get; set; }

        //public virtual ICollection<ResearcherEducation> ResearcherEducations {
        //    get => _researcherEducations ?? (_researcherEducations = new List<ResearcherEducation>());
        //    set => _researcherEducations = value;
        //}
    }
}