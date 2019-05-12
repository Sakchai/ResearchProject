using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.Projects
{
    public class ProjectSearchModel : BaseSearchModel
    {

        public ProjectSearchModel()
        {
            AvailableFiscalYears = new List<SelectListItem>();
            AvailableAgencies = new List<SelectListItem>();
            AvailableProjectStatuses = new List<SelectListItem>();
            AvailableProgressStatuses = new List<SelectListItem>();
        }

        public int Id { get; set; }

        [Display(Name = "ชื่อโครงการวิจัย")]
        public string SearchProjectName { get; set; }

        [Display(Name = "ปีงบประมาณ")]
        public int FiscalScheduleId { get; set; }

        [Display(Name = "หน่วยงาน")]
        public int AgencyId { get; set; }

        [Display(Name = "สถานะโครงการวิจัย")]
        public int ProgressStatusId { get; set; }

        [Display(Name = "สถานะผลการพิจารณา")]
        public int ProjectStatusId { get; set; }

        public IList<SelectListItem> AvailableFiscalYears { get; set; }
        public IList<SelectListItem> AvailableAgencies { get; set; }
        public IList<SelectListItem> AvailableProjectStatuses { get; set; }
        public IList<SelectListItem> AvailableProgressStatuses { get; set; }
    }
}