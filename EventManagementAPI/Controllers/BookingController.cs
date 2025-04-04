using EventManagementSystemMerged.Repo;
using Microsoft.AspNetCore.Mvc;

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
            if (request == null || request.UserId <= 0 || request.EventId <= 0)
            {
                return BadRequest("Invalid booking request.");
            }

            _bookingProcessor.ProcessTicketBooking(request.UserId, request.EventId, request.PaymentStatus);

            return Ok("Booking processed successfully.");
        }
    }

    public class BookingRequest
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public bool PaymentStatus { get; set; }
    }
}
