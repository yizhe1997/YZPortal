using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using YZPortalV8.Client;
using YZPortalV8.Client.Extensions;
using YZPortalV8.Client.Services.Authorization;
using YZPortalV8.Client.Services.LocalStorage;
using YZPortalV8.Client.Clients.YZPortalApi;
using YZPortalV8.Client.Services.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var configuration = builder.Configuration;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

#region Services

// ref: https://code-maze.com/localization-in-blazor-webassembly-applications/
builder.Services.AddLocalization();

builder.Services.AddLocalStorageService();

builder.Services.AddAuthentication(configuration);

builder.Services.AddAuthorization();

builder.Services.AddYZPortalApi(configuration);

builder.Services.AddBootstrapBlazor();

#endregion

await builder.Build().SetDefaultCulture();

await builder.Build().RunAsync();
