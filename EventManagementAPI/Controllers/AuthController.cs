
using EventManagement_Merged_.Repos;
using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement_Merged_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;

        public AuthController(AppDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // DTOs inside the controller
        public class RegisterUserDTO
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string ContactNumber { get; set; }
            public string UserType { get; set; }
        }

        public class LoginDTO
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDTO model)
        {
            if (_context.Users.Any(u => u.Email == model.Email))
                return BadRequest("Email already exists.");

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

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO model)
        {
            var user = _authService.Authenticate(model.Email, model.Password);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = _authService.GenerateToken(user);
            return Ok(new { token });
        }
    }
}