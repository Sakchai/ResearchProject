using System;
using Research.Core;
using Research.Data;
using System.Linq;
using Research.Core.Data;

namespace Research.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<AcademicRank> _academicRankrepository;
        //private readonly IRepository<ProjectHistory> _projectHistoryRepository;
        //private readonly IRepository<ProjectProgress> _projectProgressRepository;
        //private readonly IRepository<ProjectResearcher> _projectResearcherRepository;
        private readonly string _entityName;

        public ProjectService(IRepository<Project> projectRepository,
                              IRepository<AcademicRank> academicRankRepository)
        {
            this._projectRepository = projectRepository;
            this._academicRankrepository = academicRankRepository;
            this._entityName = typeof(Project).Name;
        }
        public void DeleteProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            project.Deleted = true;

            UpdateProject(project);
        }

        /// <summary>
        /// Updates the project
        /// </summary>
        /// <param name="project">Project</param>
        public virtual void UpdateProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            _projectRepository.Update(project);
        }

        public IPagedList<Project> GetAllProjects(string email = null, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            //var query2 = _academicRankrepository.Table;
            //var academicRanks = new PagedList<AcademicRank>(query2, pageIndex, pageSize, getOnlyTotalCount);
            var query = _projectRepository.Table;
            //query = query.OrderByDescending(p => p.FiscalYear);

            var projects = new PagedList<Project>(query, pageIndex, pageSize, getOnlyTotalCount);
            return projects;
        }

        public Project GetProjectById(int projectId)
        {
            if (projectId == 0)
                return null;

            return _projectRepository.GetById(projectId);
        }
    }
}
