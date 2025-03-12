using FoodDeliveryAPI.Models;
using FoodDeliveryAPI.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserServices _userService;
        private readonly IConfiguration _config;

        public AuthController(UserServices userService, IConfiguration config)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService)); ;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var existingUser = await _userService.GetUserByEmail(user.Email);
            if (existingUser != null)
                return BadRequest(new { message = "Email already in use." });

            await _userService.CreateUser(user);
            return Ok(new { message = "User registered successfully." });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _userService.AuthenticateUser(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] User updatedUser)
        {
            var success = await _userService.UpdateUser(id, updatedUser);
            if (!success)
                return NotFound(new { message = "User not found or no changes made." });

            return Ok(new { message = "User updated successfully." });
        }

        private string GenerateJwtToken(User user)
        {
            var secretKey = _config["JwtSettings:SecretKey"]
                ?? throw new ArgumentNullException("JwtSettings:SecretKey is missing in configuration.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? throw new ArgumentNullException("User ID cannot be null.")),
                new Claim(ClaimTypes.Name, user.Username ?? "Unknown"),
                new Claim(ClaimTypes.Email, user.Email ?? "no-email@example.com"),
                new Claim(ClaimTypes.Role, user.Role ?? "user")
            };

            var issuer = _config["JwtSettings:Issuer"] ?? throw new ArgumentNullException("Issuer is missing.");
            var audience = _config["JwtSettings:Audience"] ?? throw new ArgumentNullException("Audience is missing.");

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
