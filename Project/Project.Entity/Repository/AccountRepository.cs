using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Project.Entity;
using Project.Entity.Context;
using Dapper;


namespace Project.Entity.Repository
{

    /// <summary>
    /// Account repository class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AccountRepository : IRepository<Account>
    {

        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        private ProjectContext _context;
        /// <summary>
        /// Connection and Transaction is per created unitofwork instance
        /// </summary>
        /// <param name="dbProvider"></param>
        public AccountRepository(ProjectContext dbProvider)
        {
            _connection = dbProvider.Connection;
            if (_connection.State == ConnectionState.Closed) _connection.Open();
            _transaction = dbProvider.Transaction;
            _context = dbProvider;
        }

        public IDbConnection Connection { get => _connection; }
        public IDbTransaction Transaction { get => _transaction; set { _transaction = value; _context.Transaction = value; } }

        public IEnumerable<Account> GetAll()
        {
            string sQuery = "SELECT * FROM Accounts";
            return _connection.Query<Account>(sQuery);
            //example= how to use sp
            //string readSp = "GetAllAccounts";
            //return conn.Query<Account>(readSp, commandType: CommandType.StoredProcedure).ToList();
        }

        public IEnumerable<Account> Get(Func<Account, bool> predicate)
        {
            string sQuery = "SELECT * FROM Accounts";
            return _connection.Query<Account>(sQuery, null).Where(predicate);
        }

        public Account GetOne(object id)
        {
            string sQuery = "SELECT * FROM Accounts WHERE Id = @Id";
            return _connection.Query<Account>(sQuery, new { Id = id }).FirstOrDefault();
        }
        public int Insert(Account entity)
        {
            int retval = -1;
            if (entity != null)
            {
                string sQuery = "INSERT INTO Accounts (Name, Email, Description, IsTrial, IsActive, SetActive, Created, Modified )"
                                             + " VALUES(@Name, @Email, @Description, @IsTrial, @IsActive, @SetActive, GETUTCDATE(), GETUTCDATE());";
                sQuery += "SELECT CAST(SCOPE_IDENTITY() as int); ";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                retval = _connection.Query<int>(sQuery, entity, _transaction).Single();
            }
            return retval;
        }
        public void Update(object id, Account entity)
        {
            if (entity != null)
            {
                string sQuery = "UPDATE Accounts SET Name=@Name, Email=@Email, Description=@Description, IsTrial=@IsTrial, IsActive=@IsActive,SetActive=@SetActive, Modified=GETUTCDATE()"
                                    + " WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                _connection.Query(sQuery, entity, _transaction);
            }
        }
        public void Delete(object id)
        {
            string sQuery = "DELETE FROM Accounts"
                                + " WHERE Id = @Id";
            if (_transaction == null) Transaction = _connection.BeginTransaction();
            _connection.Execute(sQuery, new { Id = id }, _transaction);
        }
        public void Delete(Account entity)
        {
            if (entity != null)
            {
                var id = entity.Id;
                string sQuery = "DELETE FROM Accounts"
                                    + " WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                _connection.Execute(sQuery, new { Id = id }, _transaction);
            }
        }

    }


}
