using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.Core.Domain.Database.Memberships;
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
                if (string.IsNullOrEmpty(options.AdminEmail) || string.IsNullOrEmpty(options.AdminPassword))
                    throw new ArgumentException("Admin email/password cannot be null");

                var Admin = userManager.FindByEmailAsync(options.AdminEmail).Result;
                string contextToken = Guid.Empty.ToString();
                var adminMembershipDealerGuids = await dbContext.Memberships.Where(x => x.UserId == Admin.Id && x.DealerId != Guid.Empty).Select(x => x.DealerId).ToListAsync(cancellationToken);

                // Periodically check missing membership for admin
                if (Admin != null && adminMembershipDealerGuids.Count != await dbContext.Dealers.CountAsync(cancellationToken))
                {
                    var dealerGuids = await dbContext.Dealers.Select(x => x.Id).ToListAsync(cancellationToken);

                    var missingAdminMembershipDealerGuids = dealerGuids.Where(d => !adminMembershipDealerGuids.Contains(d)).ToList();

                    if (missingAdminMembershipDealerGuids.Any())
                        foreach (var guid in missingAdminMembershipDealerGuids)
                            dbContext.Memberships.Add(new Membership { DealerId = guid, UserId = Admin.Id, User = Admin});

                    if ((missingAdminMembershipDealerGuids?.Count() ?? 0) + adminMembershipDealerGuids.Count() != dealerGuids.Count)
                        logger.LogError("Mismatch between number of admin membership(s) and number of existing dealer(s)! Consider deleting admin membership(s) which are duplicate or do not have existing dealer.");

                    await dbContext.SaveChangesAsync();
                }

                logger.LogInformation("Saving users to database...");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while checking/creating Admin membership: {ex.Message}");
            }
        }
    }
}
