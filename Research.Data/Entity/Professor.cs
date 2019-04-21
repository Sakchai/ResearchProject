using Research.Enum;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class Professor : BaseEntity
    {

        ICollection<ProfessorHistory> _professorHistories;
        //public Professor()
        //{
        //}

        public int TitleId { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfessorCode { get; set; }
        public int? ProfessorTypeId { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public string Comment { get; set; }
        public virtual ProfessorType ProfessorType { get; set; }
        //    get => (ProfessorType)ProfessorTypeId;
        //    set => ProfessorTypeId = (int) value;
        //}

        public int? AddressId { get; set; }
        public virtual Address Address { get; set; }

        public virtual ICollection<ProfessorHistory> ProfessorHistories {
            get => _professorHistories ?? (_professorHistories = new List<ProfessorHistory>());
            set => _professorHistories = value;
        }
        public virtual Title Title { get; set; }
    }


}