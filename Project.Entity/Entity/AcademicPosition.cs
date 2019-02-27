using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class AcademicPosition : BaseEntity
    {
        public AcademicPosition()
        {
            ProfessorAcademicPosition = new HashSet<ProfessorAcademicPosition>();
            ResearcherAcademicPosition = new HashSet<ResearcherAcademicPosition>();
        }

        public string NameTh { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<ProfessorAcademicPosition> ProfessorAcademicPosition { get; set; }
        public virtual ICollection<ResearcherAcademicPosition> ResearcherAcademicPosition { get; set; }
    }
}