using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;
using BL.Api;
using DAL.Models;
using iText.StyledXmlParser.Jsoup.Parser;
using BL.Models;
using RegisterRequest = BL.Models.RegisterRequest;


namespace server.Controllers
{
    [ApiController]
    [Route("DosFlix/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IBL _context;

        public UsersController(IBL context)
        {
            _context = context;
        }
        
        private JwtSecurityToken GetToken(BLUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName ?? ""),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKeyThatIsAtLeast128BitsLong!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "jwt-app",
                audience: "jwt-users",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return token;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _context.User.Login(request.Email, request.Password);
                if (user == null)
                    return Unauthorized("User not found or ID number incorrect");

                var token = GetToken(user);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new
                {
                    token = tokenString,
                    username = user.FullName, 
                    role = user.Role
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = await _context.User.Register(request); 

                var token = GetToken(user);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new
                {
                    token = tokenString,
                    username = user.FullName,
                    role = user.Role
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }

}
