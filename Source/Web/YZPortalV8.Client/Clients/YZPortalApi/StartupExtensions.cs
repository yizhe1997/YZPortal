using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace YZPortalV8.Client.Clients.YZPortalApi
{
    public static class StartupExtensions
    {
        public static void AddYZPortalApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<CustomAuthorizationMessageHandler>();
            // Configure options
            services.Configure<YZPortalApiConfig>(configuration.GetSection("YZPortalApi"));

            var yzPortalApiConfig = configuration.GetSection("YZPortalApi").Get<YZPortalApiConfig>() ?? new();

            // REF: https://stackoverflow.com/questions/60552768/how-to-acess-the-appsettings-in-blazor-webassembly
            services.AddSingleton(yzPortalApiConfig);

            if (string.IsNullOrWhiteSpace(yzPortalApiConfig.BaseAddress))
                throw new InvalidOperationException("BaseAddress for YZPortalApi not configured.");

            // Add client
            services.AddHttpClient<YZPortalApiHttpClient>("YZPortal.ServerAPI", client => client.BaseAddress = new Uri(yzPortalApiConfig.BaseAddress))
				.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

			// Supply HttpClient instances that include access tokens when making requests to the server project
			services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("YZPortal.ServerAPI"));
		}
    }

    // REF: https://stackoverflow.com/questions/63076954/automatically-attaching-access-token-to-http-client-in-blazor-wasm
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager, IConfiguration configuration) : base(provider, navigationManager)
        {
            var yzPortalApiConfig = configuration.GetSection("YZPortalApi").Get<YZPortalApiConfig>() ?? new();

            if (string.IsNullOrWhiteSpace(yzPortalApiConfig.BaseAddress))
                throw new InvalidOperationException("BaseAddress for YZPortalApi not configured.");

            if (string.IsNullOrWhiteSpace(yzPortalApiConfig.Scope))
                throw new InvalidOperationException("Scope for YZPortalApi not configured.");

            ConfigureHandler(authorizedUrls:
            [
                yzPortalApiConfig.BaseAddress
            ],
            scopes:
            [
                yzPortalApiConfig.Scope
            ]);
        }
    }
}
