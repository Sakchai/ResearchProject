using Project.Entity;
using Project.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Domain
{
    /// <summary>
    /// A ... attached to an Researcher
    /// </summary>
    public class ResearcherViewModel : BaseDomain
    {
        public int TitleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Sex { get; set; }
        public string IDCard { get; set; }
        public string ResearcherCode { get; set; }
        public int PersonTypeId { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsAcceptedConditions { get; set; }
        public string PhoneNumber { get; set; }
        public int PictureId { get; set; }
        public int FacultyId { get; set; }
        public string LastUpdateBy { get; set; }
        public virtual Gender Gender { get { return (Gender)Sex; } }
        public virtual PersonType PersonType { get { return (PersonType)PersonTypeId; } }
    
    }
}
