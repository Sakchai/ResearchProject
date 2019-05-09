
using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Data;
using Research.Enum;
using Research.Web.Framework.Models;
using System.Collections.Generic;

namespace Research.Web.Models.Researchers
{
    /// <summary>
    /// A ... attached to an Researcher
    /// </summary>
    public class ResearcherModel : BaseEntityModel
    {
        public ResearcherModel()
        {
            AvailableTitles = new List<SelectListItem>();
            AvailablePersonTypes = new List<SelectListItem>();
            AvailableAgencies = new List<SelectListItem>();
        }
        public IList<SelectListItem> AvailableTitles { get; set; }
        public IList<SelectListItem> AvailablePersonTypes { get; set; }
        public IList<SelectListItem> AvailableAgencies { get; set; }
        public int TitleId { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string IDCard { get; set; }
        public string ResearcherCode { get; set; }
        public int PersonTypeId { get; set; }
        public string PersonTypeName { get; set; }

        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsAcceptedConditions { get; set; }
        public string PhoneNumber { get; set; }
        public string LastUpdateBy { get; set; }
        public int AgencyId { get; set; }
        public string AgencyName { get; set; }
        public string FullName { get => $"{TitleName}{FirstName} {LastName}"; }
    
        public bool IsCompleted { get => IsActive && IsAcceptedConditions; }
    }
}
