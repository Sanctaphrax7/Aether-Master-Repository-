using Aether.Client.Services.AccountService;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.Design;
using System.Net.Http.Json;

namespace Aether.Client.Services.AccountService
{
    public interface IAccountService
    {

        User User { get; }
        Task Initialize(string UserName);
        Task Login(User user);

        Task Logout(User user);
    }
}

public class AccountService : IAccountService
{
    private readonly NavigationManager _navigationManager;
    private readonly HttpClient _http;
    public User User { get; set; }

    public async Task Initialize(string UserName)
    {
        var result = await _http.GetFromJsonAsync<User>($"api/Auth/user/{UserName}");
        if (result != null)
            User = result;
    }

    public Task Login(User user)
    {
      throw new NotImplementedException();
    }

    public Task Logout(User user) 
    {  
        throw new NotImplementedException(); 
    }
}