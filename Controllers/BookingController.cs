using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSWindowFormAPI.Models;
using POSWindowFormAPI.Services;
using POSWindowFormAPI.Services.Interfaces;

namespace POSWindowFormAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingTableService _bookingTableService;
        public BookingController(ILogger<BookingController> logger,
                       IBookingTableService bookingTableService)
        {
            _logger = logger;
            _bookingTableService = bookingTableService;
        }


        [HttpGet("get-by-username")]
        public bool GetByUsername(string username)
        {
            return _bookingTableService.GetByName(username);
        }
        [HttpPost("create-account")]
        public void CreateBooking(BookingTableDetail bookingTableDetail)
        {
            _bookingTableService.CreateBooking(bookingTableDetail);
        }

        [HttpPut("update-username")]
        public void UpdateBooking(string name, [FromBody] BookingTableDetail bookingTableDetail)
        {
            _bookingTableService.UpdateBooking(name, bookingTableDetail);
        }

        [HttpDelete("delete-username")]
        public void DeleteBooking(string username)
        {
            _bookingTableService.DeleteBooking(username);
        }
    }
}
