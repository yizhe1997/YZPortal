using Microsoft.AspNetCore.Builder;
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

        public static async Task UseDatabaseServiceAsync(this WebApplication app, CancellationToken cancellationToken = new CancellationToken())
        {
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;

				// Make sure the latest EFCore migration is applied everytime the API is initiated
				var dbContext = services.GetRequiredService<PortalContext>();
                await dbContext.Database.MigrateAsync(cancellationToken);

                // Applied before migration so that DatabaseService transaction can take place for new/existing DB.
                // Make sure to arrange the database services sequentially as chronology affects the functionality
                var service = services.GetRequiredService<DatabaseService>();
                await service.UserSeedConfigsAsync(cancellationToken);
            }
        }
    }
}
