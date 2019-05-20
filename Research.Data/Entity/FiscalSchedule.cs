using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class FiscalSchedule : BaseEntity
    {
        //private ICollection<Project> _projects;
        //public FiscalSchedule()
        //{
        //}

        public string FiscalCode { get; set; }
        public int FiscalYear { get; set; }
        public int FiscalTimes { get; set; }
        public string ScholarName { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public bool Deleted { get; set; }
        public string LastUpdateBy { get; set; }

        public DateTime Created { get; set; }

        //public virtual ICollection<Project> Projects {
        //    get => _projects ?? (_projects = new List<Project>());
        //    set => _projects = value;
        //}
    }
}