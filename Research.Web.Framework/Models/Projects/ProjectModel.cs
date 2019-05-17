
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
        }
        [Display(Name = "รหัสโครงการวิจัย")]
        public string ProjectCode { get; set; }
        [Display(Name = "ชื่อโครงการวิจัย(ภาษาไทย)")]
        public string ProjectNameTh { get; set; }
        [Display(Name = "ลักษณะโครงการวิจัย(ภาษาอังกฤษ)")]
        public string ProjectNameEn { get; set; }
        [Display(Name = "ชื่อแผนงานวิจัย(ภาษาไทย)ถ้ามี")]
        public string PlanNameTh { get; set; }
        [Display(Name = "ชื่อแผนงานวิจัย(ภาษาอังกฤษ)ถ้ามี")]
        public string PlanNameEn { get; set; }
        [Display(Name = "ปีงบประมาณ")]
        public int FiscalYear { get; set; }
        [Display(Name = "ลักษณะโครงการวิจัย")]
        public string ProjectType { get; set; }
        [Display(Name = "ประเด็นการวิจัย")]
        public int ResearchIssueId { get; set; }
        [Display(Name = "งบประมาณ")]
        public decimal FundAmount { get; set; }
        public string ProjectStatusName { get; set; }
        public string ProgressStatusName { get; set; }
        public int ProjectUploadId { get; set; }
        [Display(Name = "วันที่ทำสัญญา")]
        public DateTime StartContractDate { get; set; }
        public string StartContractDateName { get; set; }
        [Display(Name = "วันที่สิ้นสุดสัญญา")]
        public DateTime EndContractDate { get; set; }
        public string LastUpdateBy { get; set; }
        [Display(Name = "หมายเหตุ")]
        public string Comment { get; set; }
        [Display(Name = "กลุ่มเรื่องตามแนวยุทธศาสตร์มหาวิทยาลัย")]
        public int? StrategyGroupId { get; set; }
        public IList<SelectListItem> AvailableStrategyGroups { get; set; }

        public virtual ICollection<ResearcherModel> Researchers { get; set; }

        [Display(Name = "ผลการพิจารณา")]
        public int? ProjectStatusId { get; set; }
        public IList<SelectListItem> AvailableProjectStatuses { get; set; }

        public IList<SelectListItem> AvailableResearchIssues { get; set; }

        #region project researchers
        public ProjectResearcherSearchModel ProjectResearcherSearchModel { get; set; }
        [Display(Name = "ชื่อนักวิจัย")]
        public int AddProjectResearcherId { get; set; }
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
        [Display(Name = "บทบาทในโครงการ")]
        public int AddProjectProfessorTypeId { get; set; }
        public IList<SelectListItem> AvailableProfessorTypes { get; set; }
        public IList<SelectListItem> AvailableProfessors { get; set; }
        #endregion

        #region project progresses
        public ProjectProgressSearchModel ProjectProgressSearchModel { get; set; }
        [Display(Name = "สถานะโครงการวิจัย")]
        public int AddProjectProgressStatusId { get; set; }
        [Display(Name = "วันที่เริ่มต้น")]
        public DateTime AddProjectProgressStartDate { get; set; }
        [Display(Name = "วันที่สิ้นสุด")]
        public DateTime AddProjectProgressEndDate { get; set; }
        [Display(Name = "หมายเหตุ")]
        public string AddProjectProgressComment { get; set; }

        public IList<SelectListItem> AvailableProgressStatuses { get; set; }

        #endregion
    }
}