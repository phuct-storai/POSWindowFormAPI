using System.ComponentModel.DataAnnotations;

namespace POSWindowFormAPI.Models
{
    public class Account
    {
        public string? ID { get; set; }
        public string? Name { get; set; }
        public string? Username{ get; set; }
        public string? Password { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Position{ get; set; }
        public bool? isEmailVerified { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? isPhoneNumberVerified { get; set; }
        public string? CreateDate { get; set; }
        public string? LastModified { get; set; }

    }
}
