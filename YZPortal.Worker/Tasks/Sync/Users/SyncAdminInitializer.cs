using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.Worker.Infrastructure.ScheduledTasks;

namespace YZPortal.Worker.Tasks.Sync.Users
{
    public class SyncAdminInitializer : SyncTask
    {
        public SyncAdminInitializer(IServiceScopeFactory serviceScopeFactory, IOptions<ScheduledTasksOptions> options) : base(serviceScopeFactory, options)
        {
        }

        public override async Task ProcessInScope(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var dbContext = serviceProvider.GetRequiredService<PortalContext>();
            var mapper = serviceProvider.GetRequiredService<IMapper>();
            var logger = serviceProvider.GetRequiredService<ILogger<SyncAdminInitializer>>();
            var usermanager = serviceProvider.GetRequiredService<UserManager<User>>();
            var option = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>();

            dbContext.Database.SetCommandTimeout(0);
            await SyncAdminMembershipAsync(dbContext, usermanager, mapper, logger, option.Value, cancellationToken);
        }

        public async Task SyncAdminMembershipAsync(PortalContext dbContext, UserManager<User> userManager, IMapper mapper, ILogger<SyncAdminInitializer> logger, DatabaseOptions options, CancellationToken cancellationToken)
        {
            logger.LogInformation("Started checking/creating Admin membership...");

            try
            {
                //if (string.IsNullOrEmpty(options.AdminEmail) || string.IsNullOrEmpty(options.AdminPassword))
                //    throw new ArgumentException("Admin email/password cannot be null");

                //var Admin = userManager.FindByEmailAsync(options.AdminEmail).Result;
                //var dealerIds = Admin != null ? await dbContext.Dealers.Select(x => x.Id).ToListAsync() : new List<Guid>();
                //var adminMembershipDealerIds = dealerIds.Any() ? await dbContext.Memberships.Where(x => x.UserId == Admin.Id && x.DealerId != Guid.Empty).Select(x => x.DealerId).ToListAsync(cancellationToken) : new List<Guid>();
                //var adminMissingMembershipDealerIds = dealerIds.Any() ? dealerIds.Except(adminMembershipDealerIds) : new List<Guid>();

                //if (adminMissingMembershipDealerIds.Any())
                //{
                //    var dealers = await dbContext.Dealers.Where(x => adminMissingMembershipDealerIds.Contains(x.Id)).ToListAsync();
                //    foreach (var dealer in dealers)
                //    {
                //        dbContext.Memberships.Add(new Membership { DealerId = dealer.Id, UserId = Admin.Id, User = Admin, Admin = true }); ;
                //    }

                //    await dbContext.SaveChangesAsync();
                //}

                //logger.LogInformation("Saving users to database...");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while checking/creating Admin membership: {ex.Message}");
            }
        }
    }
}
