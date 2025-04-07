
using EventManagementSystem_Merged_.DTO_s;
using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;

namespace EventManagementSystemMerged.Repos
{
    public class EventService
    {
        #region Add Event
        public void AddEvent(Event eventDetails)
        {
            using (var context = new AppDbContext())
            {
                context.Events.Add(eventDetails);
                context.SaveChanges();
            }
        }
        #endregion

        #region Get All Events
        public List<Event> GetAllEvents()
        {
            using (var context = new AppDbContext())
            {
                return context.Events.ToList();
            }
        }
        #endregion

        #region Get Event By Id
        public Event GetEventById(int id)
        {
            using (var context = new AppDbContext())
            {
                return context.Events.Find(id);
            }
        }
        #endregion

        #region Update Event
        public void UpdateEvent(EventDTO eventDetails, int id)
        {
            using (var context = new AppDbContext())
            {
                var existingEvent = context.Events.Find(id);
                if (existingEvent != null)
                {
                    // Check if the start date has changed
                    bool isDateChanged = existingEvent.StartDate != eventDetails.StartDate;

                    existingEvent.Name = eventDetails.Name;
                    existingEvent.CategoryID = eventDetails.CategoryID;
                    existingEvent.LocationID = eventDetails.LocationID;
                    existingEvent.StartDate = eventDetails.StartDate;
                    existingEvent.EndDate = eventDetails.EndDate;
                    existingEvent.UserID = eventDetails.UserID;
                    existingEvent.Description = eventDetails.Description;
                    existingEvent.IsPrice = eventDetails.IsPrice;
                    existingEvent.Price = eventDetails.Price;
                    existingEvent.IsActive = eventDetails.IsActive;

                    context.SaveChanges();

                    if (isDateChanged)
                    {
                        NotifyUsersOfDateChange(existingEvent.EventID, existingEvent.StartDate);
                    }
                }
            }
        }

        private void NotifyUsersOfDateChange(int eventId, DateTime newDate)
        {
            using (var context = new AppDbContext())
            {
                var users = context.Tickets
                                   .Where(t => t.EventID == eventId)
                                   .Select(t => t.UserID)
                                   .Distinct()
                                   .ToList();

                var eventName = context.Events.Find(eventId)?.Name ?? "Unknown Event";

                foreach (var userId in users)
                {
                    var notification = new Notification
                    {
                        UserID = userId,
                        EventID = eventId,
                        Message = $"The date for {eventName} has been changed to {newDate}.",
                        SentTimestamp = DateTime.Now
                    };

                    context.Notifications.Add(notification);
                }

                context.SaveChanges();
            }
        }

        #endregion

        #region Delete Event
        public void DeleteEvent(int id)
        {
            using (var context = new AppDbContext())
            {
                var eventToRemove = context.Events.Find(id);
                if (eventToRemove != null)
                {
                    context.Events.Remove(eventToRemove);
                    context.SaveChanges();
                }
            }
        }

        public void AddEvent(EventDTO eventDetails)
        {
            using (var context = new AppDbContext())
            {
                var eventEntity = new Event
                {
                    EventID = eventDetails.EventID,
                    Name = eventDetails.Name,
                    CategoryID = eventDetails.CategoryID,
                    LocationID = eventDetails.LocationID,
                    StartDate = eventDetails.StartDate,
                    EndDate = eventDetails.EndDate,
                    UserID = eventDetails.UserID,
                    Description = eventDetails.Description,
                    IsPrice = eventDetails.IsPrice,
                    Price = eventDetails.Price ?? 0,
                    IsActive = eventDetails.IsActive
                };
                context.Events.Add(eventEntity);
                context.SaveChanges();
            }
        }


        
        #endregion
    }
}
