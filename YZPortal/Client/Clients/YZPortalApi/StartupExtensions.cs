namespace YZPortal.Client.Clients.YZPortalApi
{
    public static class StartupExtensions
    {
        public static void AddYZPortalApi(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure options
            services.Configure<YZPortalApiOptions>(configuration.GetSection("YZPortalApi"));

            var yZPortalApiOptions = configuration.GetSection("YZPortalApi").Get<YZPortalApiOptions>() ?? new();

            if (string.IsNullOrWhiteSpace(yZPortalApiOptions.BaseAddress))
                throw new InvalidOperationException("BaseAddress for YZPortalApi not configured.");

            // Add client
            services.AddHttpClient<YZPortalApiHttpClient>(client => client.BaseAddress = new Uri(yZPortalApiOptions.BaseAddress));
        }
    }
}
