

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Research.Web.Models.Researchers
{
    public class ResearcherEducationModel : BaseEntityModel
    {
        public ResearcherEducationModel()
        {
            AvailableCountries = new List<SelectListItem>();
            AvailableEducationLevels = new List<SelectListItem>();
            AvailableInstitutes = new List<SelectListItem>();
        }
        public int ResearcherId { get; set; }
        public int EducationLevelId { get; set; }
        public string EducationLevelName { get; set; }
        public int InstituteId { get; set; }
        public string InstituteName { get; set; }
        public int DegreeId { get; set; }
        public string Degress { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int GraduationYear { get; set; }
        public IList<SelectListItem> AvailableCountries { get; set; }
        public IList<SelectListItem> AvailableEducationLevels { get; set; }
        public IList<SelectListItem> AvailableInstitutes { get; set; }
    }
}
