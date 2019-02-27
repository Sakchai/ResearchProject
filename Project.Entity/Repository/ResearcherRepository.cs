using Dapper;
using Project.Entity.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Project.Entity.Repository
{
    /// <summary>
    /// Researcher repository class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResearcherRepository : IResearcherRepository
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        private ProjectContext _context;

        public ResearcherRepository(ProjectContext dbProvider)
        {
            _connection = dbProvider.Connection;
            if (_connection.State == ConnectionState.Closed) _connection.Open();
            _transaction = dbProvider.Transaction;
            _context = dbProvider;
        }

        public IDbConnection Connection { get => _connection; }
        public IDbTransaction Transaction { get => _transaction; set { _transaction = value; _context.Transaction = value; } }

        public IEnumerable<Researcher> GetAll()
        {
            string sQuery = "SELECT * FROM Researchers";
            return _connection.Query<Researcher>(sQuery);
        }

        public IEnumerable<Researcher> Get(Func<Researcher, bool> predicate)
        {
            string sQuery = "SELECT * FROM Researchers";
            return _connection.Query<Researcher>(sQuery, null).Where(predicate);
        }

        public Researcher GetOne(object id)
        {
            string sQuery = "SELECT * FROM Researchers WHERE Id = @Id";
            return _connection.Query<Researcher>(sQuery, new { Id = id }).FirstOrDefault();
        }

        public int Insert(Researcher entity)
        {
            int retval = -1;
            if (entity != null)
            {
                string sQuery = @"INSERT INTO Researcheres
                                       (TitleId,FirstName,LastName,Sex,IDCard,ResearcherCode,PersonType,Email,IsActive
                                       ,IsAcceptedConditions,PhoneNumber,PictureId,FacultyId,Created,LastUpdateBy)
                                 VALUES (@TitleId,@FirstName,@LastName,@Sex,@IDCard,@ResearcherCode,@PersonType,@Email,1
                                       ,0,@PhoneNumber,@PictureId,@FacultyId,GETUTCDATE(),@LastUpdateBy)";
                sQuery += "SELECT CAST(SCOPE_IDENTITY() as int); ";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                retval = _connection.Query<int>(sQuery, entity, _transaction).Single();
            }
            return retval;
        }

        public void Update(object id, Researcher entity)
        {
            if (entity != null)
            {
                string sQuery = @"UPDATE Researcheres
                                   SET TitleId = @TitleId
                                      ,FirstName = @FirstName
                                      ,LastName = @LastName
                                      ,Sex = @Sex
                                      ,IDCard = @IDCard
                                      ,ResearcherCode = @ResearcherCode
                                      ,PersonType = @PersonType
                                      ,Email = @Email
                                      ,IsActive = @IsActive
                                      ,IsAcceptedConditions = @IsAcceptedConditions
                                      ,PhoneNumber = @PhoneNumber
                                      ,PictureId = @PictureId
                                      ,FacultyId = @FacultyId
                                      ,LastUpdateBy = @LastUpdateBy
                                      ,Modified = GETUTCDATE()
                                 WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                _connection.Query(sQuery, entity, _transaction);
            }
        }

        public void Delete(object id)
        {
            string sQuery = "DELETE FROM Researchers"
                                + " WHERE Id = @Id";
            if (_transaction == null) Transaction = _connection.BeginTransaction();
            _connection.Execute(sQuery, new { Id = id }, _transaction);
        }

        public void Delete(Researcher entity)
        {
            if (entity != null)
            {
                var id = entity.Id;
                string sQuery = "DELETE FROM Researchers"
                                    + " WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                _connection.Execute(sQuery, new { Id = id }, _transaction);
            }
        }

        public IEnumerable<Researcher> GetResearchersByProjectId(int projectid)
        {
            string sQuery = "SELECT Researchers.* FROM Researchers join ProjectResearchers on  Researchers.Id = ProjectResearchers.ResearcherId"
                                    + " WHERE ProjectResearchers.ProjectId = @projectid";
            return _connection.Query<Researcher>(sQuery, new { ProjectId = projectid });
        }

        public Researcher GetResearcherUserByEmail(string email)
        {
            string sQuery = "SELECT Researchers.* FROM Researchers join Users on Researchers.Id = Users.ResearcherId" 
                            + " WHERE Users.Email = @Email";
            return _connection.Query<Researcher>(sQuery, new { Email = email}).FirstOrDefault();
        }

    }
}