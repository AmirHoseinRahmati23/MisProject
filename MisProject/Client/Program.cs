global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using MisProject.Client;
global using MudBlazor;
global using MudBlazor.Services;

global using ClientLibraries.HttpCallers;

using Blazored.LocalStorage;
using ClientLibraries.Security;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

#region ComponentServices

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices(p =>
{
    p.SnackbarConfiguration.VisibleStateDuration = 3000;
    p.SnackbarConfiguration.HideTransitionDuration = 200;
    p.SnackbarConfiguration.ShowTransitionDuration = 200;

    p.SnackbarConfiguration.ClearAfterNavigation = false;
    p.SnackbarConfiguration.MaxDisplayedSnackbars = 3;
    p.SnackbarConfiguration.NewestOnTop = false;
    p.SnackbarConfiguration.PositionClass = "Top-End";
    p.SnackbarConfiguration.PreventDuplicates = true;
    p.SnackbarConfiguration.BackgroundBlurred = true;
    p.SnackbarConfiguration.ShowCloseIcon = true;
    p.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

#endregion

#region Clients

builder.Services.AddTransient<CustomAuthorizationHandler>();
builder.Services.AddHttpClient<IUserCaller, UserCaller>("Client", c =>
{
    c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
}).AddHttpMessageHandler<CustomAuthorizationHandler>();

#endregion

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();
