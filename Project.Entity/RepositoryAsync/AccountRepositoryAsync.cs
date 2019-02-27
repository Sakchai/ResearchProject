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
    /// General repository class async
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AccountRepositoryAsync : IRepositoryAsync<Account>
    {

        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        private ProjectContext _context;
        public AccountRepositoryAsync(ProjectContext dbProvider)
        {
            _connection = dbProvider.Connection;
            if (_connection.State == ConnectionState.Closed) _connection.Open();
            _transaction = dbProvider.Transaction;
            _context = dbProvider;
        }

        public IDbConnection Connection { get => _connection; }
        public IDbTransaction Transaction { get => _transaction; set { _transaction = value; _context.Transaction = value; } }

        public async Task<IEnumerable<Account>> GetAll()
        {
            string sQuery = "SELECT * FROM Accounts";
            return await _connection.QueryAsync<Account>(sQuery);
        }

        public async Task<IEnumerable<Account>> Get(Func<Account, bool> predicate)
        {
            string sQuery = "SELECT * FROM Accounts";
            IEnumerable<Account> accounts = await _connection.QueryAsync<Account>(sQuery, null);
            return accounts.Where(predicate);
        }

        public async Task<Account> GetOne(object id)
        {
            string sQuery = "SELECT * FROM Accounts WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Account>(sQuery, new { Id = id });
        }
        public async Task<int> Insert(Account entity)
        {
            int retval = -1;
            if (entity != null)
            {
                string sQuery = "INSERT INTO Accounts (Name, Email, Description, IsTrial, IsActive, SetActive, Created, Modified )"
                                             + " VALUES(@Name, @Email, @Description, @IsTrial, @IsActive, @SetActive, GETUTCDATE(), GETUTCDATE())";

                sQuery += "SELECT CAST(SCOPE_IDENTITY() as int); ";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                IEnumerable<int> retvals = await _connection.QueryAsync<int>(sQuery, entity, _transaction);
                retval = retvals.Single();
            }
            return retval;
        }
        public async Task Update(object id, Account entity)
        {
            if (entity != null)
            {
                string sQuery = "UPDATE Accounts SET Name=@Name, Email=@Email, Description=@Description, IsTrial=@IsTrial, IsActive=@IsActive, SetActive=@SetActive, Modified=GETUTCDATE() "
                                    + " WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                await _connection.QueryAsync(sQuery, entity, _transaction); ;
            }
        }
        public async Task Delete(object id)
        {
            string sQuery = "DELETE FROM Accounts"
                                + " WHERE Id = @Id";
            if (_transaction == null) Transaction = _connection.BeginTransaction();
            await _connection.ExecuteAsync(sQuery, new { Id = id }, _transaction);
        }
        public async Task Delete(Account entity)
        {
            if (entity != null)
            {
                var id = entity.Id;
                string sQuery = "DELETE FROM Accounts"
                                    + " WHERE Id = @Id";
                if (_transaction == null) Transaction = _connection.BeginTransaction();
                await _connection.ExecuteAsync(sQuery, new { Id = id }, _transaction);
            }
        }


    }


}
