using EventManagement_Merged_.Repos;
using EventManagementSystemMerged.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace EventManagement_Merged_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        private int? GetUserIdFromToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserID");

            if (userIdClaim == null)
            {
                return null;
            }

            return int.Parse(userIdClaim.Value);
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var userIdFromToken = GetUserIdFromToken();
            if (userIdFromToken != id) return Forbid();

            var user = _userService.GetUserById(id);
            if (user == null) return NotFound();

            var userDto = new
            {
                user.UserID,
                user.Name,
                user.Email,
                user.ContactNumber,
                user.UserType
            };

            return Ok(userDto);
        }

        [Authorize(Policy = "UserOnly")]
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            bool success = _userService.UpdateUser(id, userDto.Name, userDto.ContactNumber, userDto.UserType);
            if (!success) return NotFound();
            return Ok(new { message = "User updated successfully." });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("type/{userType}")]
        public IActionResult GetUsersByType(string userType)
        {
            var users = _userService.GetUsersByType(userType);
            var userDtos = users.Select(u => new
            {
                u.UserID,
                u.Name,
                u.Email,
                u.ContactNumber,
                u.UserType
            });

            return Ok(userDtos);
        }

        [Authorize(Policy = "UserOnly")]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            bool deleted = _userService.DeleteUser(id);
            if (!deleted) return NotFound();
            return Ok(new { message = "User deleted successfully." });
        }

        [Authorize(Policy = "UserOnly")]
        [HttpPut("recover/{id}")]
        public IActionResult RecoverUser(int id)
        {
            bool recovered = _userService.RecoverUser(id);
            if (!recovered) return NotFound();
            return Ok(new { message = "User recovered successfully." });
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("{id}/history")]
        public IActionResult GetUserHistory(int id)
        {
            var userIdFromToken = GetUserIdFromToken();
            if (userIdFromToken != id) return Forbid();

            var user = _userService.GetUserHistory(id);
            if (user == null) return NotFound();

            var historyDto = new
            {
                user.UserID,
                user.Name,
                user.Email,
                user.ContactNumber,
                user.UserType,
                Tickets = user.Tickets.Select(t => new { t.TicketID, t.EventID, t.BookingDate, t.Status }),
                Feedbacks = user.Feedbacks.Select(f => new { f.FeedbackID, f.EventID, f.Rating, f.Comments, f.SubmittedTimestamp }),
                Payments = user.Payments.Select(p => new { p.PaymentID, p.EventID, p.Amount, p.PaymentDate, p.PaymentStatus }),
                AttendedEvents = user.Tickets.Select(t => t.EventID).Distinct()
            };

            return Ok(historyDto);
        }

        public class UpdateUserDto
        {
            public string Name { get; set; }
            public string ContactNumber { get; set; }
            public string UserType { get; set; }
        }
    }
}
