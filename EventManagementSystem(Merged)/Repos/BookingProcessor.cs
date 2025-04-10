using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;
using System;
using System.Linq;

namespace EventManagementSystemMerged.Repo
{
    public class BookingProcessor
    {
        private readonly NotificationServices _notificationService;

        public BookingProcessor()
        {
            _notificationService = new NotificationServices();
        }

        public string ProcessTicketBooking(int userId, int eventId)
        {
            using (var context = new AppDbContext())
            {
                var eventEntity = context.Events.Find(eventId);
                var location = context.Locations.Find(eventEntity.LocationID);

                // Check if the event is already completed
                if (DateTime.Now > eventEntity.EndDate)
                {
                    return "Cannot book the event as it is already completed.";
                }

                if (eventEntity.BookedCapacity >= location.Capacity)
                {
                    return "Capacity is full. Cannot book the ticket for the event.";
                }

                // Fetch payment details based on EventID
                decimal amount = GetEventPrice(eventId);
                bool paymentStatus = true; // Assuming payment is successful for simplicity

                if (paymentStatus)
                {
                    // Create a new ticket
                    var ticket = new Ticket
                    {
                        UserID = userId,
                        EventID = eventId,
                        BookingDate = DateTime.Now,
                        Status = "Confirmed"
                    };

                    // Create a new payment
                    var payment = new Payment
                    {
                        UserID = userId,
                        EventID = eventId,
                        Amount = amount,
                        PaymentDate = DateTime.Now,
                        PaymentStatus = true
                    };

                    // Save the ticket and payment to the database
                    context.Tickets.Add(ticket);
                    context.Payments.Add(payment);

                    // Increment the booked capacity
                    eventEntity.BookedCapacity++;
                    context.SaveChanges();

                    // Send the notification
                    var eventName = GetEventName(eventId);
                    var eventDate = GetEventStartDate(eventId);
                    _notificationService.SendNotification(userId, eventId, true, eventName, eventDate);

                    return "Booking processed successfully.";
                }
                else
                {
                    // Send the notification for payment failure
                    var eventName = GetEventName(eventId);
                    var eventDate = GetEventStartDate(eventId);
                    _notificationService.SendNotification(userId, eventId, false, eventName, eventDate);

                    return "Payment failed. Booking not processed.";
                }
            }
        }

        public string CancelTicketBooking(int userId, int eventId)
        {
            using (var context = new AppDbContext())
            {
                var ticket = context.Tickets.FirstOrDefault(t => t.UserID == userId && t.EventID == eventId && t.Status == "Confirmed");
                if (ticket == null)
                {
                    return "No confirmed booking found for the given user and event.";
                }

                // Update the ticket status to "Cancelled"
                ticket.Status = "Cancelled";

                // Update the payment status to "Refund" and amount to 0
                var payment = context.Payments.FirstOrDefault(p => p.UserID == userId && p.EventID == eventId && p.PaymentStatus == true);
                if (payment != null)
                {
                    payment.PaymentStatus = false;
                    payment.Amount = 0;
                }

                context.SaveChanges();

                // Reduce the booked capacity
                var eventEntity = context.Events.Find(eventId);
                eventEntity.BookedCapacity--;
                context.SaveChanges();

                // Send the cancellation notification
                var eventName = GetEventName(eventId);
                var eventDate = GetEventStartDate(eventId);
                _notificationService.SendNotification(userId, eventId, false, eventName, eventDate, "Your booking has been cancelled.");

                return "Booking cancelled successfully.";
            }
        }

        private decimal GetEventPrice(int eventId)
        {
            using (var context = new AppDbContext())
            {
                var eventEntity = context.Events.Find(eventId);
                return eventEntity?.Price ?? 0;
            }
        }

        private string GetEventName(int eventId)
        {
            using (var context = new AppDbContext())
            {
                var eventEntity = context.Events.Find(eventId);
                return eventEntity?.Name ?? "Unknown Event";
            }
        }

        private DateTime GetEventStartDate(int eventId)
        {
            using (var context = new AppDbContext())
            {
                var eventEntity = context.Events.Find(eventId);
                return eventEntity?.StartDate ?? DateTime.MinValue;
            }
        }
    }
}
