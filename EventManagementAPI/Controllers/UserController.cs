using EventManagementSystem_Merged_.DTO_s;
using EventManagementSystem_Merged_.Repos;
using EventManagementSystemMerged.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EventManagementAPI.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userBLL = new UserService();

        // GET: api/User
        [HttpGet]
        [Route("api/users")]
        public ActionResult<IEnumerable<object>> GetAllUsers()
        {
            var users = _userBLL.GetAllUsers();
            List<object> userList = new List<object>();

            foreach (DataRow row in users.Rows)
            {
                userList.Add(new
                {
                    UserID = row["UserID"],
                    Name = row["Name"],
                    Email = row["Email"],
                    Password = row["Password"],
                    ContactNumber = row["ContactNumber"],
                    UserType = row["UserType"],
                    IsDelete = row["IsDelete"]
                });
            }

            return Ok(userList);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public ActionResult<object> GetUser(int id)
        {
            var user = _userBLL.GetUserById(id);
            if (user.Rows.Count > 0)
            {
                DataRow row = user.Rows[0];
                var userObj = new
                {
                    UserID = row["UserID"],
                    Name = row["Name"],
                    Email = row["Email"],
                    Password = row["Password"],
                    ContactNumber = row["ContactNumber"],
                    UserType = row["UserType"],
                    IsDelete = row["IsDelete"]
                };
                return Ok(userObj);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/User
        [HttpPost]
        [Route("api/users")]
        public ActionResult<object> CreateUser([FromBody] UserDTO user)
        {
            _userBLL.AddUser(user.Name, user.Email, user.Password, user.ContactNumber, user.UserType);
            return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
        }


        // PUT: api/User/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserDTO user)
        {
            if (id != user.UserID)
            {
                return BadRequest();
            }

            _userBLL.UpdateUser(id, user.Name, user.Email, user.Password, user.ContactNumber, user.UserType);
            return NoContent();
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            _userBLL.DeleteUser(id);
            return NoContent();
        }
    }
}
