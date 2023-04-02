using YZPortal.FullStackCore.Infrastructure.Security.Authorization;

namespace YZPortal.Client.Services.Authorization
{
    public static class StartupExtensions
    {
        public static void AddAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationCore(opts =>
            {
                // DealerId
                opts.AddDealerIdPolicy();

                // Role
                opts.AddIsRoleAdminPolicy();

                // Access Levels
                opts.AddIsAccessLvlUserPolicy();
            });
        }
    }
}
