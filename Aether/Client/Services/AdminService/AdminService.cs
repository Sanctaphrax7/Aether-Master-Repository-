using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace Aether.Client.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;
        public AdminService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }

        public List<User> Users { get; set; } = new List<User>();
        public List<Role> Roles { get; set; }= new List<Role>();
        public async Task GetUserList()
        {
            var response = await _http.GetFromJsonAsync<List<User>>("api/admin/users");
            if (response != null)
                Users = response;
        }
      
        public async Task GetRoleList()
        {
            var response = await _http.GetFromJsonAsync<List<Role>>("api/admin/roles");
            if (response != null) 
                Roles = response;
        }

        public async Task<HttpResponseMessage> UpdateUser(User user)
        {
            var response = await _http.PutAsJsonAsync("api/admin/update", user);
            return response;
        }

        public async Task<HttpResponseMessage> AddUser(User user)
        {
            var response = await _http.PostAsJsonAsync("api/admin/create", user);
            _navigationManager.NavigateTo("Admin");
            return response;
        }

        //public async Task SetUser(HttpResponseMessage response)
        //{
        //    var message = await response.Content.ReadFromJsonAsync<List<User>>();
        //    Users = message;
        //}
    }
}
