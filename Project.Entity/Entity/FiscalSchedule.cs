using System;
using System.Collections.Generic;

namespace Project.Entity
{
    public partial class FiscalSchedule : BaseEntity
    {
        public FiscalSchedule()
        {
            Project = new HashSet<Project>();
        }

        public string FiscalCode { get; set; }
        public int FiscalYear { get; set; }
        public int FiscalTimes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LastUpdateBy { get; set; }

        public virtual ICollection<Project> Project { get; set; }
    }
}