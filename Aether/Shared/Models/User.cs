using CsvHelper.Configuration;
using System;
using System.Collections.Generic;

namespace Aether.Shared.Models;

public partial class User
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public bool? Enabled { get; set; }

    public string? Role { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<BudgetDatum> BudgetData { get; set; } = new List<BudgetDatum>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

public sealed class UserMap : ClassMap<User>
{
    public UserMap()
    {
        Map(m => m.Enabled).Ignore();
        Map(m => m.Role).Ignore();
    }

}
