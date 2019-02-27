using Project.Enum;
using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class Project : BaseEntity
    {
        public Project()
        {
            ProjectHistory = new HashSet<ProjectHistory>();
            ProjectKeyword = new HashSet<ProjectKeyword>();
            ProjectProgress = new HashSet<ProjectProgress>();
            ProjectResearcher = new HashSet<ProjectResearcher>();
        }

        public string ProjectCode { get; set; }
        public string ProjectNameTh { get; set; }
        public string ProjectNameEn { get; set; }
        public int FiscalYear { get; set; }
        public int ResearchIssueId { get; set; }
        public decimal FundAmount { get; set; }
        public int? InternalProfessorId { get; set; }
        public int? InternalProfessor2Id { get; set; }
        public int? ExternalProfessorId { get; set; }
        public int? ProposalUploadId { get; set; }
        public int? ProjectStatusId { get; set; }
        public DateTime? StartContractDate { get; set; }
        public DateTime? EndContractDate { get; set; }
        public string Published { get; set; }
        public string Utilization { get; set; }
        public string AbstractTitle { get; set; }
        public int? CompletedUploadId { get; set; }
        public int? FiscalScheduleId { get; set; }
        public string LastUpdateBy { get; set; }
        public virtual ProjectStatus ProjectStatus { get { return (ProjectStatus)ProjectStatusId; } }  
        public virtual Download CompletedUpload { get; set; }
        public virtual FiscalSchedule FiscalSchedule { get; set; }
        public virtual Download ProposalUpload { get; set; }
        public virtual ResearchIssue ResearchIssue { get; set; }
        public virtual ICollection<ProjectHistory> ProjectHistory { get; set; }
        public virtual ICollection<ProjectKeyword> ProjectKeyword { get; set; }
        public virtual ICollection<ProjectProgress> ProjectProgress { get; set; }
        public virtual ICollection<ProjectResearcher> ProjectResearcher { get; set; }
    }

}