using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using Serilog;
using SixLabors.ImageSharp.Web.DependencyInjection;
using System.Reflection;
using YZPortal.API.Infrastructure.AzureStorage;
using YZPortal.API.Infrastructure.Mediatr.PipelineBehavior;
using YZPortal.API.Infrastructure.Mvc;
using YZPortal.API.Infrastructure.Security.Authentication;
using YZPortal.API.Infrastructure.Security.Authentication.BasicAuthentication;
using YZPortal.API.Infrastructure.Security.Authorization;
using YZPortal.API.Infrastructure.Security.AzureAdB2C;
using YZPortal.API.Infrastructure.Security.Jwt;
using YZPortal.API.Infrastructure.Swagger;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.Core.Error;
using YZPortal.Core.Graph;
using YZPortal.Core.StorageConnection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Ref: https://www.codeproject.com/Articles/5344667/Logging-with-Serilog-in-ASP-NET-Core-Web-API#:~:text=Create%20ASP.NET%20Core%20Web%20API%20Project&text=Choose%20.,then%20choose%20to%20install%20Serilog.
var logger = new LoggerConfiguration()
					   .ReadFrom.Configuration(builder.Configuration)
					   .Enrich.FromLogContext()
                       .WriteTo.Console()
                       .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Ref: https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/PII
// https://stackoverflow.com/questions/54435551/invalidoperationexception-idx20803-unable-to-obtain-configuration-from-pii
IdentityModelEventSource.ShowPII = true;

#region Add services to container

// The ConfigureServices method is used to configure the application's services,
// which are components that can be used throughout the application to provide functionality.
// In this method, you can register and configure services such as databases, authentication,
// authorization, logging, and more. These services are typically registered with the dependency injection (DI) container,
// which makes them available to other parts of the application.

logger.Information("Configuring and registering services to DI container...");

// Serilog
builder.Host.UseSerilog(logger);

// Add cross-origin resource sharing to IServiceCollection
builder.Services.AddCors();

builder.Services.AddMvc(opts =>
{
    opts.Filters.Add(typeof(ValidatorActionFilter));
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressConsumesConstraintForFormFileParameters = true;
    options.SuppressInferBindingSourcesForParameters = true;
    options.SuppressModelStateInvalidFilter = true;
    options.SuppressMapClientErrors = true;
})
.AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

// Fluent Validation Ref: https://github.com/FluentValidation/FluentValidation/issues/1965
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>(includeInternalTypes: true);

// Api Versioning
builder.Services.AddApiVersioning(opts => { opts.AssumeDefaultVersionWhenUnspecified = true; opts.ReportApiVersions = true; opts.DefaultApiVersion = new ApiVersion(1, 0); });
builder.Services.AddVersionedApiExplorer(opts => { opts.GroupNameFormat = "'v'VV"; opts.SubstituteApiVersionInUrl = true; opts.DefaultApiVersion = new ApiVersion(1, 0); });

// MediatR
// https://github.com/jbogard/MediatR/wiki/Migration-Guide-11.x-to-12.0
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#region Context

// Db Context
builder.Services.AddDbContext(configuration);

// Current Context
builder.Services.AddCurrentContext();

#endregion

// Seeding Db
builder.Services.AddDatabaseService(configuration);

// Prereq Microsoft.AspNetCore.Identity and to use usermanager (not meant for ef core migration)
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<PortalContext>()
    .AddDefaultTokenProviders();

// Jwt            
builder.Services.AddJwt(configuration);

// Basic Auth            
builder.Services.AddBasicAuth(configuration);

// AzureAdB2C            
builder.Services.AddAzureAdB2C(configuration);

// Authentication
builder.Services.AddAuthentications(configuration);

// Authorization
builder.Services.AddAuthorizations(configuration);

// Image processing
builder.Services.AddImageSharp();

// Azure Blob Storage
builder.Services.AddAzureStorage(configuration);

// Storage Connection
builder.Services.AddStorageConnectionStrings(configuration);

// Swagger            
builder.Services.AddSwaggerDocumentation(configuration);

// Graph
builder.Services.AddGraph(configuration);

// Add pipeline behaviour (automatic validation and db entity auditing
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DbContextTransactionPipelineBehaviour<,>));

// Max Value Allowed Content Length
builder.Services.Configure<KestrelServerOptions>(options =>
{
    // Increase for file size limit. if don't set default value is: 30 MB
    options.Limits.MaxRequestBodySize = int.MaxValue;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

#region WebApp MiddleWare

// The Configure method is used to configure the application's request pipeline,
// which is the sequence of middleware components that process incoming HTTP requests
// and generate outgoing responses. In this method, you can add middleware components
// to the pipeline, such as authentication middleware, routing middleware, error handling middleware,
// and more. Middleware components are executed in the order in which they are added to the pipeline.

logger.Information("Configuring request pipeline via middleware...");

if (app.Environment.IsDevelopment())
{
}

app.ConfigureExceptionHandler();

app.UseSerilogRequestLogging();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseDatabaseService();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Make the default route redirect to Swagger documentation
var option = new RewriteOptions();
option.AddRedirect("^$", "docs");
app.UseRewriter(option);

// Expose Swagger documentation
app.UseSwaggerDocumentation(app.Services.GetRequiredService<IApiVersionDescriptionProvider>(), configuration);

app.MapControllers();

app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always,
    HttpOnly = HttpOnlyPolicy.Always
});

#endregion

app.Run();
