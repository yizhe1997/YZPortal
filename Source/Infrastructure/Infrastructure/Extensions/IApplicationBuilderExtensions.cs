using Hangfire;
using Infrastructure.Services.BackgroundJob;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infrastructure.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        private static readonly string[] supportedCultures = ["en", "de"];

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IServiceCollection services) =>
            builder
                .UseExceptionHandler(o => { }) // https://github.com/dotnet/aspnetcore/issues/51888
                .UseSerilogRequestLogging()
                .UseCorsPolicy()
                .UseHsts()
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseRequestLocalization(options =>
                {
                    options.SetDefaultCulture(supportedCultures[0])
                        .AddSupportedCultures(supportedCultures)
                        .AddSupportedUICultures(supportedCultures);
                })
                .UseHangfireDashboard()
                .UseOpenTelemetryPrometheusScrapingEndpoint();

        private static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions()
                {
                    Authorization =
                    [
                        new HangFireDashboardAuthorizationFilter()
                    ],
                    // To remove back link
                    AppPath = null
                })
                .RequireAuthorization("Hangfire");
            });

            return app;
        }

        private static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app) => app.UseCors("CORS");
    }
}
