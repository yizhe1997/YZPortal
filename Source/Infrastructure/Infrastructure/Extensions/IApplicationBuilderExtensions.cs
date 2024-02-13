using Hangfire;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHangfire(this IApplicationBuilder app, IConfiguration configuration)
        {
            var azureAdB2CManagementConfig = configuration.GetSection("AzureAdB2CManagement").Get<AzureAdB2CManagementConfig>() ?? new();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions()
                {
                    IgnoreAntiforgeryToken = true,
                    // To remove back link
                    AppPath = null
                }).RequireAuthorization("Hangfire");
            });

            return app;
        }

        public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app) => app.UseCors("CORS");
    }
}
