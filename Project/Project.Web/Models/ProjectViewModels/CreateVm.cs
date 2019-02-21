using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Web.Models.ProjectViewModels
{
    public class CreateVm
    {
        public CreateVm()
        {
            this.AvailableFiscalSchedules = new List<SelectListItem>();
            this.AvailableProfessors = new List<SelectListItem>();
            this.AvailableResearchIssues = new List<SelectListItem>();
        }

        [Display(Name = "ชื่อโครงการวิจัย(ไทย)")]
        public string ProjectNameTh { get; set; }

        [Display(Name = "ประเด็นการวิจัย")]
        public int ResearchIssueId { get; set; }

        public IList<SelectListItem> AvailableResearchIssues { get; set; }

        [Display(Name = "งบประมาณ")]
        public decimal FundAmount { get; set; }

        [Display(Name = "ผู้ทรงคุณวุฒิ(ภายใน)")]
        public int InternalProfessorId { get; set; }

        [Display(Name = "ผู้ทรงคุณวุฒิ(ภายใน)")]
        public int InternalProfessor2Id { get; set; }

        [Display(Name = "ผู้ทรงคุณวุฒิ(ภายนอก)")]
        public int ExternalProfessorId { get; set; }

        public IList<SelectListItem> AvailableProfessors { get; set; }

        [Display(Name = "ข้อเสนอโครงการวิจัย")]
        public int ProposalUploadId { get; set; }

        [Display(Name = "ปีงบประมาณ")]
        public int FiscalScheduleId { get; set; }

        public IList<SelectListItem> AvailableFiscalSchedules { get; set; }
        public virtual ICollection<ResearcherViewModel> Researchers { get; set; }
    }
}