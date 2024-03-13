using POSWindowFormAPI.Models;
using POSWindowFormAPI.Models.Request;

namespace POSWindowFormAPI.Services.Interfaces
{
    public interface IBookingTableService
    {
        public Task<string> GetAllBookings();
        public string CreateBooking(BookingTableDetail bookingTableDetail);
        public void UpdateBooking(string name, BookingTableDetail bookingTableDetail);
        public void DeleteBooking(string bookingId);
        public bool GetByName(string name);
        public string AddAnniversaryType(AnniversaryType anniversaryType);
    }
}
