using Microsoft.Extensions.Options;
using NCrontab;
using YZPortal.Worker.Infrastructure.ScheduledTasks;

namespace YZPortal.Worker.Tasks.Sync
{
	public abstract class SyncTask : ScheduledTask
	{
		protected override CrontabSchedule Schedule => CrontabSchedule.Parse(_options.SyncSchedule);

		public SyncTask(IServiceScopeFactory serviceScopeFactory, IOptions<ScheduledTasksOptions> options) : base(serviceScopeFactory, options)
		{
		}
	}
}
