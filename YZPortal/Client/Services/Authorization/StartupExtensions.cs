using YZPortal.FullStackCore.Infrastructure.Security.Authorization;

namespace YZPortal.Client.Services.Authorization
{
    public static class StartupExtensions
    {
        public static void AddAuthorization(this IServiceCollection services)
        {
            // i forgot why we need this... can remove if dont need
			services.AddApiAuthorization();

			services.AddAuthorizationCore(opts =>
            {
                // Role
                opts.AddAdministratorPolicy();
                opts.AddGeneralPolicy();

                // Subscription
                opts.AddBadmintonSubscriptionPolicy();
            });
        }
    }
}
