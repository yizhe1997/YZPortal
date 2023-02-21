using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using YZPortal.API.Infrastructure.Swagger;

namespace YZPortal.API.Infrastructure.Security.Authorization
{
    public static class StartupExtensions
    {
        public static void AddAuthorizations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(opts =>
            {
                var swaggerOptions = configuration.GetSection("Swagger").Get<SwaggerOptions>() ?? new();

                // TODO: properly implement this
                // Dealer roles
                opts.AddPolicy("dealer", policy => policy.RequireClaim("dealerId"));
                opts.AddPolicy("representative", policy => policy.RequireClaim("representative", "1"));
                opts.AddPolicy("authorizedUser", policy => policy.RequireClaim("authorizedUser", "1"));
                opts.AddPolicy("admin", policy => policy.RequireClaim("admin", "1"));
                opts.AddPolicy("systemUser", policy => policy.RequireClaim("systemUser", "1"));

                // Access Levels
                opts.AddPolicy("All", policy => policy.RequireClaim("All", "1"));
                opts.AddPolicy("PartsOrders", policy => policy.RequireClaim("PartsOrders", "1"));
                opts.AddPolicy("PartsInvoices", policy => policy.RequireClaim("PartsInvoices", "1"));
                opts.AddPolicy("ReturnOrders", policy => policy.RequireClaim("ReturnOrders", "1"));
                opts.AddPolicy("PartsBackOrders", policy => policy.RequireClaim("PartsBackOrders", "1"));
                opts.AddPolicy("DeviceOrders", policy => policy.RequireClaim("DeviceOrders", "1"));
                opts.AddPolicy("DeviceInquiries", policy => policy.RequireClaim("DeviceInquiries", "1"));
                opts.AddPolicy("DeviceInvoices", policy => policy.RequireClaim("DeviceInvoices", "1"));
                opts.AddPolicy("DeliveryReports", policy => policy.RequireClaim("DeliveryReports", "1"));
                opts.AddPolicy("DeviceBackOrders", policy => policy.RequireClaim("DeviceBackOrders", "1"));
                opts.AddPolicy("DeviceStatus", policy => policy.RequireClaim("DeviceStatus", "1"));
                opts.AddPolicy("ServiceCampaigns", policy => policy.RequireClaim("ServiceCampaigns", "1"));
                opts.AddPolicy("Case", policy => policy.RequireClaim("Case", "1"));
                opts.AddPolicy("WarrantyClaims", policy => policy.RequireClaim("WarrantyClaims", "1"));
                opts.AddPolicy("None", policy => policy.RequireClaim("None", "1"));

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
