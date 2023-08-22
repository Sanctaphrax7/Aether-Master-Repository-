using System;
using System.Collections.Generic;

namespace Aether.Shared.Models;

public partial class BudgetDataTemp
{
    public int Id { get; set; }

    public string Division { get; set; } = null!;

    public string GlAccountNo { get; set; } = null!;

    public string GlDeptNo { get; set; } = null!;

    public string SubAccountNo { get; set; } = null!;

    public decimal? PerAmt { get; set; }

    public int CompanyNo { get; set; }

    public int FiscalYear { get; set; }

    public int? FiscalMonth { get; set; }

    public int CalMonth { get; set; }

    public int RevisionNo { get; set; }

    public DateTime? LastUpdated { get; set; }

    public int? UpdatedBy { get; set; }
}
