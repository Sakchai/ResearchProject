using Research.Data;
using Research.Domain;
using Research.Web.Models.Projects;

namespace Research.Web.Factories
{
    public interface IProjectModelFactory
    {
        ProjectModel PrepareProjectModel(ProjectModel model, Project project);
        ProjectSearchModel PrepareProjectSearchModel(ProjectSearchModel searchModel);
        ProjectListModel PrepareProjectListModel(ProjectSearchModel searchModel);
    }
}