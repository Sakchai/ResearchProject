using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class ResearcherAcademicPosition : BaseEntity
    {
        public int ResearcherId { get; set; }
        public int AcademicPostionId { get; set; }

        public virtual AcademicPosition AcademicPostion { get; set; }
        public virtual Researcher Researcher { get; set; }
    }
}