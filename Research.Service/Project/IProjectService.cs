using Research.Core;
using Research.Data;

namespace Research.Services.Projects
{
    public partial interface IProjectService
    {
        /// <summary>
        /// Gets all projects
        /// </summary>
        /// <param name="username">Username; null to load all projects</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="getOnlyTotalCount">A value in indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
        /// <returns>Projects</returns>
        IPagedList<Project> GetAllProjects(string email = null, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

        /// <summary>
        /// Delete a Project
        /// </summary>
        /// <param name="project">Project</param>
        void DeleteProject(Project project);

        /// <summary>
        /// Gets a Project
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <returns>A Project</returns>
        Project GetProjectById(int projectId);
        void UpdateProject(Project project);
        void RemoveProjectResearcher(Project project, ProjectResearcher projectResearcher);
        void InsertProjectResearcher(ProjectResearcher projectResearcher);
        void InsertProject(Project project);
        IPagedList<ProjectResearcher> GetAllProjectResearchers(int projectId, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

        void RemoveProjectProfessor(Project project, ProjectProfessor projectProfessor);
        void InsertProjectProfessor(ProjectProfessor projectProfessor);
        IPagedList<ProjectProfessor> GetAllProjectProfessors(int projectId, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

        void RemoveProjectProgress(Project project, ProjectProgress projectProgress);
        void InsertProjectProgress(ProjectProgress projectProgress);
        IPagedList<ProjectProgress> GetAllProjectProgresses(int projectId, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

    }
}
