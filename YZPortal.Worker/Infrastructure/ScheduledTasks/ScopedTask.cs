namespace YZPortal.Worker.Infrastructure.ScheduledTasks
{
    public abstract class ScopedTask : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ScopedTask(IServiceScopeFactory serviceScopeFactory) : base()
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Process(CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                await ProcessInScope(scope.ServiceProvider, cancellationToken);
            }
        }

        public abstract Task ProcessInScope(IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }
}
