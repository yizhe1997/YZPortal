using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Web.Helpers;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.Core.Domain.Database.Sync;
using YZPortal.Core.Domain.Contexts;
using YZPortal.FullStackCore.Enums.Memberships;
using YZPortal.FullStackCore.Extensions;

namespace YZPortal.Core.Domain.Database
{
    public class DatabaseService
    {
        private readonly PortalContext _dbContext;
        private readonly DatabaseOptions _options;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<DatabaseService> _logger;

        // Constructor
        public DatabaseService(PortalContext dbContext, UserManager<User> userManager, IOptions<DatabaseOptions> options, ILogger<DatabaseService> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _options = options.Value;
            _logger = logger;
        }

        #region Create and update admin user on startup

        public void UserAdmin()
        {
            // Check option for seeding admin
            if (string.IsNullOrEmpty(_options.AdminEmail) || string.IsNullOrEmpty(_options.AdminPassword))
                throw new ArgumentException("Admin email/password cannot be null");

            // Check if configured admin email exist
            var Admin = _userManager.FindByEmailAsync(_options.AdminEmail).Result;
            if (Admin == null)
            {
                // Check if theres pre-existing admin user, create new if no and overwrite if yes
                var AdminCheck = _userManager.Users.FirstOrDefault();
                if (AdminCheck == null)
                {
                    Admin = new User
                    {
                        Email = _options.AdminEmail,
                        DisplayName = "admin",
                        EmailConfirmed = true
                    };

                    var result = _userManager.CreateAsync(Admin, _options.AdminPassword).Result;

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Admin user created successfully.");
                    }
                    else
                    {
                        _logger.LogError("Failed to create Admin user: {0}", result.Errors.ToString());
                    }
                }
                else
                {
                    AdminCheck.Email = _options.AdminEmail;
                    AdminCheck.PasswordHash = Crypto.HashPassword(_options.AdminPassword);
                    var result = _userManager.UpdateAsync(AdminCheck).Result;

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Admin user updated successfully.");
                    }
                    else
                    {
                        _logger.LogError("Failed to update Admin user: {0}", result.Errors.ToString());
                    }
                }
            }
        }

        #endregion

        #region Create, update and delete entities via enum values

        public void EnumValues()
        {
            //var DealerRoleTypes = typeof(DealerRoleNames).GetEnumDataTypeValues();

            //foreach (var type in DealerRoleTypes)
            //{
            //    if (!_dbContext.DealerRoles.Where(x => type == x.Name).Any())
            //    {
            //        _dbContext.Add(new DealerRole() { Name = type });
            //    }
            //}

            //var ContentAccessLevelTypes = typeof(ContentAccessLevelNames).GetEnumDataTypeValues();

            //foreach (var type in ContentAccessLevelTypes)
            //{
            //    if (!_dbContext.ContentAccessLevels.Where(x => type == x.Name).Any())
            //    {
            //        _dbContext.Add(new ContentAccessLevel() { Name = type });
            //    }
            //}

            //if (!_dbContext.Dealers.Where(d => d.Name == "Default").Any())
            //{
            //    _dbContext.Add(new Dealer() { Name = "Default" });
            //}

            //_dbContext.SaveChanges();
        }
        public void SyncStatuses()
        {
            // To seed and update missing syn statuses
            SyncStatusTypeUser syncStatusTypeUser = new SyncStatusTypeUser(_dbContext);
            syncStatusTypeUser.CreateSyncStatuses();

            SyncStatusTypeDevice syncStatusTypeDevice = new SyncStatusTypeDevice(_dbContext);
            syncStatusTypeDevice.CreateSyncStatuses();

            // To delete obsolete sync statuses
            foreach (var syncStatus in typeof(DealerRoleNames).GetEnumDataTypeValues())
            {
                var checkSyncStatus = _dbContext.SyncStatuses.FirstOrDefault(x => x.Name == syncStatus);
                if (checkSyncStatus != null)
                {
                    _dbContext.SyncStatuses.Remove(checkSyncStatus);
                }
            }
        }

        #endregion
    }
}
