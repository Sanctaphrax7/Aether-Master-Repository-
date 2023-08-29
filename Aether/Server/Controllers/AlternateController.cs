using Aether.Server.Data;
using Aether.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Aether.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlternateController : ControllerBase
    {
        public static User user = new User();

        private readonly IConfiguration _configuration;
        private readonly DataContext _context;


        public AlternateController(DataContext context, IConfiguration configuration)
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


                        if (!CheckUser(creds.UserName))
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

        private bool CheckUser(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        private string CreateToken(User user, UserDto creds)
        {

            var key = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            } 

            var signingKey = new SymmetricSecurityKey(key);

            var cred = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);


            return jwt;
        }


    }
}
