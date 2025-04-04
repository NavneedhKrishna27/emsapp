using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem_Merged_.DTO_s
{
    public  class EventDTO
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
    }
}
