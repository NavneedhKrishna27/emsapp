using System.ComponentModel.DataAnnotations;

namespace EventManagementSystemMerged.Models
{
    public class Payment
    {
        
        public int PaymentID { get; set; }
        public int UserID { get; set; }
        //public User User { get; set; }
        public int EventID { get; set; }
        //public Event Event { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public required string PaymentStatus { get; set; }
          
    }
}
