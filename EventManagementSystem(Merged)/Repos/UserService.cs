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

        public bool RegisterUser(User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email)) return false;

            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }

        public User? Login(string email, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password && !u.IsDelete);
        }

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
