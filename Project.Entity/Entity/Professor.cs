using Project.Enum;
using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class Professor : BaseEntity
    {
        public Professor()
        {
            ProfessorAcademicPosition = new HashSet<ProfessorAcademicPosition>();
            ProfessorAddress = new HashSet<ProfessorAddress>();
        }

        public int TitleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfessorCode { get; set; }
        public int? ProfessorTypeId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ProfessorType ProfessorType { get { return (ProfessorType)ProfessorTypeId; } }
        public virtual ICollection<ProfessorAcademicPosition> ProfessorAcademicPosition { get; set; }
        public virtual ICollection<ProfessorAddress> ProfessorAddress { get; set; }
    }


}