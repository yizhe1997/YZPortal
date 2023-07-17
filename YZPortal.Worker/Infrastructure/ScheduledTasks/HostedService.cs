namespace YZPortal.Worker.Infrastructure.ScheduledTasks
{
    public abstract class HostedService : IHostedService
    {
        private Task? ExecutingTask;

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            // Store the task we're executing
            ExecutingTask = ExecuteAsync(cancellationToken);

            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if (ExecutingTask.IsCompleted)
            {
                return ExecutingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (ExecutingTask == null)
            {
                return;
            }
            else
            {
                await Task.WhenAny(ExecutingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        protected virtual async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                await Process(stoppingToken);

                await Task.Delay(5000, stoppingToken); //5 seconds delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        protected abstract Task Process(CancellationToken cancellationToken);
    }
}
