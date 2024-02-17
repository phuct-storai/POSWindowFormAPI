using Dapper;
using POSWindowFormAPI.Data.Constants;
using POSWindowFormAPI.Data.Repositories.Interfaces;
using POSWindowFormAPI.Models;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;

namespace POSWindowFormAPI.Data.Repositories
{
    public class BookingTableRepository : IBookingTableRepository
    {
        private readonly ILogger<BookingTableDetail> _logger;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public BookingTableRepository(ILogger<BookingTableDetail> logger, ISqlConnectionFactory sqlConnectionFactory)
        {
            _logger = logger;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public bool GetByUsername(string username)
        {
            string query = $"SELECT * FROM [dbo].[PSG_Bookings] WHERE Username = '{username}'";
            using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
            {
                var bookingTable = connection.QueryFirstOrDefault(query);
                if (connection.QueryFirstOrDefault(query) != null)
                {
                    _logger.LogInformation($"Get booking : {username}");
                    return true;
                }
                else
                {
                    _logger.LogInformation($"No booking exits - : {username}");
                    return false;
                }
            }
        }
        public bool GetByTime(string time)
        {
            string query = $"SELECT * FROM [dbo].[PSG_Bookings] WHERE Username = '{time}'";
            using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
            {
                var bookingTable = connection.QueryFirstOrDefault(query);
                if (connection.QueryFirstOrDefault(query) != null)
                {
                    _logger.LogInformation($"Get booking : {time}");
                    return true;
                }
                else
                {
                    _logger.LogInformation($"No booking exits - : {time}");
                    return false;
                }
            }
        }
        public bool GetByName(string name)
        {
            string query = $"SELECT * FROM [dbo].[PSG_Bookings] WHERE Username = '{name}'";
            using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
            {
                var bookingTable = connection.QueryFirstOrDefault(query);
                if (connection.QueryFirstOrDefault(query) != null)
                {
                    _logger.LogInformation($"Get booking : {name}");
                    return true;
                }
                else
                {
                    _logger.LogInformation($"No booking exits - : {name}");
                    return false;
                }
            }
        }

        public string CreateBooking(BookingTableDetail bookingTableDetail)
        {
            string query = "INSERT INTO [dbo].[PSG_Bookings] VALUES " +
                "(@BookingId, @Username, @Name, @People, @PhoneNumber, " +
                "@Date, @Time, @TypeAnniversary, @Note, " +
                "@Status, @CreateDate, @LastModified)";
            var queryParameter = new
            {
                BookingId = Guid.NewGuid().ToString(),
                Username = bookingTableDetail.Username,
                Name = bookingTableDetail.Name,
                People = bookingTableDetail.People,
                PhoneNumber = bookingTableDetail.PhoneNumber,
                Date = bookingTableDetail.Date,
                Time = bookingTableDetail.Time,
                TypeAnniversary = bookingTableDetail.TypeAnniversary,
                Note = bookingTableDetail.Note,
                Status = bookingTableDetail.Status,
                CreateDate = DateTime.Now.ToString("G"),
                LastModified = DateTime.Now.ToString("G")
            };
            try
            {
                using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
                {
                    if (!GetByUsername(bookingTableDetail.Name))
                    {
                        connection.Execute(query, queryParameter);
                        _logger.LogInformation($"Booking Complete!: {queryParameter.BookingId} - {queryParameter.Name}");
                        return BookingTableConstant.RESULT_SUCCESS;
                    }
                    else
                    {
                        _logger.LogInformation($"This slot is already booked. Try again");
                        return BookingTableConstant.RESULT_CONFLICT;
                    }
                        

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BookingTableConstant.RESULT_FAILED;
            }
        }

        public string UpdateBooking(string name, BookingTableDetail bookingTableDetail)
        {
            string query = "UPDATE [dbo].[PSG_Bookings] SET " +
                "Name = @Name, " +
                "People = @People, " +
                "PhoneNumber = @PhoneNumber, " +
                "Date = @Date, " +
                "Time = @Time, " +
                "TypeAnniversary = @TypeAnniversary, " +
                "Note= @Note, " +
                "CreateDate = @CreateDate, " +
                $"LastModified = @LastModified WHERE Name = '{name}'";
            var queryParameter = new
            {
                Username = bookingTableDetail.Username,
                Name = bookingTableDetail.Name,
                People = bookingTableDetail.People,
                PhoneNumber = bookingTableDetail.PhoneNumber,
                Date = bookingTableDetail.Date,
                Time = bookingTableDetail.Time,
                TypeAnniversary = bookingTableDetail.TypeAnniversary,
                Note = bookingTableDetail.Note,
                Status = bookingTableDetail.Status,
                CreateDate = bookingTableDetail.CreateDate,
                LastModified = bookingTableDetail.LastModified
            };

            try
            {
                using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
                {
                    var booking = connection.QueryFirstOrDefault($"SELECT * FROM [dbo].[PSG_Bookings] WHERE Name= '{name}'");
                    if (booking != null)
                    {
                        connection.Execute(query, queryParameter);
                        _logger.LogInformation($"Booking is Updated: {name} - Table for {queryParameter.People}: {queryParameter.Username}");
                        return BookingTableConstant.RESULT_SUCCESS;
                    }
                    else
                    {
                        _logger.LogInformation($"Unavailable booking");
                        return BookingTableConstant.RESULT_UNAVAILABLE;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return BookingTableConstant.RESULT_FAILED;
            }

        }
        public string DeleteBooking(string username)
        {
            string query = $"DELETE FROM [dbo].[PSG_Accounts] WHERE Username = '{username}'";

            try
            {
                using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
                {
                    if (GetByUsername(username))
                    {
                        connection.Execute(query);
                        _logger.LogInformation($"Account is Removed: {username}");
                        return BookingTableConstant.RESULT_SUCCESS;
                    }
                    else
                    {
                        _logger.LogInformation($"Unavailable account");
                        return BookingTableConstant.RESULT_UNAVAILABLE;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BookingTableConstant.RESULT_FAILED;
            }
        }

        public bool LoginValidate(string username, string password)
        {
            string query = $"SELECT * FROM [dbo].[PSG_Accounts] WHERE Username = '{username}' AND Password ='{password}";
            using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
            {
                var account = connection.QueryFirstOrDefault(query);
                if (connection.QueryFirstOrDefault(query) != null)
                {   
                    _logger.LogInformation($"Login success : {username}");
                    return true;
                }
                else
                {
                    _logger.LogInformation($"Login failed - : {username}");
                    return false;
                }
            }
        }
    }
}
