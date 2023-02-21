using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YZPortal.Core.Domain.Contexts;

namespace YZPortal.Core.Domain.Database
{
    public static class StartupExtensions
    {
        public static void AddDatabaseService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseOptions>(configuration.GetSection("Database"));
            services.AddTransient<DatabaseService>();
        }

        public static void UseDatabaseService(this IServiceProvider provider)
        {
			// Make sure the latest EFCore migration is applied everytime the API is initiated
			var dbContext = provider.GetRequiredService<PortalContext>();
			dbContext.Database.Migrate();

			// Applied before migration so that DatabaseService transaction can take place for new/existing DB.
			// Make sure to arrange the database service sequentially as chronology affects the functionality
			var service = provider.GetRequiredService<DatabaseService>();
            service.UserAdmin();
            service.EnumValues();
            service.SyncStatuses();
        }
    }
}
