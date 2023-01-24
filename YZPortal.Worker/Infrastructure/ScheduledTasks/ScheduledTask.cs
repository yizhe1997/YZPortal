using Microsoft.Extensions.Options;
using NCrontab;

namespace YZPortal.Worker.Infrastructure.ScheduledTasks
{
    public abstract class ScheduledTask : ScopedTask
    {
        protected abstract CrontabSchedule Schedule { get; }
        protected ScheduledTasksOptions _options;
        private DateTime _nextRun;

        public ScheduledTask(IServiceScopeFactory serviceScopeFactory, IOptions<ScheduledTasksOptions> options) : base(serviceScopeFactory)
        {
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _nextRun = DateTime.UtcNow;

            do
            {
                var now = DateTime.UtcNow;
                if (now > _nextRun)
                {
                    await Process(stoppingToken);
                    _nextRun = Schedule.GetNextOccurrence(DateTime.UtcNow);
                }
                await Task.Delay(5000, stoppingToken);
            }
            while (!stoppingToken.IsCancellationRequested);
        }
    }
}
