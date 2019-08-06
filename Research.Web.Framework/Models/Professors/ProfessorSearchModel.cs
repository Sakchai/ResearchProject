using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Research.Web.Models.Professors
{
    /// <summary>
    /// Represents a topic search model
    /// </summary>
    public partial class ProfessorSearchModel : BaseSearchModel
    {
        #region Ctor

        public ProfessorSearchModel()
        {
            AvailableTitles = new List<SelectListItem>();
            AvailableProfessorTypes = new List<SelectListItem>();
        }

        #endregion

        #region Properties
        [Display(Name = "ประเภทผู้ทรงคุณวุฒิ")]
        public int ProfessorTypeId { get; set; }
        [Display(Name = "คำนำหน้าชื่อ")]
        public int TitleId { get; set; }
        public IList<SelectListItem> AvailableTitles { get; set; }
        public IList<SelectListItem> AvailableProfessorTypes { get; set; }
        [Display(Name = "ชื่อ")]
        public string FirstName { get; set; }
        [Display(Name = "นามสกุล")]
        public string LastName { get; set; }

        #endregion
    }
}