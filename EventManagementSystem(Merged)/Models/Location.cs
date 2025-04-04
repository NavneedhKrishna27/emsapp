namespace EventManagementSystemMerged.Models
{
    public class Location
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int Capacity { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string PrimaryContact { get; set; }
        public string? SecondaryContact { get; set; }
        
    }
}
