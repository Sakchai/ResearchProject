using Research.Enum;
using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class AcademicRank : BaseEntity
    {
        public AcademicRank()
        {
        }
        public int PersonTypeId { get; set; }
        public PersonType PersonType {
            get => (PersonType) PersonTypeId;
            set => PersonTypeId = (int) value; 
        }


        public string NameTh { get; set; }
        public string NameEn { get; set; }

    }
}