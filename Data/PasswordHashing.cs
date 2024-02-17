using System.Security.Cryptography;
using System.Text;

namespace POSWindowFormAPI.Data
{
    public class PasswordHashing
    {
        public string HashEncodingPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hashedPassword;
            }
        }
    }
}
