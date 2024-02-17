using POSWindowFormAPI.Models;
using POSWindowFormAPI.Models.Request;
using System.Net;

namespace POSWindowFormAPI.Data.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        public string CreateAccount(AuthenticationRequest authenticationRequest);
        public bool UpdateAccount(string username, AuthenticationRequest authenticationRequest);
        public bool DeleteAccount(string username);
        public Account GetByUsername(string username);
        public bool LoginValidate(string username, string password);


    }
}
