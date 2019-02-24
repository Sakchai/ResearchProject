using Dapper;
using Project.Entity.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Project.Entity.Repository
{
    /// <summary>
    /// ProjectResearcher repository class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProjectResearcherRepository : IProjectResearcherRepository
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        private ProjectContext _context;

        public ProjectResearcherRepository(ProjectContext dbProvider)
        {
            _connection = dbProvider.Connection;
            if (_connection.State == ConnectionState.Closed) _connection.Open();
            _transaction = dbProvider.Transaction;
            _context = dbProvider;
        }

        public IDbConnection Connection { get => _connection; }
        public IDbTransaction Transaction { get => _transaction; set { _transaction = value; _context.Transaction = value; } }

        public IEnumerable<ProjectResearcher> GetAll()
        {
            string sQuery = "SELECT * FROM ProjectResearchers";
            return _connection.Query<ProjectResearcher>(sQuery);
        }

        public IEnumerable<ProjectResearcher> Get(Func<ProjectResearcher, bool> predicate)
        {
            string sQuery = "SELECT * FROM ProjectResearchers";
            return _connection.Query<ProjectResearcher>(sQuery, null).Where(predicate);
        }

        public int Insert(ProjectResearcher entity)
        {
            int retval = -1;
            if (entity != null)
            {
                string sQuery = @"INSERT INTO dbo.ProjectResearchers
                                       (ProjectId
                                       , ResearcherId
                                       , Portion
                                       , ProjectRole)
                                 VALUES
                                       (@ProjectId
                                       , @ResearcherId
                                       , @Portion
                                       , @ProjectRole)";
                sQuery += "SELECT CAST(SCOPE_IDENTITY() as int); ";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                retval = _connection.Query<int>(sQuery, entity, _transaction).Single();
            }
            return retval;
        }

        public void Update(object id, ProjectResearcher entity)
        {
            if (entity != null)
            {
                string sQuery = @"UPDATE ProjectResearchers
                                   SET Portion = @Portion
                                      ,ProjectRole = @ProjectRole
                                 WHERE ProjectId = @ProjectId
                                      and ResearcherId = @ResearcherId";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                _connection.Query(sQuery, entity, _transaction);
            }
        }

        public void Delete(object id)
        {
            string sQuery = "DELETE FROM ProjectResearchers"
                                + " WHERE Id = @Id";
            if (_transaction == null) Transaction = _connection.BeginTransaction();
            _connection.Execute(sQuery, new { Id = id }, _transaction);
        }

        public void Delete(ProjectResearcher entity)
        {
            if (entity != null)
            {
                var id = entity.Id;
                string sQuery = "DELETE FROM ProjectResearchers"
                                    + " WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                _connection.Execute(sQuery, new { Id = id }, _transaction);
            }
        }

        public ProjectResearcher GetProjectResearcher(int projectId, int researcherId)
        {
            string sQuery = "SELECT * FROM ProjectResearchers WHERE ProjectId = @ProjectId and ResearcherId = @ResearcherId";
            return _connection.Query<ProjectResearcher>(sQuery, new
            {
                ProjectId = projectId,
                ResearcherId = researcherId
            }).FirstOrDefault();
        }

        public IEnumerable<ProjectResearcher> GetProjectResearcherByProjectId(int projectid)
        {
            string sQuery = @"SELECT ProjectResearchers.*, Researcheres.FirstName + ' ' +  Researcheres.LastName as ResearcherName 
                            FROM ProjectResearchers join Researcheres on  Researcheres.Id = ProjectResearchers.ResearcherId
                            WHERE ProjectResearchers.ProjectId = @Projectid";
            return _connection.Query<ProjectResearcher>(sQuery, new { ProjectId = projectid });
        }

        public ProjectResearcher GetOne(object id)
        {
            throw new NotImplementedException();
        }
    }
}