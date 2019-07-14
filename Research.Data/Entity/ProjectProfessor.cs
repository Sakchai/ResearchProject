using Research.Enum;
using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class ProjectProfessor : BaseEntity
    {
        public int ProjectId { get; set; }
        public int ProfessorId { get; set; }
        public int ProfessorTypeId { get; set; }
        public string ProfessorName { get; set; }
        //public DateTime ProgressStartDate { get; set; }
        //public DateTime ProgressEndDate { get; set; }
        //public string Comment { get; set; }
        public ProfessorType ProfessorType
        {
            get => (ProfessorType)ProfessorTypeId;
            set => ProfessorTypeId = (int)value;
        }
        public virtual Project Project { get; set; }
        public virtual Professor Professor { get; set; }
    }
}