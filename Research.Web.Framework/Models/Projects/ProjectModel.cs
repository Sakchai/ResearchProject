
using Research.Enum;
using Research.Data;
using System;
using System.Collections.Generic;
using Research.Web.Models.Researchers;
using Research.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.Projects
{
    /// <summary>
    /// A Researcher attached to a Project
    /// </summary>
    public class ProjectModel : BaseEntityModel
    {
        public ProjectModel()
        {
            AvailableFiscalSchedules = new List<SelectListItem>();
            AvailableProjectStatuses = new List<SelectListItem>();
            AvailableProfessors = new List<SelectListItem>();
            AvailableResearchIssues = new List<SelectListItem>();
        }
        public string ProjectCode { get; set; }
        public string ProjectNameTh { get; set; }
        public string ProjectNameEn { get; set; }
        public string PlanNameTh { get; set; }
        public string PlanNameEn { get; set; }
        public int FiscalYear { get; set; }
        [Display(Name = "ประเด็นการวิจัย")]
        public int ResearchIssueId { get; set; }
        public decimal FundAmount { get; set; }
        public int InternalProfessorId { get; set; }
        public int InternalProfessor2Id { get; set; }
        [Display(Name = "ผู้ทรงคุณวุฒิ (ภายนอก)")]
        public int ExternalProfessorId { get; set; }
        public int ProjectUploadId { get; set; }
        public DateTime StartContractDate { get; set; }
        public DateTime EndContractDate { get; set; }
        public int FiscalScheduleId { get; set; }
        public string LastUpdateBy { get; set; }
        public virtual ICollection<ResearcherModel> Researchers { get; set; }

        public IList<SelectListItem> AvailableFiscalSchedules { get; set; }
        [Display(Name = "ผลการพิจารณา")]
        public int ProjectStatusId { get; set; }
        public IList<SelectListItem> AvailableProjectStatuses { get; set; }
        public IList<SelectListItem> AvailableProfessors { get; set; }
        public IList<SelectListItem> AvailableResearchIssues { get; set; }
    }
}