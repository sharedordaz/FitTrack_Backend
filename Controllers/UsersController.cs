using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly FitTrackContext _context;

        public UsersController(FitTrackContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        [HttpGet("check-username/{username}")]
        public async Task<ActionResult> CheckUsernameAvailable(string username)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Username == username);
            if (userExists)
            {
                return Conflict(new { message = "Username already exists" });
            }
            return Ok(new { message = "Username is available" });
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            // Check if username already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if (existingUser != null)
            {
                return Conflict(new { message = "Username already exists" });
            }

            // Hash the password before saving
            user.Password = HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Don't return the password in the response
            var responseUser = new { user.Id, user.Username };
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, responseUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                // Find user by username
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);
                if (user == null)
                {
                    return Unauthorized(new { success = false, message = "Invalid username or password" });
                }

                // Verify password
                var hashedPassword = HashPassword(loginRequest.Password);
                if (user.Password != hashedPassword)
                {
                    return Unauthorized(new { success = false, message = "Invalid username or password" });
                }

                // Return success response with user info (without password)
                return Ok(new 
                { 
                    success = true, 
                    message = "Login successful",
                    user = new { user.Id, user.Username }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Login failed" });
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}