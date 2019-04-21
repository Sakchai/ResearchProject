using Research.Domain;
using Research.Web.Models.Projects;

namespace Research.Web.Factories
{
    public interface IProjectModelFactory
    {
        CreateVm PrepareProjectCreateModel(CreateVm model);
        ModifyVm PrepareProjectEditModel(ModifyVm model, Research.Data.Project project);
        ProjectSearchModel PrepareProjectSearchModel(ProjectSearchModel searchModel);
        ProjectListModel PrepareProjectListModel(ProjectSearchModel searchModel);
    }
}