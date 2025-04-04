using EventManagementSystem_Merged_.Repos;
using EventManagementSystemMerged.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketService _TicketService;
        public TicketController(TicketService TicketService)
        {
            _TicketService = TicketService;


        }
        [HttpGet]
        public ActionResult<List<Ticket>> GetTickets()
        {
            var Tickets =  _TicketService.GetAllTicketsAsync();
            return Ok(Tickets);
        }
    }
}
