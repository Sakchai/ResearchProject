using Project.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Domain;

namespace Project.Web.Models.ProjectViewModels
{
    public class ModifyVm
    {
        public ModifyVm()
        {
            this.AvailableFiscalSchedules = new List<SelectListItem>();
            this.AvailableProfessors = new List<SelectListItem>();
            this.AvailableProjectStatuses = new List<SelectListItem>();
            this.AvailableResearchIssues = new List<SelectListItem>();
        }
        public string ProjectCode { get; set; }
        public string ProjectNameTh { get; set; }
        public int FiscalYear { get; set; }
        public int ResearchIssueId { get; set; }
        public IList<SelectListItem> AvailableResearchIssues { get; set; }
        public decimal FundAmount { get; set; }
        public int InternalProfessorId { get; set; }
        public int InternalProfessor2Id { get; set; }
        public int ExternalProfessorId { get; set; }

        public IList<SelectListItem> AvailableProfessors { get; set; }
        public int ProposalUploadId { get; set; }
        public int ProjectStatusId { get; set; }
        public IList<SelectListItem> AvailableProjectStatuses { get; set; }
        public DateTime StartContractDate { get; set; }
        public DateTime EndContractDate { get; set; }
        public string Published { get; set; }
        public string Utilization { get; set; }
        public string AbstractTitle { get; set; }
        public int CompletedUploadId { get; set; }
        public int FiscalScheduleId { get; set; }
        public IList<SelectListItem> AvailableFiscalSchedules { get; set; }
        public virtual ICollection<ResearcherViewModel> Researchers { get; set; }
    }
}
