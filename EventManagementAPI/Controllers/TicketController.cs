using EventManagementSystem_Merged_.Repos;
using EventManagementSystemMerged.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public TicketController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public ActionResult<List<Ticket>> GetTickets()
        {
            var tickets = _ticketService.GetAllTickets();
            return Ok(tickets);
        }

        [HttpGet("{ticketId}")]
        public ActionResult<Ticket> GetTicketId(int ticketId)
        {
            var ticket = _ticketService.GetTicketById(ticketId);
            if (ticket == null)
            {
                return NotFound(new { message = "Ticket not found" });
            }
            return Ok(ticket);
        }

        [HttpGet("tickets-sold/{eventId}")]
        public ActionResult<int> GetTicketsSold(int eventId)
        {
            var ticketsSold = _ticketService.GetNumberOfTicketsSold(eventId);
            return Ok(ticketsSold);
        }

        [HttpGet("participants/{eventId}")]
        public ActionResult<List<User>> GetParticipants(int eventId)
        {
            var participants = _ticketService.GetParticipants(eventId);
            return Ok(participants);
        }
    }
}
