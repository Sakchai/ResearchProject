
using Research.Enum;
using Research.Data;
using System;
using System.Collections.Generic;
using Research.Web.Models.Researchers;

namespace Research.Web.Models.Projects
{
    /// <summary>
    /// A Researcher attached to a Project
    /// </summary>
    public class ProjectModel : BaseDomain
    {
        public string ProjectCode { get; set; }
        public string ProjectNameTh { get; set; }
        public string ProjectNameEn { get; set; }
        public string PlanNameTh { get; set; }
        public string PlanNameEn { get; set; }
        public int FiscalYear { get; set; }
        public int ResearchIssueId { get; set; }
        public decimal FundAmount { get; set; }
        public int InternalProfessorId { get; set; }
        public int InternalProfessor2Id { get; set; }
        public int ExternalProfessorId { get; set; }
        public int ProjectUploadId { get; set; }
        public int ProjectStatusId { get; set; }
        public DateTime StartContractDate { get; set; }
        public DateTime EndContractDate { get; set; }
        public int FiscalScheduleId { get; set; }
        public string LastUpdateBy { get; set; }
        public virtual ProjectStatus ProjectStatus { get { return (ProjectStatus)ProjectStatusId; } }
        public virtual ICollection<ResearcherModel> Researchers { get; set; }
    }
}