using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;

namespace EventManagement_Merged_.Repos
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.Where(u => !u.IsDelete).ToList();
        }

        public User? GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UserID == id && !u.IsDelete);
        }

        public List<User> GetUsersByType(string userType)
        {
            return _context.Users
                .Where(u => u.UserType.ToLower() == userType.ToLower() && !u.IsDelete)
                .ToList();
        }

        public User GetUserHistory(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId && !u.IsDelete);
            if (user == null) return null;

            user.Tickets = _context.Tickets.Where(t => t.UserID == userId).ToList();
            user.Feedbacks = _context.Feedbacks.Where(f => f.UserID == userId).ToList();
            user.Payments = _context.Payments.Where(p => p.UserID == userId).ToList();

            return user;
        }

        public List<Event> GetEventsByOrganizer(int organizerId)
        {
            return _context.Events.Where(e => e.UserID == organizerId).ToList();
        }

        public int GetParticipantsCount(int eventId)
        {
            return _context.Tickets.Count(t => t.EventID == eventId);
        }

        public List<User> GetParticipants(int eventId)
        {
            var userIds = _context.Tickets
            .Where(t => t.EventID == eventId)
            .Select(t => t.UserID)
            .Distinct()
            .ToList();

            return _context.Users
            .Where(u => userIds.Contains(u.UserID) && !u.IsDelete)
            .ToList();
        }

        public decimal GetRevenue(int eventId)
        {
            return _context.Payments
            .Where(p => p.EventID == eventId && p.PaymentStatus)
            .Sum(p => p.Amount);
        }



        public List<User> GetUsersByOrganizer(int organizerId)
        {
            var eventIds = _context.Events
            .Where(e => e.UserID == organizerId)
            .Select(e => e.EventID)
            .ToList();

            var userIds = _context.Tickets
            .Where(t => eventIds.Contains(t.EventID))
            .Select(t => t.UserID)
            .Distinct()
            .ToList();

            return _context.Users
            .Where(u => userIds.Contains(u.UserID) && !u.IsDelete)
            .ToList();
        }



        //public bool RegisterUser(User user)
        //{
        //    if (_context.Users.Any(u => u.Email == user.Email)) return false;

        //    _context.Users.Add(user);
        //    _context.SaveChanges();
        //    return true;
        //}

        //public User? Login(string email, string password)
        //{
        //    return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password && !u.IsDelete);
        //}

        public bool UpdateUser(int id, string name, string contactNumber, string userType)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == id);
            if (user == null) return false;

            user.Name = name;
            user.ContactNumber = contactNumber;
            user.UserType = userType;

            _context.SaveChanges();
            return true;
        }

        public bool DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == id);
            if (user == null) return false;

            user.IsDelete = true;
            _context.SaveChanges();
            return true;
        }

        public bool RecoverUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == id && u.IsDelete == true);
            if (user == null) return false;

            user.IsDelete = false;
            _context.SaveChanges();
            return true;
        }
    }
}
