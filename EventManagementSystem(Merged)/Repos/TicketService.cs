using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;
using System.Collections.Generic;
using System.Linq;

namespace EventManagementSystem_Merged_.Repos
{
    public class TicketService
    {
        private readonly AppDbContext _context;

        public TicketService(AppDbContext context)
        {
            _context = context;
        }

        public List<Ticket> GetAllTickets()
        {
            return _context.Tickets.ToList();
        }

        public Ticket GetTicketById(int ticketId)
        {
            return _context.Tickets.Find(ticketId);
        }

        public int GetNumberOfTicketsSold(int eventId)
        {
            return _context.Tickets.Count(t => t.EventID == eventId);
        }

        public List<User> GetParticipants(int eventId)
        {
            return _context.Tickets
                           .Where(t => t.EventID == eventId)
                           .Join(_context.Users,
                                 ticket => ticket.UserID,
                                 user => user.UserID,
                                 (ticket, user) => user)
                           .ToList();
        }
    }
}
