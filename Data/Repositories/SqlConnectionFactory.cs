using POSWindowFormAPI.Data.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace POSWindowFormAPI.Data.Repositories
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly IConfiguration _configuration;
        private IDbConnection _connection;
        private bool _disposed = false;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetOpenConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                _connection.Open();
            }

            return _connection;
        }

        public IDbConnection GetNewConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        /// <summary>
        /// Document: https://rules.sonarsource.com/csharp/RSPEC-3881
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing && _connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Dispose();
            }

            _disposed = true;
        }
    }
}
