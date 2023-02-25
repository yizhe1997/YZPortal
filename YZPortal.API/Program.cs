using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using SixLabors.ImageSharp.Web.DependencyInjection;
using YZPortal.API.Infrastructure.AzureStorage;
using YZPortal.API.Infrastructure.Mediatr;
using YZPortal.API.Infrastructure.Mvc;
using YZPortal.API.Infrastructure.Security.Authentication;
using YZPortal.API.Infrastructure.Security.Authorization;
using YZPortal.API.Infrastructure.Security.AzureAd;
using YZPortal.API.Infrastructure.Security.AzureAdB2C;
using YZPortal.API.Infrastructure.Security.Jwt;
using YZPortal.API.Infrastructure.Swagger;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.Core.Error;
using YZPortal.Core.StorageConnection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var logger = new LoggerConfiguration()
					   .ReadFrom.Configuration(builder.Configuration)
					   .Enrich.FromLogContext()
					   .CreateLogger();

#region Add services to container

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
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.FullName.StartsWith("Microsoft.VisualStudio.TraceDataCollector", StringComparison.Ordinal)).ToArray());

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.FullName.StartsWith("Microsoft.VisualStudio.TraceDataCollector", StringComparison.Ordinal)));

// EFCore
var conn = configuration.GetConnectionString("Primary");
builder.Services.AddDbContext<PortalContext>(options =>
{
    options.UseSqlServer(conn);
    options.EnableSensitiveDataLogging(true);
});

// Seeding Db
builder.Services.AddDatabaseService(configuration);

// Current Context
builder.Services.AddTransient<CurrentContext>();

// Prereq for Iidentity... and to use usermanager
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<PortalContext>()
    .AddDefaultTokenProviders();

// Jwt            
builder.Services.AddJwt(configuration);

// AzureAd            
builder.Services.AddAzureAd(configuration);

// AzureAdB2C            
builder.Services.AddAzureAdB2C(configuration);

// Swagger             
builder.Services.AddSwaggerOption(configuration);

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler();

Log.Information("Initializing..."); // not sure why not working need to work on this

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
