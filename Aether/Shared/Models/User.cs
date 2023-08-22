using CsvHelper.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

   
}

public sealed class UserMap : ClassMap<User>
{
    public UserMap()
    {
        Map(m => m.Enabled).Ignore();
        Map(m => m.Role).Ignore();
    }

}

