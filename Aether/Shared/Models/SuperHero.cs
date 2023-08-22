using System;
using System.Collections.Generic;

namespace Aether.Shared.Models;

public partial class SuperHero
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string HeroName { get; set; } = null!;

    public int ComicId { get; set; }

    public virtual Comic Comic { get; set; } = null!;
}
