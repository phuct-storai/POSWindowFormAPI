using System.ComponentModel.DataAnnotations;

namespace POSWindowFormAPI.Models
{
    public class BookingTableDetail
    {
        public string? BookingId { get; set; }
        public string? Username { get; set; }
        public string Name { get; set; }
        public int People { get; set; }
        public string PhoneNumber { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string TypeAnniversary { get; set; }
        public string Note { get; set; } 
        public string? Status { get; set; }
        public string? CreateDate { get; set; }
        public string? LastModified { get; set; }
    }
}