using EventManagementSystem_Merged_.DTO_s;
using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;
using EventManagementSystemMerged.Repos;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;
        private readonly AppDbContext _context;

        public EventController()
        {
            _eventService = new EventService();
            _context = new AppDbContext();
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            var events = _eventService.GetAllEvents();
            return Ok(events);
        }

        [HttpPost("add-event")]
        public IActionResult AddEvent([FromForm] EventDTO eventDetails)
        {
            if (!eventDetails.IsPrice)
            {
                eventDetails.Price = 0; // Set price to 0 if IsPrice is not checked
            }
            _eventService.AddEvent(eventDetails);
            return Ok(new { message = "Event added successfully" });
        }

        [HttpGet("view-event/{id}")]
        public IActionResult ViewEventByID(int id)
        {
            var eventDetails = _eventService.GetEventById(id);
            if (eventDetails == null)
            {
                return NotFound(new { message = "Event not found" });
            }
            return Ok(eventDetails);
        }

        [HttpPut("edit-event")]
        public IActionResult EditEvent([FromForm] EventDTO evt)
        {
            if (!evt.IsPrice)
            {
                evt.Price = 0; // Set price to 0 if IsPrice is not checked
            }
            _eventService.UpdateEvent(evt, evt.EventID);
            return Ok(new { message = "Event updated successfully" });
        }

        [HttpDelete("delete-event/{id}")]
        public IActionResult DeleteConfirmation(int id)
        {
            _eventService.DeleteEvent(id);
            return Ok(new { message = "Event deleted successfully" });
        }

        [HttpGet("details/{id}")]
        public IActionResult Details(int id)
        {
            var eventDetails = _eventService.GetEventById(id);
            if (eventDetails == null)
            {
                return NotFound(new { message = "Event not found" });
            }
            return Ok(eventDetails);
        }

        [HttpGet("{eventId}/users")]
        public IActionResult GetUsersForEvent(int eventId)
        {
            var users = _eventService.GetUsersForEvent(eventId);
            return Ok(users);
        }

        [HttpGet("{eventId}/tickets-sold")]
        public IActionResult GetTicketsSoldForEvent(int eventId)
        {
            var ticketsSold = _eventService.GetTicketsSoldForEvent(eventId);
            return Ok(ticketsSold);
        }

        [HttpGet("{eventId}/revenue")]
        public IActionResult GetRevenueForEvent(int eventId)
        {
            var revenue = _eventService.GetRevenueForEvent(eventId);
            return Ok(revenue);
        }

        [HttpGet("{eventId}/status")]
        public IActionResult GetEventStatus(int eventId)
        {
            var status = _eventService.GetEventStatus(eventId);
            return Ok(status);
        }

        [HttpGet("completed")]
        public IActionResult GetCompletedEvents()
        {
            var events = _eventService.GetCompletedEvents();
            return Ok(events);
        }

        [HttpGet("upcoming")]
        public IActionResult GetUpcomingEvents()
        {
            var events = _eventService.GetUpcomingEvents();
            return Ok(events);
        }

        [HttpGet("current")]
        public IActionResult GetCurrentEvents()
        {
            var events = _eventService.GetCurrentEvents();
            return Ok(events);
        }

    }
}
