using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Infrastructure.Extensions;
using Application.Extensions;
using YZPortal.API.Extensions;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Host.UseSerilog(configuration);

// Ref: https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/PII
// https://stackoverflow.com/questions/54435551/invalidoperationexception-idx20803-unable-to-obtain-configuration-from-pii
IdentityModelEventSource.ShowPII = true;

#region Add services to the container.

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(configuration);
builder.Services.AddWebLayer(configuration);

#endregion

var app = builder.Build();

// TODO: clean this find a place to store
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Make sure the latest EFCore migration is applied everytime the API is initiated
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseMiddlewareExceptionHandler();

app.UseSerilogRequestLogging();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Make the default route redirect to Swagger documentation
var option = new RewriteOptions();
option.AddRedirect("^$", "docs");
app.UseRewriter(option);

// Expose Swagger documentation
app.UseSwagger(app.Services.GetRequiredService<IApiVersionDescriptionProvider>(), configuration);

app.MapControllers();

app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always,
    HttpOnly = HttpOnlyPolicy.Always
});

app.Run();

