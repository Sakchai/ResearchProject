using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Research.Web.Models.Researchers
{
    /// <summary>
    /// Represents a topic search model
    /// </summary>
    public partial class ResearcherSearchModel : BaseSearchModel
    {
        #region Ctor

        public ResearcherSearchModel()
        {
            AvailableAgencies = new List<SelectListItem>();
            AvailablePersonTypes = new List<SelectListItem>();
            AvailableActiveStatues = new List<SelectListItem>();
        }

        #endregion

        #region Properties
        [Display(Name = "หน่วยงานหลัก")]
        public int AgencyId { get; set; }
        [Display(Name = "ประเภทบุคคลากร")]
        public int PersonTypeId { get; set; }
        public IList<SelectListItem> AvailableAgencies { get; set; }
        public IList<SelectListItem> AvailablePersonTypes { get; set; }
        public IList<SelectListItem> AvailableActiveStatues { get; set; }
        [Display(Name = "ชื่อ")]
        public string FirstName { get; set; }
        [Display(Name = "นามสกุล")]
        public string LastName { get; set; }
        [Display(Name = "สถานะเข้าใช้งานระบบ")]
        public int IsCompleted { get; set; }
        #endregion
    }
}