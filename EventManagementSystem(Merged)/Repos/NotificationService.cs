using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;
using System;

namespace EventManagementSystemMerged.Repo
{
    public class NotificationServices
    {
        public void SendNotification(int userId, int eventId, bool bookingStatus, string eventName, DateTime eventDate, string customMessage = null)
        {
            using (var context = new AppDbContext())
            {
                var ticket = context.Tickets.FirstOrDefault(t => t.UserID == userId && t.EventID == eventId);
                if (ticket == null || ticket.Status == "Cancelled")
                {
                    
                    return;
                }

                var message = customMessage ?? (bookingStatus ? $"Your booking for {eventName} on {eventDate} is confirmed." : $"Your booking for {eventName} on {eventDate} failed.");
                var notification = new Notification
                {
                    UserID = userId,
                    EventID = eventId,
                    Message = message,
                    SentTimestamp = DateTime.Now
                };

                SaveNotification(notification);
            }
        }

        private void SaveNotification(Notification notification)
        {
            using (var context = new AppDbContext())
            {
                context.Notifications.Add(notification);
                context.SaveChanges();
            }
        }
    }


}
