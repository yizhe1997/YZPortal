namespace YZPortal.Worker.Infrastructure.ScheduledTasks
{
    public static class StartupExtensions
    {
        public static void AddScheduledTasks(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ScheduledTasksOptions>(configuration.GetSection("ScheduledTasks"));

            // Start Mail services in the background
            //services.AddSingleton<IHostedService, SendDealerInvitesTask>();
            //services.AddSingleton<IHostedService, SendPasswordResetsTask>();
            //services.AddSingleton<IHostedService, SendMembershipNotificationsTask>();

            // Start Sync Services in the background
            //services.AddSingleton<IHostedService, SyncAdminInitializer>();
        }
    }
}
