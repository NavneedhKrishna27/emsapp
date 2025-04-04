namespace EventManagementSystemMerged.Models
{
    public class Event
    {
        public int EventID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
       
        public int LocationID { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserID { get; set; }
        //public User User { get; set; }
        public string Description { get; set; }
        public bool IsPrice { get; set; }
        public decimal? Price { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

    }
}
