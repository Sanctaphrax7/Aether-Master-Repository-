using Aether.Server.Data;
using Aether.Shared.Models;

namespace Aether.Server.Authentication
{
    public class UserAccountService
    {
        private readonly DataContext _context;
        //private List<User> _userAccountList = new();

        public UserAccountService(DataContext context)
        {
            _context = context;
        }

        public User? GetUserAccountByUserName(string userName)
        {
             var userAccountList = _context.Users.FirstOrDefault(x => x.UserName == userName);

             return userAccountList; 
        }
    }
}
