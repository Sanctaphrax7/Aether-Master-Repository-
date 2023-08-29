global using Aether.Client.Services.BudgetViewService;
global using Aether.Shared.Models;
global using Microsoft.AspNetCore.Components.Authorization;
global using Blazored.LocalStorage;
global using Blazored.SessionStorage;
global using Aether.Client.ClientModels;
using Aether.Client;
using Aether.Client.Services.AuthStateProvider;
using Aether.Client.Services.AuthService;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Aether.Client.Services.AccountService;
using Aether.Client.Services.UserService;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IViewBudgetService, ViewBudgetService>();
builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>(p => p.GetRequiredService<AuthStateProvider>());
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();


await builder.Build().RunAsync();

