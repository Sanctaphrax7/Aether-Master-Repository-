using CsvHelper.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace Aether.Shared.Models;

public partial class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string UserName { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public string Role { get; set; } = string.Empty;

    public virtual ICollection<BudgetDatum> BudgetData { get; set; } = new List<BudgetDatum>();

    public List<string> Roles { get; set; } = new();

    public ClaimsPrincipal ToClaimsPrincipal() => new(new ClaimsIdentity(new Claim[]
    {
        new(ClaimTypes.Name, UserName),
        new(ClaimTypes.Role, Role)
    }.Concat(Roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray()), "Claims"));

    public static User FromClaimsPrinciple(ClaimsPrincipal principal) => new()
    {
        UserName = principal.FindFirst(ClaimTypes.Name)?.Value ?? "",
        Role = principal.FindFirst(ClaimTypes.Role)?.Value ?? "",
        Roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList()
    };

}

public sealed class UserMap : ClassMap<User>
{
    public UserMap()
    {
        Map(m => m.Enabled).Ignore();
        Map(m => m.Role).Ignore();
    }

}

