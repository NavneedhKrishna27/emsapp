
using EventManagement_Merged_.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
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
        //[Authorize]
        //[HttpGet("{id}")]
        //public IActionResult GetUserById(int id)
        //{
        //    var user = _userService.GetUserById(id);
        //    if (user == null) return NotFound();
        //    return Ok(user);
        //}

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

        [Authorize(Policy="UserOnly")]
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var userIdFromToken = GetUserIdFromToken();
            if (userIdFromToken != id) return Forbid();

            var user = _userService.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }


        [Authorize(Policy = "UserOnly")]
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            bool success = _userService.UpdateUser(id, userDto.Name, userDto.ContactNumber, userDto.UserType);
            if (!success) return NotFound();
            return Ok(new { message = "User updated successfully." });
        }

        [Authorize(Roles="Admin")]
        [HttpGet("type/{userType}")]
        public IActionResult GetUsersByType(string userType)
        {
            var users = _userService.GetUsersByType(userType);
            return Ok(users);
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

        public class UpdateUserDto
        {
            public string Name { get; set; }
            public string ContactNumber { get; set; }
            public string UserType { get; set; }
        }


    }
}