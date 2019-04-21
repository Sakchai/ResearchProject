using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class ResearcherHistory : BaseEntity
    {
        public int ResearcherId { get; set; }
        public int TitleId { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastUpdateBy { get; set; }

        public virtual Researcher Researcher { get; set; }
        public virtual Title Title { get; set; }
    }
}