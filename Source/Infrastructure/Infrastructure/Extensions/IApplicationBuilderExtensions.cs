using Hangfire;
using Hangfire.Dashboard;
using Infrastructure.Configurations;
using Infrastructure.Services.BackgroundJob;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHangfire(this IApplicationBuilder app, IConfiguration configuration)
        {
            var azureAdB2CManagementConfig = configuration.GetSection("AzureAdB2CManagement").Get<AzureAdB2CManagementConfig>() ?? new();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                DashboardTitle = "Scheduler",
                // To remove back link
                AppPath = null,
                Authorization = new IDashboardAuthorizationFilter[]
                    {
                        new DashboardOpenIdAuthorizationFilter(azureAdB2CManagementConfig)
                    }
            });

            // TODO: add another hangfire dashboard accessible from swagger which requires authentication

            return app;
        }

        public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app) => app.UseCors("CORS");
    }
}
