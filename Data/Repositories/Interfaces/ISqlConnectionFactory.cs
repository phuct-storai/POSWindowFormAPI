using System.Data;

namespace POSWindowFormAPI.Data.Repositories.Interfaces
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
        IDbConnection GetNewConnection();
    }
}
