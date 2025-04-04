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

        public void ProcessTicketBooking(int userId, int eventId, bool paymentStatus)
        {
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

                // Send the notification
                var eventName = GetEventName(eventId);
                var eventDate = GetEventStartDate(eventId);
                _notificationService.SendNotification(userId, eventId, true, eventName, eventDate);
            }
            else
            {
                // Send the notification for payment failure
                var eventName = GetEventName(eventId);
                var eventDate = GetEventStartDate(eventId);
                _notificationService.SendNotification(userId, eventId, false, eventName, eventDate);
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
