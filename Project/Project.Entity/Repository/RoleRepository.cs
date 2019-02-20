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
    /// Role repository class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RoleRepository : IRoleRepository
    {

        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        private ProjectContext _context;
        public RoleRepository(ProjectContext dbProvider)
        {
            _connection = dbProvider.Connection;
            if (_connection.State == ConnectionState.Closed) _connection.Open();
            _transaction = dbProvider.Transaction;
            _context = dbProvider;
        }

        public IDbConnection Connection { get => _connection; }
        public IDbTransaction Transaction { get => _transaction; set { _transaction = value; _context.Transaction = value; } }

        public IEnumerable<Role> GetAll()
        {
            string sQuery = "SELECT * FROM Roles";
            return _connection.Query<Role>(sQuery);
        }

        public IEnumerable<Role> Get(Func<Role, bool> predicate)
        {
            string sQuery = "SELECT * FROM Roles";
            return _connection.Query<Role>(sQuery, null).Where(predicate);
        }

        public Role GetOne(object id)
        {
            string sQuery = "SELECT * FROM Roles WHERE Id = @Id";
            return _connection.Query<Role>(sQuery, new { Id = id }).FirstOrDefault();
        }

        public IEnumerable<Role> GetUserRoles(object userId)
        {
            string sQuery = "SELECT Roles.* FROM Roles join UserRoles on Roles.Id = UserRoles.RoleId " +
                            " WHERE UserRoles.UserId = @Id";
            return _connection.Query<Role>(sQuery, new { Id = userId }).AsList();
        }
        public int Insert(Role entity)
        {
            int retval = -1;
            if (entity != null)
            {
                string sQuery = "INSERT INTO Roles (FirstName, LastName, RoleName, Email, Description, IsAdminRole, Roles, IsActive, Password, AccountId, Created, Modified )"
                                             + " VALUES(@FirstName, @LastName, @RoleName, @Email, @Description, @IsAdminRole, @Roles, @IsActive, @Password, @AccountId, GETUTCDATE(), GETUTCDATE())";
                sQuery += "SELECT CAST(SCOPE_IDENTITY() as int); ";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                retval = _connection.Query<int>(sQuery, entity, _transaction).Single();
            }
            return retval;
        }

        public void Update(object id, Role entity)
        {
            if (entity != null)
            {

                string sQuery = "UPDATE Roles SET Name=@Name, IsActive=@IsActive, LastUpdateBy=@LastUpdateBy, Modified=GETUTCDATE() "
                                    + " WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                _connection.Query(sQuery, entity, _transaction);
            }
        }
        public void Delete(object id)
        {
            string sQuery = "DELETE FROM Roles"
                                + " WHERE Id = @Id";
            if (_transaction == null) Transaction = _connection.BeginTransaction();
            _connection.Execute(sQuery, new { Id = id }, _transaction);
        }
        public void Delete(Role entity)
        {
            if (entity != null)
            {
                var id = entity.Id;
                string sQuery = "DELETE FROM Roles"
                                    + " WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                _connection.Execute(sQuery, new { Id = id }, _transaction);
            }
        }

    }


}
