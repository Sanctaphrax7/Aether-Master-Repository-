using System;
using System.Collections.Generic;

namespace Aether.Shared.Models;

public partial class Comic
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<SuperHero> SuperHeroes { get; set; } = new List<SuperHero>();
}
