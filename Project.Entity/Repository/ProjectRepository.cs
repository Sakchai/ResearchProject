using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Project.Entity;
using Project.Entity.Context;
using Dapper;


namespace Project.Entity.Repository
{

    /// <summary>
    /// Project repository class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProjectRepository : IProjectRepository
    {

        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        private ProjectContext _context;
        public ProjectRepository(ProjectContext dbProvider)
        {
            _connection = dbProvider.Connection;
            if (_connection.State == ConnectionState.Closed) _connection.Open();
            _transaction = dbProvider.Transaction;
            _context = dbProvider;
        }

        public IDbConnection Connection { get => _connection; }
        public IDbTransaction Transaction { get => _transaction; set { _transaction = value; _context.Transaction = value; } }

        public IEnumerable<Project> GetAll()
        {
            string sQuery = "SELECT * FROM Projects";
            return _connection.Query<Project>(sQuery);
        }

        public IEnumerable<Project> Get(Func<Project, bool> predicate)
        {
            string sQuery = "SELECT * FROM Projects";
            return _connection.Query<Project>(sQuery, null).Where(predicate);
        }

        public Project GetOne(object id)
        {
            string sQuery = "SELECT * FROM Projects WHERE Id = @Id";
            return _connection.Query<Project>(sQuery, new { Id = id }).FirstOrDefault();
        }

        public int Insert(Project entity)
        {
            int retval = -1;
            if (entity != null)
            {
                string sQuery = @"declare 
                                    @NextProjectCode varchar(10);
                                  begin
                                     select @NextProjectCode = 'pp-' + substring(cast(cast(max(substring(ProjectCode,4,6)) as int) + 1000001 as varchar(7)),2,6) 
                                     from Projects where ProjectStatusId=1;
                                    INSERT INTO Projects(ProjectCode, ProjectNameTH, ProjectNameEN, FiscalYear, ResearchIssueId,FundAmount 
                                  , InternalProfessorId, InternalProfessor2Id, ExternalProfessorId, ProposalUploadId, ProjectStatusId, StartContractDate
                                  , EndContractDate, Published, Utilization, AbstractTitle, CompletedUploadId, FiscalScheduleId, Created, LastUpdateBy)
                                  VALUES(@NextProjectCode, @ProjectNameTH, @ProjectNameEN, @FiscalYear, @ResearchIssueId, @FundAmount, @InternalProfessorId
                                  , @InternalProfessor2Id, @ExternalProfessorId, @ProposalUploadId, @ProjectStatusId, @StartContractDate, @EndContractDate
                                  , @Published, @Utilization, @AbstractTitle, @CompletedUploadId, @FiscalScheduleId, GETUTCDATE(), @LastUpdateBy);
                                  SELECT CAST(SCOPE_IDENTITY() as int); 
                                 end ";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                retval = _connection.Query<int>(sQuery, entity, _transaction).Single();
            }
            return retval;
        }

        public void Update(object id, Project entity)
        {
            if (entity != null)
            {

                string sQuery = @"UPDATE Projects
                               SET ProjectCode = @ProjectCode
                                  ,ProjectNameTH = @ProjectNameTH
                                  ,ProjectNameEN = @ProjectNameEN
                                  ,FiscalYear = @FiscalYear
                                  ,ResearchIssueId = @ResearchIssueId
                                  ,FundAmount = @FundAmount
                                  ,InternalProfessorId = @InternalProfessorId
                                  ,InternalProfessor2Id = @InternalProfessor2Id
                                  ,ExternalProfessorId = @ExternalProfessorId
                                  ,ProposalUploadId = @ProposalUploadId
                                  ,ProjectStatusId = @ProjectStatusId
                                  ,StartContractDate = @StartContractDate
                                  ,EndContractDate = @EndContractDate
                                  ,Published = @Published
                                  ,Utilization = @Utilization
                                  ,AbstractTitle = @AbstractTitle
                                  ,CompletedUploadId = @CompletedUploadId
                                  ,FiscalScheduleId = @FiscalScheduleId
                                  ,Modified = GETUTCDATE()
                                  ,LastUpdateBy = @LastUpdateBy
                             WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                _connection.Query(sQuery, entity, _transaction);
            }
        }
        public void Delete(object id)
        {
            string sQuery = "DELETE FROM Projects"
                                + " WHERE Id = @Id";
            if (_transaction == null) Transaction = _connection.BeginTransaction();
            _connection.Execute(sQuery, new { Id = id }, _transaction);
        }
        public void Delete(Project entity)
        {
            if (entity != null)
            {
                var id = entity.Id;
                string sQuery = "DELETE FROM Projects"
                                    + " WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                _connection.Execute(sQuery, new { Id = id }, _transaction);
            }
        }

    }


}
