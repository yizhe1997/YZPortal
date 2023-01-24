namespace YZPortal.API.Infrastructure.Security.AzureAdB2C
{
    public static class StartupExtensions
    {
        public static void AddAzureAdB2C(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureAdB2CApiOptions>(configuration.GetSection("AzureAdB2CApi"));
            services.Configure<AzureAdB2CSwaggerOptions>(configuration.GetSection("AzureAdB2CSwagger"));
        }
    }
}