namespace EventManagementSystemMerged.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public int UserID { get; set; }
   
        //public User User { get; set; }
        public int EventID { get; set; }
        //public Event Event { get; set; }
        public string Message { get; set; }
        public DateTime SentTimestamp { get; set; }
    }
}
