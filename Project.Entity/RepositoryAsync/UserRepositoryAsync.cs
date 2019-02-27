using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Project.Entity;
using Project.Entity.Context;
using Dapper;
using System.Threading.Tasks;


namespace Project.Entity.Repository
{

    /// <summary>
    /// User repository class async
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UserRepositoryAsync : IRepositoryAsync<User>
    {

        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        private ProjectContext _context;
        public UserRepositoryAsync(ProjectContext dbProvider)
        {
            _connection = dbProvider.Connection;
            if (_connection.State == ConnectionState.Closed) _connection.Open();
            _transaction = dbProvider.Transaction;
            _context = dbProvider;
        }

        public IDbConnection Connection { get => _connection; }
        public IDbTransaction Transaction { get => _transaction; set { _transaction = value; _context.Transaction = value; } }

        public async Task<IEnumerable<User>> GetAll()
        {
            string sQuery = "SELECT * FROM Users";
            return await _connection.QueryAsync<User>(sQuery);
        }

        public async Task<IEnumerable<User>> Get(Func<User, bool> predicate)
        {
            string sQuery = "SELECT * FROM Users WHERE Id = @Id";
            IEnumerable<User> users = await _connection.QueryAsync<User>(sQuery, null);
            return users.Where(predicate);
        }

        public async Task<User> GetOne(object id)
        {
            string sQuery = "SELECT * FROM Users WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<User>(sQuery, new { Id = id });
        }
        public async Task<int> Insert(User entity)
        {
            int retval = -1;
            if (entity != null)
            {
                string sQuery = "INSERT INTO Users (FirstName, LastName, UserName, Email, Description, IsAdminRole, Roles, IsActive, Password, AccountId, Created, Modified )"
                                             + " VALUES(@FirstName, @LastName, @UserName, @Email, @Description, @IsAdminRole, @Roles, @IsActive, @Password, @AccountId, GETUTCDATE(), GETUTCDATE())";
                sQuery += "SELECT CAST(SCOPE_IDENTITY() as int); ";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                IEnumerable<int> retvals = await _connection.QueryAsync<int>(sQuery, entity, _transaction);
                retval = retvals.Single();
            }
            return retval;
        }
        public async Task Update(object id, User entity)
        {
            if (entity != null)
            {
                string sQuery = "UPDATE Users SET FirstName=@FirstName, LastName=@LastName, UserName=@UserName, Description=@Description, IsAdminRole=@IsAdminRole, Roles = @Roles, IsActive=@IsActive,Password=@Password,AccountId =@AccountId, Modified=GETUTCDATE() "
                                    + " WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                await _connection.QueryAsync(sQuery, entity, _transaction);
            }
        }
        public async Task Delete(object id)
        {
            string sQuery = "DELETE FROM Users"
                                + " WHERE Id = @Id";
            if (_transaction == null) Transaction = _connection.BeginTransaction();
            await _connection.ExecuteAsync(sQuery, new { Id = id }, _transaction);
        }
        public async Task Delete(User entity)
        {
            if (entity != null)
            {
                var id = entity.Id;
                string sQuery = "DELETE FROM Users"
                                   + " WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                await _connection.ExecuteAsync(sQuery, new { Id = id }, _transaction);
            }
        }


    }


}
