using Microsoft.EntityFrameworkCore;
using Serilog;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.Worker.Infrastructure.Email;
using YZPortal.Worker.Infrastructure.Email.OfficeSmtp;
using YZPortal.Worker.Infrastructure.Email.SendGrid;
using YZPortal.Worker.Infrastructure.ScheduledTasks;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

#region Add services to the container.

// The ConfigureServices method is used to configure the application's services,
// which are components that can be used throughout the application to provide functionality.
// In this method, you can register and configure services such as databases, authentication,
// authorization, logging, and more. These services are typically registered with the dependency injection (DI) container,
// which makes them available to other parts of the application.

Log.Information("Configuring and registering services to DI container...");

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.FullName.StartsWith("Microsoft.VisualStudio.TraceDataCollector", StringComparison.Ordinal)));

// Email notifications
builder.Services.AddEmailNotifications(configuration);
builder.Services.AddSendGridNotifications(configuration);
builder.Services.AddOfficeSmtpNotifications(configuration);

// EFCore
var conn = configuration.GetConnectionString("Primary");
builder.Services.AddDbContext<PortalContext>(options =>
{
	options.UseSqlServer(conn);
	options.EnableSensitiveDataLogging(true);
});

// Current Context
builder.Services.AddTransient<CurrentContext>();

// Prereq for Iidentity... and to use usermanager (not meant for ef core migration)
builder.Services.AddIdentityCore<User>()
		.AddEntityFrameworkStores<PortalContext>();

// Scheduled Tasks
builder.Services.AddScheduledTasks(configuration);

// Seeding Db
builder.Services.AddDatabaseService(configuration);

#endregion

var app = builder.Build();

#region WebApp MiddleWare

// The Configure method is used to configure the application's request pipeline,
// which is the sequence of middleware components that process incoming HTTP requests
// and generate outgoing responses. In this method, you can add middleware components
// to the pipeline, such as authentication middleware, routing middleware, error handling middleware,
// and more. Middleware components are executed in the order in which they are added to the pipeline.

Log.Information("Configuring request pipeline via middleware...");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseDatabaseService();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

#endregion

app.Run();
