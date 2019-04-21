using Research.Enum;
using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class Title : BaseEntity
    {
        //ICollection<Researcher> _researchers;
        //ICollection<ResearcherHistory> _researcherHistories;
        //ICollection<ProjectResearcher> _projectResearchers;
        //ICollection<Professor> _professors;
        //ICollection<ProjectHistory> _projectHistories;
        //public Title()
        //{
        //}

        public string TitleNameTH { get; set; }
        public string TitleNameEN { get; set; }
        public int? GenderId { get; set; }
        public  Gender Gender {
            get => (Gender)GenderId;
            set => GenderId = (int)value;
        }
        //public virtual ICollection<Researcher> Researchers {
        //    get => _researchers ?? (_researchers = new List<Researcher>());
        //    set => _researchers = value;
        //}
        //public virtual ICollection<ResearcherHistory> ResearcherHistories {
        //    get => _researcherHistories ?? (_researcherHistories = new List<ResearcherHistory>());
        //    set => _researcherHistories = value;
        //}
        //public virtual ICollection<ProjectResearcher> ProjectResearchers {
        //    get => _projectResearchers ?? (_projectResearchers = new List<ProjectResearcher>());
        //    set => _projectResearchers = value;
        //}

        //public virtual ICollection<Professor> Professors {
        //    get => _professors ?? (_professors = new List<Professor>());
        //    set => _professors = value;
        //}
        //public virtual ICollection<ProjectHistory> ProjectHistories
        //{
        //    get => _projectHistories ?? (_projectHistories = new List<ProjectHistory>());
        //    set => _projectHistories = value;
        //}
    }
}