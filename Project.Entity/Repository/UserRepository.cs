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
    /// User repository class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UserRepository : IUserRepository
    {

        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        private ProjectContext _context;
        public UserRepository(ProjectContext dbProvider)
        {
            _connection = dbProvider.Connection;
            if (_connection.State == ConnectionState.Closed) _connection.Open();
            _transaction = dbProvider.Transaction;
            _context = dbProvider;
        }

        public IDbConnection Connection { get => _connection; }
        public IDbTransaction Transaction { get => _transaction; set { _transaction = value; _context.Transaction = value; } }

        public IEnumerable<User> GetAll()
        {
            string sQuery = "SELECT * FROM Users";
            return _connection.Query<User>(sQuery);
        }

        public IEnumerable<User> Get(Func<User, bool> predicate)
        {
            string sQuery = "SELECT * FROM Users";
            return _connection.Query<User>(sQuery, null).Where(predicate);
        }

        public User GetOne(object id)
        {
            string sQuery = "SELECT * FROM Users WHERE Id = @Id";
            return _connection.Query<User>(sQuery, new { Id = id }).FirstOrDefault();
        }
        public int Insert(User entity)
        {
            int retval = -1;
            if (entity != null)
            {
                string sQuery = "INSERT INTO Users (FirstName, LastName, UserName, Email, Description, IsAdminRole, Roles, IsActive, Password, AccountId, Created, Modified )"
                                             + " VALUES(@FirstName, @LastName, @UserName, @Email, @Description, @IsAdminRole, @Roles, @IsActive, @Password, @AccountId, GETUTCDATE(), GETUTCDATE())";
                sQuery += "SELECT CAST(SCOPE_IDENTITY() as int); ";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                retval = _connection.Query<int>(sQuery, entity, _transaction).Single();
            }
            return retval;
        }

        public void Update(object id, User entity)
        {
            if (entity != null)
            {
                string sQuery = "UPDATE Users SET FirstName=@FirstName, LastName=@LastName, UserName=@UserName, Description=@Description, IsAdminRole=@IsAdminRole, Roles = @Roles, IsActive=@IsActive,Password=@Password,AccountId =@AccountId, Modified=GETUTCDATE() "
                                    + " WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                _connection.Query(sQuery, entity, _transaction);
            }
        }
        public void Delete(object id)
        {
            string sQuery = "DELETE FROM Users"
                                + " WHERE Id = @Id";
            if (_transaction == null) Transaction = _connection.BeginTransaction();
            _connection.Execute(sQuery, new { Id = id }, _transaction);
        }
        public void Delete(User entity)
        {
            if (entity != null)
            {
                var id = entity.Id;
                string sQuery = "DELETE FROM Users"
                                    + " WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                _connection.Execute(sQuery, new { Id = id }, _transaction);
            }
        }


    }


}
