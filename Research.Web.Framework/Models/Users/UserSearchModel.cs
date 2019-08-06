using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Research.Web.Models.Users
{
    /// <summary>
    /// Represents a topic search model
    /// </summary>
    public partial class UserSearchModel : BaseSearchModel
    {
        public UserSearchModel()
        {
            AvailableAgencyies = new List<SelectListItem>();
        }
        [Display(Name = "ชื่อ")]
        public string FirstName { get; set; }
        [Display(Name = "นามสกุล")]
        public string LastName { get; set; }

        [Display(Name = "หน่วยงาน")]
        public int AgencyId { get; set; }
        public IList<SelectListItem> AvailableAgencyies { get; set; }
    }
}