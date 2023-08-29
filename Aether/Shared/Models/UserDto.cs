using Aether.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Aether.Shared.Models;

public partial class UserDto
{ 
    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;

    //TEST
   
}

