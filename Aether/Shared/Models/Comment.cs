using System;
using System.Collections.Generic;

namespace Aether.Shared.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? UploadComment { get; set; }

    public DateTime TimeStamp { get; set; }

    public virtual ICollection<BudgetDatum> BudgetData { get; set; } = new List<BudgetDatum>();

    public virtual User User { get; set; } = null!;
}
