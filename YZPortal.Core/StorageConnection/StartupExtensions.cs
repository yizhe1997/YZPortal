using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YZPortal.Core.StorageConnection
{
    public static class StartupExtensions
    {
        public static void AddStorageConnectionStrings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StorageConnectionOptions>(configuration.GetSection("ConnectionStrings"));
        }
    }
}
