using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Web.Helpers;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.Core.Domain.Database.Sync;
using YZPortal.Core.Domain.Contexts;
using YZPortal.FullStackCore.Enums.Memberships;
using YZPortal.FullStackCore.Extensions;
using System.Net;
using YZPortal.Core.Error;
using AutoMapper;
using System.Linq.Dynamic.Core;
using YZPortal.Core.Indexes;

namespace YZPortal.Core.Domain.Database
{
    // TODO: maybe can do logging here? instead of logging from error middleware, but then again the http req url already has the info? idk
    // TODO: maybe can create partial class for each entity?
    public class DatabaseService : IDatabaseService
    {
        private readonly PortalContext _dbContext;
        private readonly DatabaseOptions _options;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<DatabaseService> _logger;
        private readonly IMapper _mapper;

        // Constructor
        public DatabaseService(PortalContext dbContext, UserManager<User> userManager, IOptions<DatabaseOptions> options, ILogger<DatabaseService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _options = options.Value;
            _logger = logger;
            _mapper = mapper;
        }

        #region Startup services

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

        #region User

        /// <summary>
        ///     Async get users.
        /// </summary>
        public async Task<SearchList<User>> UsersToSearchListAsync(ISearchParams request, System.Linq.Expressions.Expression<Func<User, bool>>? searchPredicate = null, CancellationToken cancellationToken = new CancellationToken())
        {
            // AsQueryable allows dynamically build and refine query by adding additional LINQ operators
            return await _dbContext.GetUsersAsQueryable().CreateSearchListAsync(request, searchPredicate, cancellationToken);
        }

        /// <summary>
        ///     Async update user if user with subject identifier exist.
        /// </summary>
        public async Task<User> UserGetBySubIdAsync(string? subId, CancellationToken cancellationToken = new CancellationToken())
        {
            // Validate if user exist
            var user = await _dbContext.GetUserBySubIdFirstOrDefaultAsync(subId, cancellationToken) ?? throw new RestException(HttpStatusCode.BadRequest, "User not found.");

            return user;
        }

        /// <summary>
        ///     Async update user if user with Id exist.
        /// </summary>
        public async Task<User> UserGetAsync(Guid Id, CancellationToken cancellationToken = new CancellationToken())
        {
            // Validate if user exist
            var user = await _dbContext.GetUserByIdFirstOrDefaultAsync(Id, cancellationToken) ?? throw new RestException(HttpStatusCode.BadRequest, "User not found.");

            return user;
        }

        // TODO: use interface for type T to handle current context and etc, not sure if this will work? but theres similarity
        /// <summary>
        ///     Async create user if user with subject identifier does not exist.
        /// </summary>
        public async Task<User> UserCreateAsync<T>(T body, CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            // If T body is already user then dont map?
            // Map input body to new user
            var newUser = _mapper.Map<User>(body);

            // Validate if user exist
            var checkUser = await _dbContext.GetUserBySubIdFirstOrDefaultAsync(newUser.SubjectIdentifier, cancellationToken);
            if (checkUser != null)
                throw new RestException(HttpStatusCode.BadRequest, "User already exist!");

            // Create user and validate
            var createUserResult = await _userManager.CreateAsync(newUser);
            if (!createUserResult.Succeeded)
                throw new RestException(HttpStatusCode.BadRequest, createUserResult.Errors.Select(e => e.Description).ToList());

            return newUser;
        }

        // TODO: use interface for type T to handle current context and etc, not sure if this will work? but theres similarity
        // but.... model and entity should they have the same name? but i value automappers flexibility more, imo i think adding new
        // config in mapping profile is more flexible than defining it for an interface only? issue with interface is i have 
        // to add evrything right?
        /// <summary>
        ///     Async update user if user with subject identifier exist.
        /// </summary>
        public async Task<User> UserUpdateAsync<T>(string? subId, T body, CancellationToken cancellationToken = new CancellationToken()) where T : CurrentContext
        {
            // Validate if user exist
            var user = await _dbContext.GetUserBySubIdFirstOrDefaultAsync(subId, cancellationToken) ?? throw new RestException(HttpStatusCode.BadRequest, "User not found.");

            // Map fields to existing user and save
            _mapper.Map(body, user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return user;
        }

        /// <summary>
        ///     Async delete user if user with the given id exist.
        /// </summary>
        public async Task<User> UserDeleteAsync(Guid id, CancellationToken cancellationToken = new CancellationToken())
        {
            // Validate if user exist
            var user = await _dbContext.GetUserByIdFirstOrDefaultAsync(id, cancellationToken) ?? throw new RestException(HttpStatusCode.NotFound, "User not found.");

            // If user exist then remove from db and save
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return user;
        }

        #endregion
    }
}
