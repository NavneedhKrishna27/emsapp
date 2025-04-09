using EventManagement_Merged_.Repos;
using EventManagementSystemMerged.Data;
using EventManagementSystemMerged.Models;
using Microsoft.AspNetCore.Authorization;
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
            public string UserType { get; set; } = "User"; // Default to "User"
        }

        public class LoginDTO
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDTO model)
        {
            // Ensure only users or organizers can be created
            if (model.UserType != "User" && model.UserType != "Organizer")
            {
                return BadRequest("Only users or organizers can be registered through this endpoint.");
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                ContactNumber = model.ContactNumber,
                UserType = model.UserType
            };

            var result = _authService.RegisterUser(user);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("User registered successfully.");
        }

        [HttpPost("register-admin")]
        [Authorize(Policy = "SuperAdminOnly")]
        public IActionResult RegisterAdmin([FromBody] RegisterUserDTO model)
        {
            // Ensure only superAdmins can create admin accounts
            if (model.UserType != "Admin")
            {
                return BadRequest("This endpoint is only for creating admin accounts.");
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                ContactNumber = model.ContactNumber,
                UserType = model.UserType
            };

            var result = _authService.RegisterUser(user);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Admin registered successfully.");
        }

        [HttpPost("register-superadmin")]
        public IActionResult RegisterSuperAdmin([FromBody] RegisterUserDTO model)
        {
            // Ensure only superAdmins can be created through this endpoint
            if (model.UserType != "SuperAdmin")
            {
                return BadRequest("This endpoint is only for creating superAdmin accounts.");
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                ContactNumber = model.ContactNumber,
                UserType = model.UserType
            };

            var result = _authService.RegisterUser(user);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("SuperAdmin registered successfully.");
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
