using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Aether.Shared.Models;
using Microsoft.IdentityModel.Tokens;
using Aether.Client.Services.AuthStateProvider;

namespace Aether.Server.Authentication
{
    public class JwtAuthManager
    {
        public const string JwtSecurityKey = "HFaezx41W8rw5u9joTazZPGWLcq9DeKoJqe6l5Fs63gZ9saYhkhfdUaAZneCWxdc7cFusSjM5mbvYoxdO6F69TcTnSJ7h9T9Vtoik9ziiYbkPtR4FolU30tglr0Tg4aCJzi45QXyGfU8j5u2GJSQ0DnZOYolWsXi";
        private const int JwtValidityTime = 20;
        private UserAccountService _userAccountService;
        


        public JwtAuthManager(UserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        public UserSession? GenerateJwtToken(string userName)
        {
            if(string.IsNullOrEmpty(userName)) 
                return null;

            var userAccount = _userAccountService.GetUserAccountByUserName(userName);
            if (userAccount == null) 
                return null;

          

            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JwtValidityTime);
            var key = Encoding.ASCII.GetBytes(JwtSecurityKey);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, userAccount.UserName),
                new Claim(ClaimTypes.NameIdentifier, userAccount.Id.ToString()),
                new Claim(ClaimTypes.GivenName, userAccount.Name),//TODO Name field added(AUTH MANAGER1)
                new Claim(ClaimTypes.Role, userAccount.Role)
            });
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                NotBefore = DateTime.Now,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = creds
            };
            var jwt = new JwtSecurityTokenHandler();
            var securityToken = jwt.CreateToken(tokenDescriptor);
            var token = jwt.WriteToken(securityToken);

            var userSession = new UserSession
            {
                UserName = userAccount.UserName,
                Name = userAccount.Name,//TODO Name field added(AUTH MANAGER2)
                Role = userAccount.Role,
                Id = userAccount.Id,
                Token = token,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds
            };
            return userSession;
        }
    } 
}
