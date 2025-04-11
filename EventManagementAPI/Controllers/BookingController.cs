using EventManagementSystemMerged.Repo;
using Microsoft.AspNetCore.Mvc;
using EventManagementSystemMerged.Models;
using System.Text.Json.Serialization;

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
        public IActionResult BookTicket([FromForm] BookingRequest request)
        {
            if (request == null || request.UserID <= 0 || request.EventID <= 0)
            {
                return BadRequest("Invalid booking request.");
            }

            var result = _bookingProcessor.ProcessTicketBooking(request.UserID, request.EventID);

            if (result == "Capacity is full. Cannot book the ticket for the event." || result == "Cannot book the event as it is already completed.")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("cancel")]
        public IActionResult CancelTicket([FromForm] BookingRequest request)
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

        [JsonIgnore]
        public bool PaymentStatus { get; set; }

        [JsonIgnore]
        public decimal Amount { get; set; }
    }



}
