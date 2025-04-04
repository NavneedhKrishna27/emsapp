using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem_Merged_.Repos
{
    public class TicketService
    {
        private readonly AppDbContext _context;

        public TicketService(AppDbContext context)
        {
            _context = context;
        }

        public List<Ticket> GetAllTicketsAsync()
        {
            return _context.Tickets.ToList();
        }



    }
}
