using Hangfire;
using Hangfire.Dashboard;
using Infrastructure.Services.BackgroundJob;
using Microsoft.AspNetCore.Builder;

namespace Infrastructure.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions()
                {
                    Authorization = new IDashboardAuthorizationFilter[]
                    {
                        new HangFireDashboardAuthorizationFilter()
                    },
                    // To remove back link
                    AppPath = null
                })
                .RequireAuthorization("Hangfire");
            });

            return app;
        }

        public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app) => app.UseCors("CORS");
    }
}
