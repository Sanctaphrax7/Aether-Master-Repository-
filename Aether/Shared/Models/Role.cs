using System;
using System.Collections.Generic;

namespace Aether.Shared.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Roles { get; set; } = null!;

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
