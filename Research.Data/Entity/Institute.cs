using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class Institute : BaseEntity
    {
        //ICollection<ResearcherEducation> _researcherEducations;
        //public Institute()
        //{

        //}

        public string Name { get; set; }
        //public virtual ICollection<ResearcherEducation> ResearcherEducations {
        //    get => _researcherEducations ?? (_researcherEducations = new List<ResearcherEducation>());
        //    set => _researcherEducations = value;
        //}
    }
}