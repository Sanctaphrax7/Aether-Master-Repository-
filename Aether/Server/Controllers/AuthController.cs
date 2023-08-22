using Aether.Server.Data;
using Aether.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Aether.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
      
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
     
        
        public AuthController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto creds)
        {

            try
            {
                using (PrincipalContext principalContext = new(ContextType.Domain, "10.120.110.86"))
                {
                    if (principalContext.ValidateCredentials(creds.UserName, creds.Password))
                    {

                        if(!CheckUser(creds.UserName))
                        {
                            return BadRequest("You are not currently registered for this application. Contact IT Admin");
                        }

                        user = _context.Users.First(u => u.UserName == creds.UserName);
                        string token = CreateToken(user, creds);
                        Dictionary<string, object> response = new()
                        {
                            { "username", user.UserName },
                            { "token", token } 
                        };

                        return Ok(response);
                    }
                }
                return Unauthorized("Invalid Credentials");
            }
            catch (PrincipalOperationException ex)
            {
                return BadRequest("Message:" + ex.Message + "Trace: " + ex.StackTrace);
            }
            catch (Exception ex)
            {
                return BadRequest("Message:" + ex.Message + "Trace: " + ex.StackTrace);
            }

        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            throw new NotImplementedException();
        }

        private bool CheckUser(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName); 
        }


        private string CreateToken(User user, UserDto creds)
        {
            //user = _context.Users.First(u => u.UserName == creds.UserName);
           
                List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, creds.UserName),//May be Redundant
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
