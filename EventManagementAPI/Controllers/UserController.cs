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
        public IActionResult UpdateUser(int id, [FromForm] UpdateUserDto userDto)
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

        [Authorize(Policy = "OrganizerOnly")]
        [HttpGet("organizer-history/{organizerId}")]
        public IActionResult GetOrganizerHistory(int organizerId)
        {
            var userIdFromToken = GetUserIdFromToken();
            if (userIdFromToken == null) return Forbid();

            var user = _userService.GetUserById(userIdFromToken.Value);
            if (user == null || (user.UserType != "Admin" && user.UserID != organizerId)) return Forbid();

            var events = _userService.GetEventsByOrganizer(organizerId);
            var eventDetails = events.Select(e => new
            {
                e.EventID,
                e.Name,
                Participants = _userService.GetParticipantsCount(e.EventID),
                Revenue = _userService.GetRevenue(e.EventID),
                ParticipantsDetails = _userService.GetParticipants(e.EventID).Select(p => new
                {
                    p.UserID,
                    p.Name,
                    p.Email
                }).ToList()
            }).ToList();

            var topRevenueEvent = eventDetails.OrderByDescending(e => e.Revenue).FirstOrDefault();
           
            var history = new
            {
                OrganizerID = organizerId,
                Events = eventDetails,
                TopRevenueEvent = topRevenueEvent,
               
            };

            return Ok(history);
        }


        [Authorize(Policy = "OrganizerOnly")]
        [HttpGet("my-event-users")]
        public IActionResult GetUsersByOrganizer()
        {
            var organizerId = GetUserIdFromToken();
            if (organizerId == null) return Forbid();

            var users = _userService.GetUsersByOrganizer(organizerId.Value);
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
