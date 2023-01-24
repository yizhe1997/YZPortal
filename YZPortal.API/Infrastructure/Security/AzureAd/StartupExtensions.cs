namespace YZPortal.API.Infrastructure.Security.AzureAd
{
    public static class StartupExtensions
    {
        public static void AddAzureAd(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureAdApiOptions>(configuration.GetSection("AzureAdApi"));
            services.Configure<AzureAdSwaggerOptions>(configuration.GetSection("AzureAdSwagger"));
        }
    }
}
