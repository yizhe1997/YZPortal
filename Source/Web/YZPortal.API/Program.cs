using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Infrastructure.Extensions;
using Application.Extensions;
using YZPortal.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Host.UseSerilog(configuration);
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddAzureWebAppDiagnostics();


// Ref: https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/PII
// https://stackoverflow.com/questions/54435551/invalidoperationexception-idx20803-unable-to-obtain-configuration-from-pii
IdentityModelEventSource.ShowPII = true;

#region Add services to the container.

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(configuration);
builder.Services.AddWebLayer(configuration);

#endregion

var app = builder.Build();

await app.Services.MigrateDatabaseAsync();

app.UseInfrastructure();

//// Make the default route redirect to Swagger documentation
//var option = new RewriteOptions();
//option.AddRedirect("^$", "chat");
//app.UseRewriter(option);

// Expose Swagger documentation
app.UseSwagger(app.Services.GetRequiredService<IApiVersionDescriptionProvider>(), configuration);

app.MapEndpoints();

app.Run();

