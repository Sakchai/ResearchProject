
using Research.Enum;
using Research.Data;
using System;
using System.Collections.Generic;
using Research.Web.Models.Researchers;
using Research.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Research.Web.Validators.Projects;

namespace Research.Web.Models.Projects
{
    /// <summary>
    /// A Researcher attached to a Project
    /// </summary>
    [Validator(typeof(ProjectValidator))]
    public class ProjectModel : BaseEntityModel
    {
        public ProjectModel()
        {
            AvailableProjectStatuses = new List<SelectListItem>();
            AvailableProfessors = new List<SelectListItem>();
            AvailableResearchIssues = new List<SelectListItem>();
            AvailableStrategyGroups = new List<SelectListItem>();
            AvailableProfessorTypes = new List<SelectListItem>();
            AvailableResearchers = new List<SelectListItem>();
            AvailableProjectRoles = new List<SelectListItem>();
            ProjectResearcherSearchModel = new ProjectResearcherSearchModel();
            ProjectProfessorSearchModel = new ProjectProfessorSearchModel();
            ProjectProgressSearchModel = new ProjectProgressSearchModel();
            AvailableProgressStatuses = new List<SelectListItem>();
            ProjectResearcherListModel = new ProjectResearcherListModel();
            ProjectProfessorListModel = new ProjectProfessorListModel();
        }
        public string CreatedName { get; set; }
        [Display(Name = "รหัสโครงการวิจัย")]
        public string ProjectCode { get; set; }
        [Required]
        [Display(Name = "ชื่อโครงการวิจัย(ภาษาไทย)")]
        public string ProjectNameTh { get; set; }
        [Display(Name = "ลักษณะโครงการวิจัย(ภาษาอังกฤษ)")]
        [Required]
        public string ProjectNameEn { get; set; }
        [Display(Name = "ชื่อแผนงานวิจัย(ภาษาไทย)ถ้ามี")]
        public string PlanNameTh { get; set; }
        [Display(Name = "ชื่อแผนงานวิจัย(ภาษาอังกฤษ)ถ้ามี")]
        public string PlanNameEn { get; set; }
        [Required]
        [Display(Name = "ปีงบประมาณ")]
        public int FiscalYear { get; set; }
        [Display(Name = "ลักษณะโครงการวิจัย")]
        public string ProjectType { get; set; }
        public string ProjectTypeName { get; set; }
        [Display(Name = "ประเด็นการวิจัย")]
        public int ResearchIssueId { get; set; }
        public string ResearchIssueName { get; set; }
        [Required]
        [Display(Name = "งบประมาณ")]
        public decimal FundAmount { get; set; }
        public string ProjectStatusName { get; set; }
        public string ProgressStatusName { get; set; }
        public int ProjectUploadId { get; set; }
        [UIHint("Date")]
        [Display(Name = "วันที่ทำสัญญา (MM/dd/YYYY)")]
        public DateTime ProjectStartDate { get; set; }
        public string ProjectStartDateName { get; set; }
        [UIHint("Date")]
        [Display(Name = "วันที่สิ้นสุดสัญญา (MM/dd/YYYY)")]
        public DateTime ProjectEndDate { get; set; }
        public string ProjectEndDateName { get; set; }
        public string LastUpdateBy { get; set; }
        [Display(Name = "หมายเหตุ")]
        public string Comment { get; set; }
        [Display(Name = "กลุ่มเรื่องตามแนวยุทธศาสตร์มหาวิทยาลัย")]
        public int? StrategyGroupId { get; set; }
        public string StrategyGroupName { get; set; }
        public IList<SelectListItem> AvailableStrategyGroups { get; set; }

        public virtual ICollection<ResearcherModel> Researchers { get; set; }

        [Display(Name = "ผลการพิจารณา")]
        public int? ProjectStatusId { get; set; }
        public IList<SelectListItem> AvailableProjectStatuses { get; set; }

        public IList<SelectListItem> AvailableResearchIssues { get; set; }

        #region project researchers
        public ProjectResearcherSearchModel ProjectResearcherSearchModel { get; set; }
        public ProjectResearcherListModel ProjectResearcherListModel { get; set; }
        public ProjectProfessorListModel ProjectProfessorListModel { get; set; }
        [Display(Name = "ชื่อนักวิจัย")]
        public int AddProjectResearcherId { get; set; }
        [Display(Name = "ชื่อนักวิจัย")]
        public string AddProjectResearcherName { get; set; }

        public IList<SelectListItem> AvailableResearchers { get; set; }
        [Display(Name = "บทบาทในโครงการ")]
        public int AddProjectRoleId { get; set; }
        public IList<SelectListItem> AvailableProjectRoles { get; set; }
        [Display(Name = "สัดส่วน")]
        public int AddProjectPortion { get; set; }
        #endregion

        #region project professors
        public ProjectProfessorSearchModel ProjectProfessorSearchModel { get; set; }
        [Display(Name = "ชื่อผู้ทรงคุณวุฒิ")]
        public int AddProjectProfessorId { get; set; }
        [Display(Name = "ชื่อผู้ทรงคุณวุฒิ")]
        public string AddProjectProfessorName { get; set; }

        [Display(Name = "บทบาทในโครงการ")]
        public int AddProjectProfessorTypeId { get; set; }

        [Display(Name = "ข้อเสนอโครงการวิจัย (pdf)")]
        [UIHint("Download")]
        public int AddProjectUploadId { get; set; }
        public IList<SelectListItem> AvailableProfessorTypes { get; set; }
        public IList<SelectListItem> AvailableProfessors { get; set; }
        #endregion

        #region project progresses
        public ProjectProgressSearchModel ProjectProgressSearchModel { get; set; }
        [Display(Name = "สถานะโครงการวิจัย")]
        public int AddProjectProgressStatusId { get; set; }

        [UIHint("Date")]
        [Display(Name = "วันที่เริ่มต้น (MM/dd/YYYY)")]
        public DateTime AddProjectProgressStartDate { get; set; }
        [UIHint("Date")]
        [Display(Name = "วันที่สิ้นสุด (MM/dd/YYYY)")]
        public DateTime AddProjectProgressEndDate { get; set; }
        [Display(Name = "หมายเหตุ")]
        public string AddProjectProgressComment { get; set; }

        public IList<SelectListItem> AvailableProgressStatuses { get; set; }

        #endregion
    }
}