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
    public class PaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }
        public List<Payment> GetAllPayments()
        {
            return  _context.Payments.ToList();
        }
        public void GenerateTicketIfPaymentCompleted(int paymentId)
        {
            var payment = _context.Payments.Find(paymentId);

            if (payment != null && payment.PaymentStatus == "Completed")
            {
                var ticket = new Ticket
                {
                    EventID = payment.EventID,
                    UserID = payment.UserID,
                    BookingDate = DateTime.Now,
                    Status = "Booked"
                };

                _context.Tickets.Add(ticket);
                 _context.SaveChanges();
            }

        }
        public void CreatePayment(Payment NewPayment)
        {
            _context.Payments.Add(NewPayment);
            _context.SaveChanges();
        }

    }
}
