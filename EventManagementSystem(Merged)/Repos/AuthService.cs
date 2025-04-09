using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventManagement_Merged_.Repos
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserID", user.UserID.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.UserType)
                }),
                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string RegisterUser(User model)
        {
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                return "Email already exists.";
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                ContactNumber = model.ContactNumber,
                UserType = model.UserType,
                IsDelete = false
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return null;
        }


        public User Authenticate(string email, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password && !u.IsDelete);
        }

        public User GetCurrentUser(ClaimsPrincipal user)
        {
            var userId = user.FindFirst("UserID")?.Value;
            return _context.Users.FirstOrDefault(u => u.UserID.ToString() == userId);
        }
    }
}
