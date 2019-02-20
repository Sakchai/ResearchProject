
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Project.Entity.Context
{

    public interface IDbConnectionProvider
    {
        IDbConnection Connection { get; }
    }

    public class ProjectContext : IDbConnectionProvider
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        public ProjectContext(string connection)
        {
            _connection = new SqlConnection(connection);
        }

        public IDbConnection Connection { get => _connection; }
        public IDbTransaction Transaction { get => _transaction; set => _transaction = value; }

    }
}





