using POSWindowFormAPI.Controllers;
using POSWindowFormAPI.Data.Constants;
using POSWindowFormAPI.Data.Repositories;
using POSWindowFormAPI.Data.Repositories.Interfaces;
using POSWindowFormAPI.Models;
using POSWindowFormAPI.Models.Request;
using POSWindowFormAPI.Services.Interfaces;

namespace POSWindowFormAPI.Services
{
    public class BookingTableService : IBookingTableService
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingTableRepository _bookingTableRepository;
        private readonly ITrackingRepository _trackingRepository;
        public BookingTableService(ILogger<BookingController> logger,
                    IBookingTableRepository bookingTableRepository,
                    ITrackingRepository trackingRepository)
        {
            _logger = logger;
            _bookingTableRepository = bookingTableRepository;
            _trackingRepository = trackingRepository;
        }

        public string CreateBooking(BookingTableDetail bookingTableDetail)
        {
            //Add tracking
            Tracking tracking = new Tracking
            {
                ActionId = Guid.NewGuid().ToString(),
                Username = bookingTableDetail.Username,
                ActionType = TrackingConstant.CREATE_BOOKING,
                ActionDetail = $"Create new booking - {bookingTableDetail.BookingId}",
                Date = DateTime.Now.ToString("G"),
                Status = string.Empty
            };
            if (_bookingTableRepository.CreateBooking(bookingTableDetail) == BookingTableConstant.RESULT_SUCCESS)
            {
                tracking.Status = TrackingConstant.SUCCESS;
                _trackingRepository.AddTracking(tracking);
            }

            else
            {
                tracking.Status = TrackingConstant.FAILED;
                _trackingRepository.AddTracking(tracking);
            }
            return tracking.Status;

        }
        public void UpdateBooking(string name, BookingTableDetail bookingTableDetail)
        {
            _bookingTableRepository.UpdateBooking(name, bookingTableDetail);

        }
        public void DeleteBooking(string name)
        {
            _bookingTableRepository.DeleteBooking(name);

        }
        public bool GetByName(string name)
        {
            return true;
        }
    }
}
