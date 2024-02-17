using POSWindowFormAPI.Models;

namespace POSWindowFormAPI.Data.Repositories.Interfaces
{
    public interface IBookingTableRepository
    {
        public string CreateBooking(BookingTableDetail bookingTableDetail);

        public string UpdateBooking(string name, BookingTableDetail bookingTableDetail);
        public string DeleteBooking(string username);
        public bool GetByUsername(string username);
        public bool GetByName(string name);

    }
}
