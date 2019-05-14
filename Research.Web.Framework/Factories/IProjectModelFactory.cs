using Research.Data;
using Research.Domain;
using Research.Web.Models.Projects;

namespace Research.Web.Factories
{
    public partial interface IProjectModelFactory
    {
        ProjectModel PrepareProjectModel(ProjectModel model, Project project, bool excludeProperties = false);
        ProjectSearchModel PrepareProjectSearchModel(ProjectSearchModel searchModel);
        ProjectListModel PrepareProjectListModel(ProjectSearchModel searchModel);
        ProjectResearcherListModel PrepareProjectResearcherListModel(ProjectResearcherSearchModel searchModel, Project project);
        ProjectModel PrepareProjectModel(ProjectModel projectModel, Project project);
    }
}