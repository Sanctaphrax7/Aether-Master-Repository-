using System.ComponentModel.DataAnnotations;

namespace Aether.Server.Authentication
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public bool Enabled { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
