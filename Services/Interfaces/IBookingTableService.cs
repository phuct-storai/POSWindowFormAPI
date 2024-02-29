using POSWindowFormAPI.Models;

namespace POSWindowFormAPI.Services.Interfaces
{
    public interface IBookingTableService
    {
        public string GetAllBookings();
        public string CreateBooking(BookingTableDetail bookingTableDetail);
        public void UpdateBooking(string name, BookingTableDetail bookingTableDetail);
        public void DeleteBooking(string name);
        public bool GetByName(string name);
    }
}
