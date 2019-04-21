
using Research.Data;
using Research.Enum;

namespace Research.Web.Models.Projects
{
    public class ProjectResearcherViewModel : BaseDomain
    {
        public int ProjectId { get; set; }
        public int ResearcherId { get; set; }
        public int Portion { get; set; }
        public int ProjectRoleId { get; set; }
        public string ResearcherName { get; set; }

        public virtual ProjectRole ProjectRole {
            get => (ProjectRole)ProjectRoleId;
            set => ProjectRoleId = (int)value;
        }
    }
}
