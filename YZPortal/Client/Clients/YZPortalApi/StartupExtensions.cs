using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace YZPortal.Client.Clients.YZPortalApi
{
    public static class StartupExtensions
    {
        public static void AddYZPortalApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<CustomAuthorizationMessageHandler>();
            // Configure options
            services.Configure<YZPortalApiOptions>(configuration.GetSection("YZPortalApi"));

            var yzPortalApiOptions = configuration.GetSection("YZPortalApi").Get<YZPortalApiOptions>() ?? new();

            if (string.IsNullOrWhiteSpace(yzPortalApiOptions.BaseAddress))
                throw new InvalidOperationException("BaseAddress for YZPortalApi not configured.");

            // Add client
            services.AddHttpClient<YZPortalApiHttpClient>("YZPortal.ServerAPI", client => client.BaseAddress = new Uri(yzPortalApiOptions.BaseAddress))
				.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

			// Supply HttpClient instances that include access tokens when making requests to the server project
			services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("YZPortal.ServerAPI"));
		}
    }

    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager, IConfiguration configuration) : base(provider, navigationManager)
        {
            ConfigureHandler(authorizedUrls: new[] 
            {
                configuration["YZPortalApi:BaseAddress"]
            });
        }
    }
}
