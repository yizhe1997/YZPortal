using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class IServiceProviderExtensions
    {
        public static async Task MigrateDatabaseAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
        {
            using var scope = services.CreateScope();

            await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync(cancellationToken);
        }
    }
}
