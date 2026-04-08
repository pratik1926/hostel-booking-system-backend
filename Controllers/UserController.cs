using FYP_Backend.Data;
using FYP_Backend.Model;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HostelDbContext _context;

        public UserController(HostelDbContext context)
        {
            _context = context;
        }

        // POST: api/User/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register(User user)
        {
            // Check if email already exists
            if (_context.Users.Any(u => u.EmailAddress == user.EmailAddress))
            {
                return BadRequest("Email is already in use.");
            }

            // Store the password in plain text (not recommended for production)
            // user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully." });
        }

        // POST: api/User/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var user = _context.Users.FirstOrDefault(u => u.EmailAddress == login.EmailAddress);
            if (user == null || login.Password != user.Password) // Compare plain text passwords
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new
            {
                message = "Login successful.",
                user = new
                {
                    user.UserID,
                    user.FullName,
                    user.EmailAddress,
                    user.PermanentAddress
                }
            });
        }
    }

    // LoginModel class to accept login details
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
