using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSWindowFormAPI.Data.Constants;
using POSWindowFormAPI.Data.Repositories.Interfaces;
using POSWindowFormAPI.Models;
using POSWindowFormAPI.Models.Request;
using POSWindowFormAPI.Services;
using POSWindowFormAPI.Services.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace POSWindowFormAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingTableService _bookingTableService;
        private readonly IBookingTableRepository _bookingTableRepository;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public BookingController(ILogger<BookingController> logger,
                       IBookingTableService bookingTableService,
                       ISqlConnectionFactory sqlConnectionFactory,
                       IBookingTableRepository bookingTableRepository)
        {
            _logger = logger;
            _bookingTableService = bookingTableService;
            _sqlConnectionFactory = sqlConnectionFactory;
            _bookingTableRepository = bookingTableRepository;
        }
        [HttpGet("get-all-bookings")]
        public async Task<string> GetAllBookings()
        {
            return await _bookingTableService.GetAllBookings();
        }

        [HttpGet("get-by-username")]
        public bool GetByUsername(string username)
        {
            return _bookingTableService.GetByName(username);
        }
        [HttpPost("create-booking")]
        public ActionResult CreateBooking([FromBody] BookingTableRequest bookingTableRequest)
        {
            _bookingTableService.CreateBooking(bookingTableRequest);
            return Ok("SUCCESS");
        }

        [HttpPut("update-booking")]
        public void UpdateBooking(string name, [FromBody] BookingTableRequest bookingTableRequest)
        {
            _bookingTableService.UpdateBooking(name, bookingTableRequest);
        }

        [HttpDelete("delete-booking/{name}-{phoneNumber}-{date}")]
        public ActionResult DeleteBooking([FromRoute] BookingTableRequest bookingTableRequest)
        {
            _bookingTableService.DeleteBooking(bookingTableRequest);
            return Ok("OK");
        }

        [HttpGet("get-anniversary-type")]
        public string GetAnniversaryTypes()
        {
            return _bookingTableRepository.GetAnniversaryTypes();
        }

        [HttpGet("get-anniversary-type-by-name")]
        public ActionResult GetAnniverSaryTypeByName(string typeName)
        {
            return Ok(_bookingTableRepository.GetAnniversaryTypeByName(typeName));
        }

        [HttpPost("add-anniversary-type")]
        public ActionResult AddAnniversaryType(AnniversaryType anniversaryType)
        {
            if (_bookingTableService.AddAnniversaryType(anniversaryType) == BookingTableConstant.RESULT_SUCCESS)
            {
                return Ok(anniversaryType);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
