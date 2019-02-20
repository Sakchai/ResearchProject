using Project.Enum;
using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class ProjectProgress : BaseEntity
    {
        public int ProjectId { get; set; }
        public DateTime ProgressStartDate { get; set; }
        public DateTime ProgressEndDate { get; set; }
        public string Note { get; set; }
        public int? ResearchStatusId { get; set; }
        public string LastUpdateBy { get; set; }
        public virtual Project Project { get; set; }
        public virtual ResearchStatus ResearchStatus { get { return (ResearchStatus)ResearchStatusId; } }
    }


}