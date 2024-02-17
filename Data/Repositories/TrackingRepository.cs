using Dapper;
using POSWindowFormAPI.Data.Constants;
using POSWindowFormAPI.Data.Repositories.Interfaces;
using POSWindowFormAPI.Models;
using POSWindowFormAPI.Models.Request;
using System.Data;

namespace POSWindowFormAPI.Data.Repositories
{
    public class TrackingRepository : ITrackingRepository
    {
        private readonly ILogger<Tracking> _logger;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;


        public TrackingRepository(ILogger<Tracking> logger, ISqlConnectionFactory sqlConnectionFactory)
        {
            _logger = logger;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public void AddTracking(Tracking tracking)
        {
            string query = "INSERT INTO [dbo].[PSG_Trackings] VALUES " +
                "(@ActionID, @Username, @ActionType, @ActionDetail, @Date, @Status)";
            var queryParameter = new
            {
                ActionID = tracking.ActionId,
                Username = tracking.Username,
                ActionType = tracking.ActionType,
                ActionDetail = tracking.ActionDetail,
                Date = tracking.Date,
                Status = tracking.Status
            };
            try
            {
                using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
                {
                    connection.Execute(query, queryParameter);
                    _logger.LogInformation($"Action is completed: {queryParameter.ActionID} - {queryParameter.ActionType}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _logger.LogInformation($"Action is failed.");
            }
        }
    }
}
