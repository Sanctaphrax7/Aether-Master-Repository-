using Aether.Server.Data;
using Aether.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Aether.Server.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Aether.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
      
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private UserAccountService _userAccountService;
        
        public AuthController(DataContext context, IConfiguration configuration, UserAccountService userAccountService)
        {
            _context = context;
            _configuration = configuration;
            _userAccountService = userAccountService;
        }




        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<UserSession> Login([FromBody] UserDto creds)
        {
            try
            {
                using (PrincipalContext principalContext = new(ContextType.Domain, "10.120.110.86"))
                {
                    if (principalContext.ValidateCredentials(creds.UserName, creds.Password))
                    {
                        //if(!CheckUser(creds.UserName))
                        //{
                        //    return BadRequest("You are not currently registered for this application. Contact IT Admin");
                        //}
                        var jwtAuthManager = new JwtAuthManager(_userAccountService);
                        var userSession = jwtAuthManager.GenerateJwtToken(creds.UserName);
                        if (userSession is null)
                        {
                            return Unauthorized();
                        }
                        if (userSession.Enabled == false)
                        {
                            return Unauthorized();
                        }
                        else
                        {
                            return userSession;
                        }
                        //user = _context.Users.First(u => u.UserName == creds.UserName);
                        ////string token = CreateToken(user, creds);
                        //Dictionary<string, object> response = new()
                        //{
                        //    { "username", user.UserName },
                        //   // { "token", token } 
                        //};

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
        //private bool CheckUser(string userName)
        //{
        //    return _context.Users.Any(u => u.UserName == userName); 
        //}


        //private string CreateToken(User user, UserDto creds)
        //{
        //    //user = _context.Users.First(u => u.UserName == creds.UserName);
           
        //        List<Claim> claims = new List<Claim> {
        //        new Claim(ClaimTypes.Name, creds.UserName),//May be Redundant
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //        new Claim(ClaimTypes.Role, user.Role)
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
        //        _configuration.GetSection("AppSettings:Token").Value!));

        //    var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        //    var token = new JwtSecurityToken(
        //            claims: claims,
        //            expires: DateTime.Now.AddDays(1),
        //            signingCredentials: cred
        //        );

        //    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        //    return jwt;
        //}

    }
}
