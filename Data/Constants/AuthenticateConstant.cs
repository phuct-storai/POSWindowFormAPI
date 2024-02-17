using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSWindowFormAPI.Data.Constants
{
    public class AuthenticaticalConstants
    {
        // AUTHENTICATE USERNAME

        public const string INVALID_USERNAME = "Invalid Username. Try again !";
        public const string NOT_EXITS_USERNAME = "No Username founded. Try again !";
        public const string EXITS_USERNAME = "Account Exits Already, Try Again!";

        // AUTHENTICATE PASSWORD

        public const string WRONG_PASSWORD = "Wrong password. Try again!";
        public const string INVALID_PASSWORD = "Invalid password. Try again !";


        // STATUS
        public const string SUCCESS = "Success";
        public const string FAILED = "Failed";

        //AUTHENTICATE TYPE
        public const string LOGIN = "LOGIN";
        public const string REGISTER = "REGISTER";
    }
}
