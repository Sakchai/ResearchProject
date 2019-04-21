using Research.Enum;
using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class Researcher : BaseEntity
    {
        private ICollection<ProjectResearcher> _projectResearchers;
        private ICollection<ResearcherEducation> _researcherEducations;
        private ICollection<ResearcherHistory> _researcherHistories;
        private ICollection<User> _users;

        public int TitleId { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDCard { get; set; }
        public string ResearcherCode { get; set; }
        public int? PersonTypeId { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool? IsAcceptedConditions { get; set; }
        public string Telephone { get; set; }
        public int? PictureId { get; set; }
        public int? AgencyId { get; set; }
        public int? AddressId { get; set; }
        public int? AcademicRankId { get; set; }
        public string LastUpdateBy { get; set; }
        public int GenderId { get; set; }
        public Gender Gender
        {
            get => (Gender) GenderId;
            set => GenderId = (int) value;
        }
        public PersonType PersonType {
            get => (PersonType)PersonTypeId;
            set => PersonType = value;
        }
        public virtual PictureBinary Picture { get; set; }
        public virtual Title Title { get; set; }
        public virtual AcademicRank AcademicRank { get; set; }
        public virtual Address Address { get; set; }
        public virtual Agency Agency { get; set; }
        public virtual ICollection<ProjectResearcher> ProjectResearchers {
            get => _projectResearchers ?? (_projectResearchers = new List<ProjectResearcher>());
            set => _projectResearchers = value;
        }
        public virtual ICollection<ResearcherEducation> ResearcherEducations {
            get => _researcherEducations ?? (_researcherEducations = new List<ResearcherEducation>());
            set => _researcherEducations = value;
        }
        public virtual ICollection<ResearcherHistory> ResearcherHistories {
            get => _researcherHistories ?? (_researcherHistories = new List<ResearcherHistory>());
            set => _researcherHistories = value;
        }
        public virtual ICollection<User> Users {
            get => _users ?? (_users = new List<User>());
            set => _users = value;
        }
    }


}