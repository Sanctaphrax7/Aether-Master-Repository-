using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace Aether.Client.Services.ViewBudgetService
{
    public class ViewBudgetService : IViewBudgetService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;

        public ViewBudgetService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }
        public List<BudgetDatum> Budgets { get; set; } = new List<BudgetDatum>();
        public List<User> Users { get; set; } = new List<User> ();

        public async Task CreateBudget(BudgetDatum budget)
        {
            var result = await _http.PostAsJsonAsync("api/viewbudget", budget);
            await SetBudget(result);
        }

        private async Task SetBudget(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<BudgetDatum>>();
            Budgets = response;
            _navigationManager.NavigateTo("viewbudget");
        }
        public async Task DeleteBudget(int id)
        {
            var result = await _http.DeleteAsync($"api/viewbudget/{id}");
            await SetBudget(result);
        }
        public async Task GetUsers()
        {
            var result = await _http.GetFromJsonAsync<List<User>>("api/viewbudget/users");
            if (result != null)
               Users = result; //This was changed in testing Create Budget
        }
        public async Task<BudgetDatum> GetThisBudget(int id)
        {
            var result = await _http.GetFromJsonAsync<BudgetDatum>($"api/viewbudget/{id}");
            if (result != null)
                return result;
            throw new Exception("Budget Not Found");
        }
        public async Task GetBudget()
        {
            var result = await _http.GetFromJsonAsync<List<BudgetDatum>>("api/viewbudget");
            if (result != null)
                Budgets = result;
        }
        //public async Task GetComment() //TODO To Be Implemented 
        //{
        //    var result = await _http.GetFromJsonAsync<List<BudgetDatum>>("api/viewbudget/comment");
        //    if (result != null)
        //        await SetBudgetComment(result);
        //}
        public async Task UpdateBudget(BudgetDatum budget)
        {
            var result = await _http.PutAsJsonAsync($"api/viewbudget/{budget.Id}", budget);
            await SetBudget(result);
        }

       
    }
}
