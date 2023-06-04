using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YZPortal.Core.Graph
{
    public static class StartupExtensions
    {
        public static void AddGraph(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureAdB2CManagementOptions>(configuration.GetSection("AzureAdB2CManagement"));
            services.Configure<GraphOptions>(configuration.GetSection("Graph"));

            services.AddSingleton<GraphClientProvider>();
        }
    }
}
