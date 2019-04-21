using Microsoft.AspNetCore.Mvc.Rendering;
using Research.Domain;
using Research.Web.Common;
using Research.Web.Models.Researchers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Research.Web.Models.Projects
{
    public class CreateVm : BaseSearchModel
    {
        public CreateVm()
        {
            this.AvailableFiscalSchedules = new List<SelectListItem>();
            this.AvailableProfessors = new List<SelectListItem>();
            this.AvailableResearchIssues = new List<SelectListItem>();
        }

        public int Id { get; set; }

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
        public virtual ICollection<ResearcherModel> Researchers { get; set; }
    }
}