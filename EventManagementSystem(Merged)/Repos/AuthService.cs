
using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystemMerged.Repos
{
    public class AuthService
    {
        public User Register(User user)
        {
            using (var context = new AppDbContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
                return user;
            }
        }

        public User Login(string email, string password)
        {
            using (var context = new AppDbContext())
            {
                return context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            }
        }

        public bool IsAdmin(int userId)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.Find(userId);
                return user != null && user.UserType == "Admin";
            }
        }
    }
}
