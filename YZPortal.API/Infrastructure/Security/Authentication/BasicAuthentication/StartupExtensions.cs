namespace YZPortal.API.Infrastructure.Security.Authentication.BasicAuthentication
{
    public static class StartupExtensions
    {
        public static void AddBasicAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BasicAuthenticationOptions>(configuration.GetSection("BasicAuthentication"));
        }
    }
}
