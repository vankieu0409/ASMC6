using ASMC6.Client;
using ASMC6.Client.Infrastructure.Authentication;
using ASMC6.Client.Infrastructure.Extensions.DependencyInjection;

using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddApplication();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001") });
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();
builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();
