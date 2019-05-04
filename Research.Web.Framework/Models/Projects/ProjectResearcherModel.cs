
using Research.Data;
using Research.Enum;
using Research.Web.Framework.Models;

namespace Research.Web.Models.Projects
{
    public class ProjectResearcherModel : BaseEntityModel
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
