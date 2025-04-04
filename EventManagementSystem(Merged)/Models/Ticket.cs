using System.ComponentModel.DataAnnotations;

namespace EventManagementSystemMerged.Models
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }
        public int EventID { get; set; }
        //public Event Event { get; set; }
        public int UserID { get; set; }
        //public User User { get; set; }
        public DateTime BookingDate { get; set; }
        public string? Status { get; set; }

    }
}
