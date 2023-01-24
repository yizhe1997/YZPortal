using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YZPortal.Core.Domain.Database
{
    public static class StartupExtensions
    {
        public static void AddDatabaseService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SeedOptions>(configuration.GetSection("Seed"));
            services.AddTransient<DatabaseService>();
        }

        public static void UseDatabaseService(this IServiceProvider provider)
        {
            var service = provider.GetRequiredService<DatabaseService>();
            service.UserAdmin();
            service.EnumValues();
            service.SyncStatuses();
        }
    }
}
