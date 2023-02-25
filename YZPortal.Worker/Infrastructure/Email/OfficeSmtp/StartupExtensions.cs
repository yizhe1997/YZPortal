namespace YZPortal.Worker.Infrastructure.Email.OfficeSmtp
{
    public static class StartupExtensions
    {
        public static void AddOfficeSmtpNotifications(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OfficeSmtpOptions>(configuration.GetSection("OfficeSmtp"));
            var options = configuration.GetSection("OfficeSmtp").Get<OfficeSmtpOptions>();
        }
    }
}
