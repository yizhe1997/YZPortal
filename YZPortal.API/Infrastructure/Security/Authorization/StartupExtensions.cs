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
                // DealerId
                opts.AddDealerIdPolicy();

                // Role
                opts.AddIsRoleAdminPolicy();

                // Access Levels
                opts.AddIsAccessLvlUserPolicy();

                var allAuthenSchemes = new AuthorizationPolicyBuilder(
						JwtBearerDefaults.AuthenticationScheme,
						Constants.AzureAdB2C,
						Constants.AzureAd);
				opts.AddPolicy("AllAuthenSchemes", allAuthenSchemes
					.RequireAuthenticatedUser()
					.Build());

				var allExternalAuthSchemes = new AuthorizationPolicyBuilder(
						Constants.AzureAdB2C,
						Constants.AzureAd);
				opts.AddPolicy("AllExternalAuthSchemes", allExternalAuthSchemes
					.RequireAuthenticatedUser()
					.Build());
			});
            services.AddRequiredScopeAuthorization();
        }
    }
}
