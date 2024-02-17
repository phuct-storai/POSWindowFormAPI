using Dapper;
using POSWindowFormAPI.Data.Constants;
using POSWindowFormAPI.Data.Repositories.Interfaces;
using POSWindowFormAPI.Models;
using POSWindowFormAPI.Models.Request;
using POSWindowFormAPI.Services.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Security.Principal;

namespace POSWindowFormAPI.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ILogger<Account> _logger;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly PasswordHashing passwordHashing = new PasswordHashing();


        public AccountRepository(ILogger<Account> logger, ISqlConnectionFactory sqlConnectionFactory)
        {
            _logger = logger;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public Account GetByUsername(string username)
        {
            string query = $"SELECT * FROM [dbo].[PSG_Accounts] WHERE Username = '{username}'";
            using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
            {
                var account = connection.QueryFirstOrDefault(query);
                if (connection.QueryFirstOrDefault(query) != null)
                {
                    Account resultAccount = new Account
                    {
                        ID = account.ID,
                        Username = account.Username,
                        Password = account.Password,
                        Name = account.Name,
                        Address = account.Address,
                        Gender = account.Gender,
                        Email = account.Email,
                        PhoneNumber = account.PhoneNumber,
                        isEmailVerified = account.IsEmailVerified,
                        isPhoneNumberVerified = account.IsPhoneNumberVerified,
                        CreateDate = account.CreateDate,
                        LastModified = account.LastModified,
                    };
                    _logger.LogInformation($"Get account : {username}");
                    return resultAccount;
                }
                else
                {
                    _logger.LogInformation($"No account exits - : {username}");
                    return null;
                }
            }
        }

        public string CreateAccount(AuthenticationRequest authenticationRequest)
        {
            string query = "INSERT INTO [dbo].[PSG_Accounts] VALUES " +
                "(@Id, @Username, @Password, @Name, @Gender, @PhoneNumber, " +
                "@Email, @Address, @Position, @IsPhoneNumberVerified, @IsEmailVerified, " +
                "@CreateDate, @LastModified)";
            var queryParameter = new
            {
                requestTime = DateTime.Now.ToString("G"),
                ID = Guid.NewGuid().ToString(),
                Username = authenticationRequest.Username,
                Password = passwordHashing.HashEncodingPassword(authenticationRequest.Password),
                Name = authenticationRequest.Name,
                Gender = authenticationRequest.Gender,
                Address = authenticationRequest.Address,
                PhoneNumber = authenticationRequest.PhoneNumber,
                Email = authenticationRequest.Email,
                Position = "Customer",
                IsPhoneNumberVerified = false,
                IsEmailVerified = false,
                CreateDate = DateTime.Now.ToString("G"),
                LastModified = DateTime.Now.ToString("G"),
            };
            try
            {
                using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
                {
                    if (GetByUsername(authenticationRequest.Username) == null)
                    {
                        connection.Execute(query, queryParameter);
                        _logger.LogInformation($"Account is Created: {authenticationRequest.ID} - {authenticationRequest.Username}");
                        return AuthenticaticalConstants.SUCCESS;
                    }
                    else
                    {
                        _logger.LogInformation($"Account Exits");
                        return AuthenticaticalConstants.EXITS_USERNAME;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return AuthenticaticalConstants.FAILED;
            }
        }

        public bool UpdateAccount(string username, AuthenticationRequest authenticationRequest)
        {
            string query = "UPDATE [dbo].[PSG_Accounts] SET " +
                "Password = @Password, " +
                "Name = @Name, " +
                "Gender = @Gender, " +
                "PhoneNumber = @PhoneNumber, " +
                "Email = @Email, " +
                "Address = @Address, " +
                "Position = @Position, " +
                "IsPhoneNumberVerified = @IsPhoneNumberVerified, " +
                "IsEmailVerified = @IsEmailVerified, " +
                "CreateDate = @CreateDate, " +
                $"LastModified = @LastModified WHERE Username = '{username}'";
            var queryParameter = new AuthenticationRequest
            {
                requestTime = DateTime.Now.ToString("G"),
                Password = authenticationRequest.Password,
                Name = authenticationRequest.Name,
                Gender = authenticationRequest.Gender,
                Address = authenticationRequest.Address,
                PhoneNumber = authenticationRequest.PhoneNumber,
                Email = authenticationRequest.Email,
                Position = authenticationRequest.Position,
                isPhoneNumberVerified = authenticationRequest.isPhoneNumberVerified,
                isEmailVerified = authenticationRequest.isEmailVerified,
                CreateDate = authenticationRequest.CreateDate,
                LastModified = DateTime.Now.ToString("G"),
            };

            try
            {
                using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
                {
                    var account = connection.QueryFirstOrDefault($"SELECT * FROM [dbo].[PSG_Accounts] WHERE Username = '{username}'");
                    if (account != null)
                    {
                        if (account.Email != authenticationRequest.Email)
                            queryParameter.isEmailVerified = false;
                        if (account.PhoneNumber != authenticationRequest.PhoneNumber)
                            queryParameter.isPhoneNumberVerified = false;
                        connection.Execute(query, queryParameter);
                        _logger.LogInformation($"Account is Updated: {username}");
                        return true;
                    }
                    else
                    {
                        _logger.LogInformation($"Unavailable account");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return false;
            }

        }
        public bool DeleteAccount(string username)
        {
            string query = $"DELETE FROM [dbo].[PSG_Accounts] WHERE Username = '{username}'";

            try
            {
                using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
                {
                    if (GetByUsername(username) != null)
                    {
                        connection.Execute(query);
                        _logger.LogInformation($"Account is Removed: {username}");
                        return true;
                    }
                    else
                    {
                        _logger.LogInformation($"Unavailable account");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }

        public bool LoginValidate(string username, string password)
        {
            string query = $"SELECT * FROM [dbo].[PSG_Accounts] WHERE Username ='{username}' AND Password ='{passwordHashing.HashEncodingPassword(password)}'";
            using (IDbConnection connection = _sqlConnectionFactory.GetNewConnection())
            {
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
