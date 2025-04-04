
using EventManagementSystem_Merged_.DTO_s;
using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;
using EventManagementSystemMerged.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        //[HttpGet("add-event")]
        //public IActionResult AddEvent()
        //{
        //    var categories = _context.Categories.ToList();
        //    var locations = _context.Locations.ToList();
        //    var users = _context.Users.ToList();
        //    return Ok(new { categories, locations, users });
        //}

        [HttpPost("add-event")]
        public IActionResult AddEvent([FromBody] EventDTO eventDetails)
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

        //[HttpGet("edit-event/{id}")]
        //public IActionResult EditEvent(int id)
        //{
        //    var eventEdit = _eventService.GetEventById(id);
        //    if (eventEdit == null)
        //    {
        //        return NotFound(new { message = "Event not found" });
        //    }
        //    var categories = _context.Categories.ToList();
        //    var locations = _context.Locations.ToList();
        //    var users = _context.Users.ToList();
        //    return Ok(new { eventEdit, categories, locations, users });
        //}

        [HttpPut("edit-event")]
        public IActionResult EditEvent([FromBody] EventDTO evt)
        {
            if (!evt.IsPrice)
            {
                evt.Price = 0; // Set price to 0 if IsPrice is not checked
            }
            _eventService.UpdateEvent(evt, evt.EventID);
            return Ok(new { message = "Event updated successfully" });
        }

        //[HttpGet("delete-event/{id}")]
        //public IActionResult DeleteEvent(int id)
        //{
        //    var eventDelete = _eventService.GetEventById(id);
        //    if (eventDelete == null)
        //    {
        //        return NotFound(new { message = "Event not found" });
        //    }
        //    return Ok(eventDelete);
        //}

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
    }
}
