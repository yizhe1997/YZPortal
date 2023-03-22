using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using YZPortal.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var configuration = builder.Configuration;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//// YZPortal
//var conn = configuration.GetConnectionString("YZPortal");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://yzportalapi.azurewebsites.net") });

await builder.Build().RunAsync();
