using Project.Domain;
using Project.Web.Models.ProjectViewModels;

namespace Project.Web.Factories
{
    public interface IProjectModelFactory
    {
        CreateVm PrepareProjectCreateModel(CreateVm model);
        ModifyVm PrepareProjectEditModel(ModifyVm model, ProjectModel project);
        ProjectSearchModel PrepareProjectSearchModel(ProjectSearchModel searchModel);
    }
}