using CsvHelper.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Aether.Shared.Models;

public class BudgetDatum
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Division { get; set; } = null!;

    public string GlAccountNo { get; set; } = null!;

    public string GlDeptNo { get; set; } = null!;

    public string SubAccountNo { get; set; } = null!;

    public decimal? PerAmt { get; set; }

    public int CompanyNo { get; set; } = 1;

    public int FiscalYear { get; set; } = GetFiscalYear();

    public int? FiscalMonth { get; set; } = GetFiscalMonth();

    public int CalMonth { get; set; } = DateTime.Now.Month;

    public int RevisionNo { get; set; }

    public DateTime? LastUpdated { get; set; }

    public int? UserId { get; set; }

    public string? UpdatedBy { get; set; }

    public string? FileName { get; set; }
    public User? User { get; set; }

    private static int GetFiscalYear()
    {
        DateTime today = DateTime.Today;
        int firstMonth = 10;
        int fiscalYear;
        if (today.Month > firstMonth)
        {
            fiscalYear = today.Year + 1;
        }
        else
        {
            fiscalYear = today.Year;
        }
        return fiscalYear;
    }

    private static int? GetFiscalMonth()
    {
        DateTime today = DateTime.Today;
        int firstMonth = 10;
        int fiscalYear;
        if (today.Month >= firstMonth)
        {
            fiscalYear = today.Year + 1;
        }
        else
        {
            fiscalYear = today.Year;
        }
        int fiscalMonth = (today.Month + 13 - firstMonth % 13);
        if (fiscalMonth == 0)
        {
            fiscalMonth = 12;
        }
        return fiscalMonth;
    }

}


public sealed class BudgetDatumMap : ClassMap<BudgetDatum>
{
    public BudgetDatumMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.Id).Ignore();
        Map(m => m.LastUpdated).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.UserId).Ignore();
        Map(m => m.FileName).Ignore();
        //Map(m => m.User).Ignore();

    }
}
