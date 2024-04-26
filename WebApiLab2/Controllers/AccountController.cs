using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiLab2.DTOs;

namespace WebApiLab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        public IActionResult Login(UserDto userDto)
        {
            // checking the user credentials
            if(userDto.Username=="Mohamed" && userDto.Password=="123456")
            {
                var token = GenerateToken();
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }

        }
        private string GenerateToken()
        {
            //JwtSecurityTokenHandler creates and validates JWTs.
            var tokenHandler = new JwtSecurityTokenHandler();

            // parsing the private key to bytes
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            // Create a SecurityTokenDescriptor which defines the parameters of the JWT.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Subject represents the claims about the user.
                Subject = new ClaimsIdentity(new Claim[]
                {
            // Add a claim with the user's name.
            new Claim(ClaimTypes.Name, "Mohamed Hamed"),

            // Add a claim with the user's role, in this case, "Admin".
            new Claim(ClaimTypes.Role, "Admin")
                }),

                // Set the expiration time for the token, in this case, 1 hour from now.
                Expires = DateTime.UtcNow.AddHours(1),

                // Set the signing credentials using a symmetric key (HMAC).
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Create a JWT using the tokenHandler and the tokenDescriptor.
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Write the JWT to a string representation.
            return tokenHandler.WriteToken(token);
        }

        [HttpGet("AnyAction")]
        [Authorize]
        public IActionResult AnyAction()
        {
            return Ok("Authorized");
        }
    }
}
