﻿using System;
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
        //private readonly IRepository<ProjectProgress> _projectProgressRepository;
        private readonly IRepository<ProjectResearcher> _projectResearcherRepository;
        private readonly IRepository<ProjectProfessor> _projectProfessorRepository;
        private readonly string _entityName;

        public ProjectService(IRepository<Project> projectRepository,
                              IRepository<AcademicRank> academicRankRepository,
                              IRepository<ProjectResearcher> projectResearcherRepository,
                              IRepository<ProjectProfessor> projectProfessorRepository)
        {
            this._projectRepository = projectRepository;
            this._academicRankrepository = academicRankRepository;
            this._projectResearcherRepository = projectResearcherRepository;
            this._projectProfessorRepository = projectProfessorRepository;
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

        public void RemoveProjectResearcher(Project project, ProjectResearcher projectResearcher)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            if (projectResearcher == null)
                throw new ArgumentNullException(nameof(projectResearcher));

            project.ProjectResearchers.Remove(projectResearcher);
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
    }
}
