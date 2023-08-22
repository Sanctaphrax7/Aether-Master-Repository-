using Aether.Shared.Models;

namespace Aether.Client.Services.BudgetViewService
{
    public interface IViewBudgetService
    {
        List<BudgetDatum> Budgets { get; set; }
        List<User> Users { get; set; }
        Task GetUsers();
        Task GetBudget();
        Task<BudgetDatum> GetThisBudget(int id);
        Task CreateBudget(BudgetDatum budget);
        Task UpdateBudget(BudgetDatum budget);
        Task DeleteBudget(int id);

       
    }
}
