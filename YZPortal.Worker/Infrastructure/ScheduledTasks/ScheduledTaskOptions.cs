namespace YZPortal.Worker.Infrastructure.ScheduledTasks
{
    public class ScheduledTasksOptions
    {
        #region Sync

        public string SyncSchedule { get; set; } = "*/5 * * * *";

        #endregion

        #region Email

        public string EmailSchedule { get; set; } = "*/10 * * * *";

        #endregion

        #region SoftDelete

        public string DeleteSchedule { get; set; } = "0 0 * * *";

        #endregion
    }
}
