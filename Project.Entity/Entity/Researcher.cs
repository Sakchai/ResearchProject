using Project.Enum;
using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class Researcher : BaseEntity
    {
        public Researcher()
        {
            ProjectResearcher = new HashSet<ProjectResearcher>();
            ResearcherAcademicPosition = new HashSet<ResearcherAcademicPosition>();
            ResearcherAddresses = new HashSet<ResearcherAddresses>();
            ResearcherEducation = new HashSet<ResearcherEducation>();
            ResearcherHistory = new HashSet<ResearcherHistory>();
            User = new HashSet<User>();
        }

        public int TitleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Sex { get; set; }
        public string IDCard { get; set; }
        public string ResearcherCode { get; set; }
        public int? PersonTypeId { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public bool IsAcceptedConditions { get; set; }
        public string PhoneNumber { get; set; }
        public int? PictureId { get; set; }
        public int? FacultyId { get; set; }
        public string LastUpdateBy { get; set; }

        public virtual Gender Gender { get { return (Gender)Sex; } }
        public virtual PersonType PersonType { get { return (PersonType)PersonTypeId; } }
        public virtual Faculty Faculty { get; set; }
        public virtual PictureBinary Picture { get; set; }
        public virtual Title Title { get; set; }
        public virtual ICollection<ProjectResearcher> ProjectResearcher { get; set; }
        public virtual ICollection<ResearcherAcademicPosition> ResearcherAcademicPosition { get; set; }
        public virtual ICollection<ResearcherAddresses> ResearcherAddresses { get; set; }
        public virtual ICollection<ResearcherEducation> ResearcherEducation { get; set; }
        public virtual ICollection<ResearcherHistory> ResearcherHistory { get; set; }
        public virtual ICollection<User> User { get; set; }
    }

 
}