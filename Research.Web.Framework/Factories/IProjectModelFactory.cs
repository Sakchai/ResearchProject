using Research.Data;
using Research.Domain;
using Research.Web.Models.Projects;

namespace Research.Web.Factories
{
    public partial interface IProjectModelFactory
    {
        ProjectModel PrepareProjectModel(ProjectModel projectModel, Project project);
        ProjectSearchModel PrepareProjectSearchModel(ProjectSearchModel searchModel);
        ProjectListModel PrepareProjectListModel(ProjectSearchModel searchModel);

        ProjectResearcherSearchModel PrepareProjectResearcherSearchModel(ProjectResearcherSearchModel searchModel, Project project);
        ProjectResearcherListModel PrepareProjectResearcherListModel(ProjectResearcherSearchModel searchModel, Project project);
        
        ProjectProfessorSearchModel PrepareProjectProfessorSearchModel(ProjectProfessorSearchModel searchModel, Project project);
        ProjectProfessorListModel PrepareProjectProfessorListModel(ProjectProfessorSearchModel searchModel, Project project);

        ProjectProgressSearchModel PrepareProjectProgressSearchModel(ProjectProgressSearchModel searchModel, Project project);
        ProjectProgressListModel PrepareProjectProgressListModel(ProjectProgressSearchModel searchModel, Project project);

    }
}