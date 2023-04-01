using YZPortal.Client.Services.Authentication;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using YZPortal.Client;
using YZPortal.Client.Clients.YZPortalApi;
using YZPortal.Client.Services.LocalStorage;
using YZPortal.Client.Pages.User;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var configuration = builder.Configuration;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

#region Services

builder.Services.AddLocalStorageService();

builder.Services.AddAuthentication();

builder.Services.AddYZPortalApi(configuration);

//builder.Services.AddScoped<UserDealerSelectionState>();

#endregion

await builder.Build().RunAsync();
