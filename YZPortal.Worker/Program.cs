using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.Worker.Infrastructure.Email;
using YZPortal.Worker.Infrastructure.Email.OfficeSmtp;
using YZPortal.Worker.Infrastructure.Email.SendGrid;
using YZPortal.Worker.Infrastructure.RazorLight;
using YZPortal.Worker.Infrastructure.ScheduledTasks;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

#region Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.FullName.StartsWith("Microsoft.VisualStudio.TraceDataCollector", StringComparison.Ordinal)));

// Email notifications
builder.Services.AddEmailNotifications(configuration);
builder.Services.AddSendGridNotifications(configuration);
builder.Services.AddOfficeSmtpNotifications(configuration);
builder.Services.AddEmailTemplates();

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

app.Run();
