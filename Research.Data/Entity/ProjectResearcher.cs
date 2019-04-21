using Research.Enum;
using System;
using System.Collections.Generic;

namespace Research.Data
{
    public partial class ProjectResearcher : BaseEntity
    {
        public int ProjectId { get; set; }
        public int ResearcherId { get; set; }
        public int Portion { get; set; }
        public int? ProjectRoleId { get; set; }
        public int? TitleId { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDCard { get; set; }
        public ProjectRole ProjectRole 
        {
            get => (ProjectRole)ProjectRoleId;
            set => ProjectRoleId = (int)value;
        }
        public virtual Project Project { get; set; }
        public virtual Researcher Researcher { get; set; }
        public virtual Title Title { get; set; }
        public virtual string ResearcherName { get { return $"{FirstName} {LastName}";  }}
    }


}