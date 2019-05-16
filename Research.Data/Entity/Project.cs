using Research.Enum;
using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class Project : BaseEntity
    {
        private ICollection<ProjectHistory> _projectHistories;
        private ICollection<ProjectProgress> _projectProgresses;
        private ICollection<ProjectResearcher> _projectResearchers;
        private ICollection<ProjectProfessor> _projectProfessors;


        public string ProjectCode { get; set; }
        public string ProjectNameTh { get; set; }
        public string ProjectNameEn { get; set; }
        public string PlanNameTh { get; set; }
        public string PlanNameEn { get; set; }
        public string ProjectType { get; set; }
        public int FiscalYear { get; set; }
        public int? ResearchIssueId { get; set; }
        public decimal FundAmount { get; set; }
        public int? DownloadId { get; set; }

        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public int? FiscalScheduleId { get; set; }
        public int? StrategyGroupId { get; set; }
        public string LastUpdateBy { get; set; }
        public string Comment { get; set; }
        public bool Deleted { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int? ProjectStatusId { get; set; }
        public ProjectStatus ProjectStatus {
            get => (ProjectStatus) ProjectStatusId;
            set => ProjectStatusId = (int)value;
        }
        public virtual FiscalSchedule FiscalSchedule { get; set; }
        public virtual StrategyGroup StrategyGroup { get; set; }
        public virtual ResearchIssue ResearchIssue { get; set; }
        public virtual ICollection<ProjectHistory> ProjectHistories
        {
            get => _projectHistories ?? (_projectHistories = new List<ProjectHistory>());
            set => _projectHistories = value;
        }
        public virtual ICollection<ProjectProgress> ProjectProgresses
        {
            get => _projectProgresses ?? (_projectProgresses = new List<ProjectProgress>());
            set => _projectProgresses = value;
        }
        public virtual ICollection<ProjectResearcher> ProjectResearchers
        {
            get => _projectResearchers ?? (_projectResearchers = new List<ProjectResearcher>());
            set => _projectResearchers = value;
        }

        public virtual ICollection<ProjectProfessor> ProjectProfessors
        {
            get => _projectProfessors ?? (_projectProfessors = new List<ProjectProfessor>());
            set => _projectProfessors = value;
        }
    }

}