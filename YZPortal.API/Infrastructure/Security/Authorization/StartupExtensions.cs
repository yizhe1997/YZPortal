using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using YZPortal.FullStackCore.Infrastructure.Security.Authorization;

namespace YZPortal.API.Infrastructure.Security.Authorization
{
    public static class StartupExtensions
    {
        public static void AddAuthorizations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(opts =>
            {
                // Role
                opts.AddAdministratorPolicy();
                opts.AddGeneralPolicy();

                // Subscription
                opts.AddBadmintonSubscriptionPolicy();

                // Authentication Schema
                var allAuthenSchemes = new AuthorizationPolicyBuilder(
						JwtBearerDefaults.AuthenticationScheme,
						Constants.AzureAdB2C);
				opts.AddPolicy("AllAuthenSchemes", allAuthenSchemes
					.RequireAuthenticatedUser()
					.Build());
			});
            services.AddRequiredScopeAuthorization();
        }
    }
}
