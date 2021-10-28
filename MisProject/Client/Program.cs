global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using MisProject.Client;
global using MudBlazor;
global using MudBlazor.Services;

global using ClientLibraries.HttpCallers;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
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

builder.Services.AddTransient<IUserCaller, UserCaller>();

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
