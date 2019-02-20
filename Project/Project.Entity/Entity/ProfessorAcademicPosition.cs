using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class ProfessorAcademicPosition : BaseEntity
    {
        public int ProfessorId { get; set; }
        public int AcademicPostionId { get; set; }

        public virtual AcademicPosition AcademicPostion { get; set; }
        public virtual Professor Professor { get; set; }
    }
}