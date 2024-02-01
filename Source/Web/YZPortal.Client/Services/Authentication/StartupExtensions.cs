using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using YZPortal.Client.Clients.YZPortalApi;

namespace YZPortal.Client.Services.Authentication
{
    public static class StartupExtensions
	{
		public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration) 
		{
            var yzPortalApiConfig = configuration.GetSection("YZPortalApi").Get<YZPortalApiConfig>() ?? new();

            services.AddMsalAuthentication<RemoteAuthenticationState, CustomUserAccount>(options =>
			{
				configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                options.UserOptions.RoleClaim = "roles";
                options.ProviderOptions.DefaultAccessTokenScopes.Add(yzPortalApiConfig.Scope ?? string.Empty);
			}).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, CustomUserAccount, CustomUserFactory>();
		}
	}
}
