using Research.Enum;
using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class ResearcherEducation : BaseEntity
    {
        public int ResearcherId { get; set; }
        public int EducationLevelId { get; set; }
        public string EducationLevelName { get; set; }
        public int InstituteId { get; set; }
        public string InstituteName { get; set; }
        public string CountryName { get; set; }
        public int DegreeId { get; set; }

        public virtual Degree Degree
        {
            get => (Degree)DegreeId;
            set => DegreeId = (int)value;
        }
        public int CountryId { get; set; }
        public int GraduationYear { get; set; }
        public virtual Country Country { get; set; }
        public virtual EducationLevel EducationLevel { get; set; }
        public virtual Institute Institute { get; set; }
        public virtual Researcher Researcher { get; set; }
    }
}