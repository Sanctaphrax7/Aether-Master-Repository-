namespace Aether.Client.Services.ViewBudgetService
{
    public interface IViewBudgetService
    {
        List<BudgetDatum> Budgets { get; set; }
        List<User> Users { get; set; }
        Task GetUsers();
        Task GetBudget();
        Task<BudgetDatum> GetThisBudget(int id);
        //Task GetComment(int id); //TODO To Be Implemented
        Task CreateBudget(BudgetDatum budget);
        Task UpdateBudget(BudgetDatum budget);
        Task DeleteBudget(int id);

       
    }
}
