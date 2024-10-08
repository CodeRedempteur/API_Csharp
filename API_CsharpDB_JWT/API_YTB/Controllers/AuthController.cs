using API_YTB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_YTB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration _configiguration;
        private readonly UserContext _context;

        public AuthController(IConfiguration configuration, UserContext context)
        {
            _configiguration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            var dbUser = _context.t_user.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            if (dbUser == null)
            {
                return Unauthorized();
            }
            var token = GenerateJSONWebToken(dbUser);
            return Ok(new { token });
        }


        private string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configiguration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };

            var token = new JwtSecurityToken(
                issuer: _configiguration["Jwt:Issuer"],
                audience: _configiguration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(180),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}
