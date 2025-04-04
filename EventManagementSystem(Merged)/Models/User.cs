using System.ComponentModel.DataAnnotations;

namespace EventManagementSystemMerged.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ContactNumber { get; set; }
        public required string UserType { get; set; }
        public bool IsDelete { get; set; } = false;
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Notification> Notifications{ get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

    }
}
