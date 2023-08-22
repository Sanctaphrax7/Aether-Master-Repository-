using Aether.Shared.Models;
using Microsoft.EntityFrameworkCore;
namespace Aether.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<BudgetDatum> BudgetData{ get; set; } 
        public DbSet<User> Users { get; set; }

    }
}
