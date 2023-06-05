using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace YZPortal.Client.Services.Authentication
{
    public static class StartupExtensions
	{
		public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration) 
		{
			services.AddMsalAuthentication<RemoteAuthenticationState, CustomUserAccount>(options =>
			{
				configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                options.UserOptions.RoleClaim = "roles";
                options.ProviderOptions.DefaultAccessTokenScopes.Add("https://yzorganization.onmicrosoft.com/797f38f6-910d-4224-a027-faa9c581e6c7/Authorization");
			}).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, CustomUserAccount, CustomUserFactory>();
		}
	}
}
