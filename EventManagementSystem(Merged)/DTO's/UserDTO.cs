using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem_Merged_.DTO_s
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ContactNumber { get; set; }
        public required string UserType { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
