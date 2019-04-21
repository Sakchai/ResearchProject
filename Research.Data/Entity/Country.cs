using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class Country : BaseEntity
    {
        private ICollection<ResearcherEducation> _researcherEducations;
        private ICollection<Province> _provinces;

        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the two letter ISO code
        /// </summary>
        public string TwoLetterIsoCode { get; set; }

        /// <summary>
        /// Gets or sets the three letter ISO code
        /// </summary>
        public string ThreeLetterIsoCode { get; set; }

        /// <summary>
        /// Gets or sets the numeric ISO code
        /// </summary>
        public int NumericIsoCode { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
        public virtual ICollection<ResearcherEducation> ResearcherEducations {
            get => _researcherEducations ?? (_researcherEducations = new List<ResearcherEducation>());
            set => _researcherEducations = value;
        }
        /// <summary>
        /// Gets or sets the state/provinces
        /// </summary>
        public virtual ICollection<Province> Provinces
        {
            get => _provinces ?? (_provinces = new List<Province>());
            protected set => _provinces = value;
        }
    }
}