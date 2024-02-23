using POSWindowFormAPI.Models;

namespace POSWindowFormAPI.Data.Repositories.Interfaces
{
    public interface IBookingTableRepository
    {
        public string CreateBooking(BookingTableDetail bookingTableDetail);

        public string UpdateBooking(string name, BookingTableDetail bookingTableDetail);
        public string DeleteBooking(string username);
        public bool GetBookingByUsername(string username);
        public bool GetBookingByName(string name);

        public string AddAnniversaryType(AnniversaryType anniversaryType);
        public string GetAnniversaryTypes();
        public string GetAnniversaryTypeByName(string typeName);

    }
}
