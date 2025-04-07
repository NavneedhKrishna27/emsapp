using EventManagementSystemMerged.Repo;
using Microsoft.AspNetCore.Mvc;
using EventManagementSystemMerged.Models;

namespace EventManagementSystemMerged.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingProcessor _bookingProcessor;

        public BookingController()
        {
            _bookingProcessor = new BookingProcessor();
        }

        [HttpPost("book")]
        public IActionResult BookTicket([FromBody] BookingRequest request)
        {
            if (request == null || request.UserID <= 0 || request.EventID <= 0)
            {
                return BadRequest("Invalid booking request.");
            }

            var result = _bookingProcessor.ProcessTicketBooking(request.UserID, request.EventID, request.PaymentStatus);

            if (result == "Capacity is full. Cannot book the ticket for the event.")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("cancel")]
        public IActionResult CancelTicket([FromBody] BookingRequest request)
        {
            if (request == null || request.UserID <= 0 || request.EventID <= 0)
            {
                return BadRequest("Invalid cancellation request.");
            }

            var result = _bookingProcessor.CancelTicketBooking(request.UserID, request.EventID);

            if (result == "No confirmed booking found for the given user and event.")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }




    public class BookingRequest
    {
        public int UserID { get; set; }
        public int EventID { get; set; }
        public bool PaymentStatus { get; set; }
    }
}
