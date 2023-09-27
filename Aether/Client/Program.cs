global using Aether.Shared.Models;
global using Microsoft.AspNetCore.Components.Authorization;
global using Blazored.LocalStorage;
global using Blazored.SessionStorage;
using Aether.Client.Services.AuthStateProvider;
using Aether.Client;
using Aether.Client.Services.AuthService;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Aether.Client.Services.AccountService;
using Aether.Client.Services.AdminService;
using Aether.Client.Services.ViewBudgetService;
using MudBlazor.Services;
using Aether.Client.ClientModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<IViewBudgetService, ViewBudgetService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminService, AdminService>();
//builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<AppStateManager>();
builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>(p => p.GetRequiredService<AuthStateProvider>());
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();


await builder.Build().RunAsync();

