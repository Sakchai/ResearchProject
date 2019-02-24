using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Web.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Web.Models.ProjectViewModels
{
    public class ProjectSearchModel : BaseSearchModel
    {
        public ProjectSearchModel()
        {
            AvailableFiscalYears = new List<SelectListItem>();
            AvailableFaculties = new List<SelectListItem>();
            AvailableProjectStatuses = new List<SelectListItem>();
            AvailableResearchStatuses = new List<SelectListItem>();
        }

        public int Id { get; set; }

        [Display(Name = "ชื่อโครงการวิจัย")]
        public string SearchProjectName { get; set; }

        [Display(Name = "ปีงบประมาณ")]
        public int FiscalScheduleId { get; set; }

        [Display(Name = "หน่วยงานหลัก")]
        public int FacultyId { get; set; }

        [Display(Name = "ประเด็นการวิจัย")]
        public int ResearchIssueId { get; set; }

        [Display(Name = "สถานะโครงการ")]
        public int ProjectStatusId { get; set; }

        public IList<SelectListItem> AvailableFiscalYears { get; set; }
        public IList<SelectListItem> AvailableFaculties { get; set; }
        public IList<SelectListItem> AvailableProjectStatuses { get; set; }
        public IList<SelectListItem> AvailableResearchStatuses { get; set; }
    }
}