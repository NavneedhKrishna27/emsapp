using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Linq;

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
                SendEmail(userId, message);
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

        private void SendEmail(int userId, string message)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.Find(userId);
                if (user == null) return;

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Event Management System", "caitlyn.smith25@ethereal.email"));
                emailMessage.To.Add(new MailboxAddress(user.Name, user.Email));
                emailMessage.Subject = "Event Notification";
                emailMessage.Body = new TextPart("plain")
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.ethereal.email", 587, false);
                    client.Authenticate("caitlyn.smith25@ethereal.email", "cHj96Z7b9u4r85gmBr");
                    client.Send(emailMessage);
                    client.Disconnect(true);
                }
            }
        }

        public void SendEventUpdateNotification(int eventId, string updateMessage)
        {
            using (var context = new AppDbContext())
            {
                var eventEntity = context.Events.Find(eventId);
                if (eventEntity == null) return;

                var participants = context.Tickets
                    .Where(t => t.EventID == eventId && t.Status == "Confirmed")
                    .Select(t => t.UserID)
                    .Distinct()
                    .ToList();

                foreach (var userId in participants)
                {
                    SendEmail(userId, updateMessage);
                }
            }
        }
    }
}
