using YZPortal.Worker.Tasks.Sync.Users;

namespace YZPortal.Worker.Infrastructure.ScheduledTasks
{
    public static class StartupExtensions
    {
        public static void AddScheduledTasks(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ScheduledTasksOptions>(configuration.GetSection("ScheduledTasks"));

            //// Start Mail services in the background
            //services.AddSingleton<IHostedService, SendInvitesTask>();
            //services.AddSingleton<IHostedService, SendPasswordResetsTask>();
            //services.AddSingleton<IHostedService, SendMembershipNotificationsTask>();
            //services.AddSingleton<IHostedService, SendStatusUpdateTask>();

            //// Start Sync Services in the background
            //services.AddSingleton<IHostedService, SyncClassifiersTask>();
            //services.AddSingleton<IHostedService, SyncDealersTask>();
            //services.AddSingleton<IHostedService, SyncDevicesTask>();
            services.AddSingleton<IHostedService, SyncAdminInitializer>();

            //// BYOD
            //services.AddSingleton<IHostedService, Tasks.Sync.BYOD.SyncUsersTask>();
            //services.AddSingleton<IHostedService, Tasks.Sync.BYOD.SyncPartsTask>();
            //services.AddSingleton<IHostedService, Tasks.Sync.BYOD.SyncLogisticsTask>();
            //services.AddSingleton<IHostedService, Tasks.Sync.BYOD.SyncDevicesTask>();
            //services.AddSingleton<IHostedService, Tasks.Sync.BYOD.SyncDealersTask>();
            //services.AddSingleton<IHostedService, Tasks.Sync.BYOD.SyncWarrantyTask>();

            //// Start Soft Delete Services in the background
            //services.AddSingleton<IHostedService, Tasks.Sync.DeleteJobs.SoftDeleteTask>();
        }
    }
}
