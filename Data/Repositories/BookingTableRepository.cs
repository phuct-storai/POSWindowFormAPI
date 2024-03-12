using Dapper;
using Newtonsoft.Json;
using POSWindowFormAPI.Data.Constants;
using POSWindowFormAPI.Data.Repositories.Interfaces;
using POSWindowFormAPI.Models;
using POSWindowFormAPI.Models.Request;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Linq;

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

        // Booking Table

        public async Task<string> GetAllBookings(string today)
        {
            //List<string> bookingList = new List<string>();
            string query = "SELECT [Name], [People], [PhoneNumber], [Date], [Time], [AnniversaryType], [Status], [Note]" +
                        "FROM [PhoSaiGon].[dbo].[PSG_Bookings] " +
                        $"WHERE Date = '{today}' " +
                        "ORDER BY Date DESC, Time DESC ";
            using (IDbConnection connection = _sqlConnectionFactory.GetOpenConnection())
            {
                var bookingList = await connection.QueryAsync(query);
                return JsonConvert.SerializeObject(bookingList);
            }
            
        }


        public bool GetBookingByUsername(string username)
        {
            string query = $"SELECT [Name], [People], [PhoneNumber], [Date], [Time], [Status] FROM [dbo].[PSG_Bookings] WHERE Username = '{username}'";
            using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
            {
                var bookingTable = connection.QueryFirstOrDefault(query);
                if (bookingTable != null)
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
        public bool GetBookingByTime(string time)
        {
            string query = $"SELECT * FROM [dbo].[PSG_Bookings] WHERE Username = '{time}'";
            using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
            {
                var bookingTable = connection.QueryFirstOrDefault(query);
                if (bookingTable != null)
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
        public bool GetBookingByName(string name)
        {
            string query = $"SELECT * FROM [dbo].[PSG_Bookings] WHERE Username = '{name}'";
            using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
            {
                var bookingTable = connection.QueryFirstOrDefault(query);
                if (bookingTable != null)
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
                    if (!GetBookingByUsername(bookingTableDetail.Name))
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
        public string DeleteBooking(BookingTableRequest bookingTableRequest)
        {
            string query = $"DELETE FROM [dbo].[PSG_Bookings] WHERE Username = '{bookingTableRequest.Name}'";

            try
            {
                using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
                {
                    if (GetBookingByUsername(bookingTableRequest.Name))
                    {
                        connection.Execute(query);
                        _logger.LogInformation($"Account is Removed: {bookingTableRequest.Name}");
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


        // Anniversary Type
        public string AddAnniversaryType(AnniversaryType anniversaryType)
        {
            string query = "INSERT INTO [dbo].[PSG_AnniversaryType]" +
                "VALUES (@TypeID, @TypeName, @Note, @CreateDate, @LastModified)";
            var queryParameter = new
            {
                TypeID = Guid.NewGuid().ToString(),
                TypeName = anniversaryType.TypeName,
                Note = anniversaryType.Note,
                CreateDate = DateTime.Now.ToString("g"),
                LastModified = DateTime.Now.ToString("g"),
            };
            try
            {
                using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
                {
                    if (GetAnniversaryTypeByName(anniversaryType.TypeName) == null)
                    {
                        connection.Execute(query, queryParameter);
                        _logger.LogInformation($"Success Added to DB!: {queryParameter.TypeName}");
                        return BookingTableConstant.RESULT_SUCCESS;
                    }
                    else
                    {
                        _logger.LogInformation($"This Anniversary Type Exits Already. Try again");
                        return BookingTableConstant.RESULT_CONFLICT;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString() + " - " + nameof(AddAnniversaryType) + " - " + BookingTableConstant.RESULT_FAILED);
                return BookingTableConstant.RESULT_FAILED;
            }
        }

        public string GetAnniversaryTypeByName(string typeName)
        {
            string query = $"SELECT [TypeName] FROM [dbo].[PSG_AnniversaryType] WHERE TypeName = '{typeName}'";
            using (IDbConnection connection = _sqlConnectionFactory.GetOpenConnection())
            {
                var anniversaryType = connection.QueryFirstOrDefault(query);
                if (anniversaryType != null)
                {
                    _logger.LogInformation($"Get Anniversary Type: {typeName}");
                    return anniversaryType;
                }
                else
                {
                    _logger.LogInformation($"No Anniversary Type exits - : {typeName}");
                    return BookingTableConstant.RESULT_FAILED;
                }
            }
        }

        public string GetAnniversaryTypes()
        {
            string query = "SELECT [TypeName] FROM [dbo].[PSG_AnniversaryType]";
            List<string> anniversaryTypes = new();
            using (IDbConnection connection = _sqlConnectionFactory.GetOpenConnection())
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Assuming your data is in the first column, adjust accordingly
                            string data = reader.GetString(0);
                            anniversaryTypes.Add(data);
                        }

                        return JsonConvert.SerializeObject(anniversaryTypes);
                    }
                }
            }
        }
    }
}
