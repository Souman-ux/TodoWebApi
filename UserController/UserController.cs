using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoWebApi.Models;

namespace TodoWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private static List<User> users = new List<User>();
        private static HashSet<string> loggedInUsers = new HashSet<string>();

        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            if (users.Any(u => u.UserName == user.UserName))
            {
                return BadRequest("User already exists");
            }

            users.Add(user);
            return Ok("User added successfully");
        }

        [HttpGet]
        public IActionResult VerifyLogin(string username, string password)
        {
            var user = users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
                return NotFound("User not found");

            if (user.IsLocked)
                return Unauthorized("User account is locked.");

            if (user.Password == password)
            {
                loggedInUsers.Add(username);
                return Ok("Login successful");
            }

            return Unauthorized("Incorrect password");
        }

        [HttpPost("logout")]
        public IActionResult Logout(string username)
        {
            if (loggedInUsers.Contains(username))
            {
                loggedInUsers.Remove(username);
                return Ok("User logged out");
            }

            return NotFound("User is not logged in");
        }

        [HttpGet("status")]
        public IActionResult GetStatus(string username)
        {
            var user = users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
                return NotFound("User not found");

            return Ok(new
            {
                userName = user.UserName,
                isLoggedIn = loggedInUsers.Contains(username)
            });
        }

        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            var result = users.Select(u => new
            {
                u.UserName,
                u.IsLocked
            });

            return Ok(result);
        }

        [HttpPost("lock")]
        public IActionResult LockUser(string username)
        {
            var user = users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
                return NotFound("User not found");

            user.IsLocked = true;
            loggedInUsers.Remove(username);

            return Ok("User locked");
        }

        [HttpPost("unlock")]
        public IActionResult UnlockUser(string username)
        {
            var user = users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
                return NotFound("User not found");

            user.IsLocked = false;
            return Ok("User unlocked");
        }
    }
}
