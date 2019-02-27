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
    /// Role repository class async
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RoleRepositoryAsync : IRepositoryAsync<Role>
    {

        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        private ProjectContext _context;
        public RoleRepositoryAsync(ProjectContext dbProvider)
        {
            _connection = dbProvider.Connection;
            if (_connection.State == ConnectionState.Closed) _connection.Open();
            _transaction = dbProvider.Transaction;
            _context = dbProvider;
        }

        public IDbConnection Connection { get => _connection; }
        public IDbTransaction Transaction { get => _transaction; set { _transaction = value; _context.Transaction = value; } }

        public async Task<IEnumerable<Role>> GetAll()
        {
            string sQuery = "SELECT * FROM Roles";
            return await _connection.QueryAsync<Role>(sQuery);
        }

        public async Task<IEnumerable<Role>> Get(Func<Role, bool> predicate)
        {
            string sQuery = "SELECT * FROM Roles WHERE Id = @Id";
            IEnumerable<Role> users = await _connection.QueryAsync<Role>(sQuery, null);
            return users.Where(predicate);
        }

        public async Task<Role> GetOne(object id)
        {
            string sQuery = "SELECT * FROM Roles WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Role>(sQuery, new { Id = id });
        }
        public async Task<int> Insert(Role entity)
        {
            int retval = -1;
            if (entity != null)
            {
                string sQuery = "INSERT INTO Roles ( Name, Created, Modified )"
                                             + " VALUES(@Name, GETUTCDATE(), GETUTCDATE())";
                sQuery += "SELECT CAST(SCOPE_IDENTITY() as int); ";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                IEnumerable<int> retvals = await _connection.QueryAsync<int>(sQuery, entity, _transaction);
                retval = retvals.Single();
            }
            return retval;
        }
        public async Task Update(object id, Role entity)
        {
            if (entity != null)
            {
                string sQuery = "UPDATE Roles SET Name=@Name, Modified=GETUTCDATE() "
                                    + " WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                await _connection.QueryAsync(sQuery, entity, _transaction);
            }
        }
        public async Task Delete(object id)
        {
            string sQuery = "DELETE FROM Roles"
                                + " WHERE Id = @Id";
            if (_transaction == null) Transaction = _connection.BeginTransaction();
            await _connection.ExecuteAsync(sQuery, new { Id = id }, _transaction);
        }
        public async Task Delete(Role entity)
        {
            if (entity != null)
            {
                var id = entity.Id;
                string sQuery = "DELETE FROM Roles"
                                   + " WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                await _connection.ExecuteAsync(sQuery, new { Id = id }, _transaction);
            }
        }


    }


}
