using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;
using System;

namespace EventManagementSystemMerged.Repo
{
    public class BookingProcessor
    {
        private readonly NotificationServices _notificationService;

        public BookingProcessor()
        {
            _notificationService = new NotificationServices();
        }

        public string ProcessTicketBooking(int userId, int eventId, bool paymentStatus)
        {
            using (var context = new AppDbContext())
            {
                var eventEntity = context.Events.Find(eventId);
                var location = context.Locations.Find(eventEntity.LocationID);

                if (eventEntity.BookedCapacity >= location.Capacity)
                {
                    return "Capacity is full. Cannot book the ticket for the event.";
                }

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

                    // Save the ticket to the database
                    SaveTicket(ticket);

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

        private void SaveTicket(Ticket ticket)
        {
            using (var context = new AppDbContext())
            {
                context.Tickets.Add(ticket);
                context.SaveChanges();
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
