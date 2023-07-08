using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YZPortal.Core.Domain.Contexts
{
    public static class StartupExtensions
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Db Context
            var conn = configuration.GetConnectionString("Primary");
            services.AddDbContext<PortalContext>(options =>
            {
                options.UseSqlServer(conn);
                options.EnableSensitiveDataLogging(true);
            });
        }

        public static void AddCurrentContext(this IServiceCollection services)
        {
            // Current Context
            services.AddTransient<CurrentContext>();
        }
    }
}
