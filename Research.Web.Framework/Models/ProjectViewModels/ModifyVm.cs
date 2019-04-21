using Research.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Domain;
using Research.Web.Common;
using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.Projects
{
    public class ModifyVm : BaseSearchModel
    {
        public ModifyVm()
        {
            this.AvailableFiscalSchedules = new List<SelectListItem>();
            this.AvailableProfessors = new List<SelectListItem>();
            this.AvailableProjectStatuses = new List<SelectListItem>();
            this.AvailableResearchIssues = new List<SelectListItem>();
            this.ProjectResearchers = new List<ProjectResearcherViewModel>();
        }
        public int ProjectId { get; set; }
        [Display(Name = "รหัสโครงการวิจัย")]
        public string ProjectCode { get; set; }
        [Display(Name = "ชื่อโครงการวิจัย")]
        public string ProjectNameTh { get; set; }
        [Display(Name = "ปีงบประมาณ")]
        public int FiscalYear { get; set; }
        [Display(Name = "ประเด็นการวิจัย")]
        public int ResearchIssueId { get; set; }
        public IList<SelectListItem> AvailableResearchIssues { get; set; }
        [Display(Name = "งบประมาณ")]
        public decimal FundAmount { get; set; }
        [Display(Name = "ผู้ทรงคุณวุฒิ (ภายใน)")]
        public int InternalProfessorId { get; set; }
        [Display(Name = "ผู้ทรงคุณวุฒิ (ภายใน)")]
        public int InternalProfessor2Id { get; set; }
        [Display(Name = "ผู้ทรงคุณวุฒิ (ภายนอก)")]
        public int ExternalProfessorId { get; set; }

        public IList<SelectListItem> AvailableProfessors { get; set; }
        [Display(Name = "ข้อเสนอโครงการวิจัย")]
        public int ProposalUploadId { get; set; }
        [Display(Name = "ผลการพิจารณา")]
        public int ProjectStatusId { get; set; }
        public IList<SelectListItem> AvailableProjectStatuses { get; set; }
        [Display(Name = "วันที่ทำสัญญา")]
        public DateTime StartContractDate { get; set; }
        [Display(Name = "วันที่สิ้นสุดสัญญา")]
        public DateTime EndContractDate { get; set; }
        public string Published { get; set; }
        public string Utilization { get; set; }
        public string AbstractTitle { get; set; }
        public int CompletedUploadId { get; set; }
        public int FiscalScheduleId { get; set; }
        [Display(Name = "วันที่แก้ไขข้อมูล")]
        public DateTime? Modified { get; set; }
        [Display(Name = "แก้ไขข้อมูลโดย")]
        public string LastUpdateBy { get; set; }

        public IList<SelectListItem> AvailableFiscalSchedules { get; set; }
        public virtual ICollection<ProjectResearcherViewModel> ProjectResearchers { get; set; }
    }
}
