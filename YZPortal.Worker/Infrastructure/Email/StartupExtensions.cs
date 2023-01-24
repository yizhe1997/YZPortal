namespace YZPortal.Worker.Infrastructure.Email
{
    public static class StartupExtensions
    {
        public static void AddEmailNotifications(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailOptions>(configuration.GetSection("Email"));
        }
    }
}
