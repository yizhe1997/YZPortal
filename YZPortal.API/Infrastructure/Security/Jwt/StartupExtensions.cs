namespace YZPortal.API.Infrastructure.Security.Jwt
{
    public static class StartupExtensions
    {
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtIssuerOptions>(configuration.GetSection("Jwt"));

            services.AddTransient<JwtTokenGenerator>();
        }
    }
}
