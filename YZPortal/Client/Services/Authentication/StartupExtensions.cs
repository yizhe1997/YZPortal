using Microsoft.AspNetCore.Components.Authorization;

namespace YZPortal.Client.Services.Authentication
{
	public static class StartupExtensions
	{
		public static void AddAuthentication(this IServiceCollection services) 
		{
			services.AddScoped<CustomAuthenticationStateProvider>();
			services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());
		}
	}
}
