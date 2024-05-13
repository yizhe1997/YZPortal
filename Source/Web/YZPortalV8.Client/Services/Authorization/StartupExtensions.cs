using Application.Extensions;

namespace YZPortalV8.Client.Services.Authorization
{
    public static class StartupExtensions
    {
        public static void AddAuthorization(this IServiceCollection services)
        {
            // Probably needed for RemoteAuthenticationState in Authentication.StartupExtensions
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
