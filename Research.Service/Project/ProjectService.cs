using System;
using Research.Core;
using Research.Data;
using System.Linq;
using Research.Core.Data;
using Research.Core.Caching;

namespace Research.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<AcademicRank> _academicRankrepository;
        //private readonly IRepository<ProjectHistory> _projectHistoryRepository;
        private readonly IRepository<ProjectProgress> _projectProgressRepository;
        private readonly IRepository<ProjectResearcher> _projectResearcherRepository;
        private readonly IRepository<ProjectProfessor> _projectProfessorRepository;
        private readonly string _entityName;

        public ProjectService(IRepository<Project> projectRepository,
                              IRepository<AcademicRank> academicRankRepository,
                              IRepository<ProjectResearcher> projectResearcherRepository,
                              IRepository<ProjectProfessor> projectProfessorRepository,
                              IRepository<ProjectProgress> projectProgressRepository)
        {
            this._projectRepository = projectRepository;
            this._academicRankrepository = academicRankRepository;
            this._projectResearcherRepository = projectResearcherRepository;
            this._projectProfessorRepository = projectProfessorRepository;
            this._projectProgressRepository = projectProgressRepository;
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

        public IPagedList<Project> GetAllProjects(string projectNameTH = null, int fiscalYear = 0, int projectStatusId = 0,
                            int agencyId = 0, int progressStatusId = 0, string createdBy = null,
                            int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var query = _projectRepository.Table;
            if (!string.IsNullOrEmpty(createdBy))
                query = query.Where(x => x.CreatedBy == createdBy);
            if (!string.IsNullOrEmpty(projectNameTH))
                query = query.Where(x => x.ProjectNameTh.Contains(projectNameTH));
            if (fiscalYear != 0)
                query = query.Where(x => x.FiscalYear == fiscalYear);
            if (projectStatusId != 0)
                query = query.Where(x => x.ProjectStatusId == projectStatusId);
            if (agencyId != 0)
            {
                query = query.Join(_projectResearcherRepository.Table, x => x.Id, y => y.ProjectId,
                        (x, y) => new { Project = x, Mappping = y })
                        .Where(z => agencyId == z.Mappping.Researcher.AgencyId)
                        .Select(z => z.Project)
                        .Distinct();
            }
            if (progressStatusId != 0)
            {
                query = query.Join(_projectProgressRepository.Table, x => x.Id, y => y.ProjectId,
                        (x, y) => new { Project = x, Mappping = y })
                        .Where(z => progressStatusId == z.Mappping.ProgressStatusId)
                        .Select(z => z.Project)
                        .Distinct();
            }
            query = query.OrderByDescending(x => x.ProjectEndDate);
            var projects = new PagedList<Project>(query, pageIndex, pageSize, getOnlyTotalCount);
            return projects;
        }

        public Project GetProjectById(int projectId)
        {
            if (projectId == 0)
                return null;

            return _projectRepository.GetById(projectId);
        }

        public void RemoveProjectResearcher(Project project, ProjectResearcher projectResearcher)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            if (projectResearcher == null)
                throw new ArgumentNullException(nameof(projectResearcher));

            //project.ProjectResearchers.Remove(projectResearcher);
            _projectResearcherRepository.Delete(projectResearcher);
        }

        public void InsertProjectResearcher(ProjectResearcher projectResearcher)
        {
            if (projectResearcher == null)
                throw new ArgumentNullException(nameof(projectResearcher));

            if (projectResearcher is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _projectResearcherRepository.Insert(projectResearcher);
        }

        public void InsertProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            if (project is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _projectRepository.Insert(project);
        }

        public IPagedList<ProjectResearcher> GetAllProjectResearchers(int projectId, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var query = _projectResearcherRepository.Table;

            query = query.Where(c => c.ProjectId == projectId);
            var projectResearcher = new PagedList<ProjectResearcher>(query, pageIndex, pageSize, getOnlyTotalCount);
            return projectResearcher;
        }

        public void RemoveProjectProfessor(Project project, ProjectProfessor projectProfessor)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            if (projectProfessor == null)
                throw new ArgumentNullException(nameof(projectProfessor));

            project.ProjectProfessors.Remove(projectProfessor);
        }

        public void InsertProjectProfessor(ProjectProfessor projectProfessor)
        {
            if (projectProfessor == null)
                throw new ArgumentNullException(nameof(projectProfessor));

            if (projectProfessor is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _projectProfessorRepository.Insert(projectProfessor);
        }

        public IPagedList<ProjectProfessor> GetAllProjectProfessors(int projectId, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var query = _projectProfessorRepository.Table;

            query = query.Where(c => c.ProjectId == projectId);
            var projectProfessor = new PagedList<ProjectProfessor>(query, pageIndex, pageSize, getOnlyTotalCount);
            return projectProfessor;
        }

        public void RemoveProjectProgress(Project project, ProjectProgress projectProgress)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            if (projectProgress == null)
                throw new ArgumentNullException(nameof(projectProgress));

            project.ProjectProgresses.Remove(projectProgress);
        }

        public void InsertProjectProgress(ProjectProgress projectProgress)
        {
            if (projectProgress == null)
                throw new ArgumentNullException(nameof(projectProgress));

            if (projectProgress is IEntityForCaching)
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

            _projectProgressRepository.Insert(projectProgress);
        }

        public IPagedList<ProjectProgress> GetAllProjectProgresses(int projectId, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var query = _projectProgressRepository.Table;

            query = query.Where(c => c.ProjectId == projectId);
            var projectProgress = new PagedList<ProjectProgress>(query, pageIndex, pageSize, getOnlyTotalCount);
            return projectProgress;
        }

        public ProjectResearcher GetProjectResearchersById(int projectResearcherId)
        {
            var query = _projectResearcherRepository.Table;

            return query.Where(c => c.Id == projectResearcherId).FirstOrDefault();

        }

        public string GetNextNumber()
        {
            var query = _projectRepository.Table;
            int maxNumber = query.LastOrDefault() != null ? query.LastOrDefault().Id : 0;
            //int? maxNumber = query.Max(e => (int?)e.Id);
            maxNumber += 1;
            return $"pp-{maxNumber.ToString("D6")}";
        }

        //public string ExportResourcesToPdf(ProjectProgress projectProgress)
        //{
        //    throw new NotImplementedException();
        //}

        //public void ImportResourcesFromPdf(ProjectProgress projectProgress, string pdf, bool updateExistingResources = true)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
