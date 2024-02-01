using Application.Interfaces.Services;
using System.Linq.Expressions;

namespace Infrastructure.Services.BackgroundJob
{
    public class HangFireService : IJobService
    {
        #region Enqueue

        public string Enqueue(Expression<Func<Task>> methodCall) =>
            Hangfire.BackgroundJob.Enqueue(methodCall);

        public string Enqueue<T>(Expression<Action<T>> methodCall) =>
            Hangfire.BackgroundJob.Enqueue(methodCall);

        public string Enqueue(Expression<Action> methodCall) =>
            Hangfire.BackgroundJob.Enqueue(methodCall);

        public string Enqueue<T>(Expression<Func<T, Task>> methodCall) =>
            Hangfire.BackgroundJob.Enqueue(methodCall);

        #endregion

        #region Schedule

        public string Schedule(Expression<Action> methodCall, TimeSpan delay) =>
            Hangfire.BackgroundJob.Schedule(methodCall, delay);

        public string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay) =>
            Hangfire.BackgroundJob.Schedule(methodCall, delay);

        public string Schedule(Expression<Action> methodCall, DateTimeOffset enqueueAt) =>
            Hangfire.BackgroundJob.Schedule(methodCall, enqueueAt);

        public string Schedule(Expression<Func<Task>> methodCall, DateTimeOffset enqueueAt) =>
            Hangfire.BackgroundJob.Schedule(methodCall, enqueueAt);

        public string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay) =>
            Hangfire.BackgroundJob.Schedule(methodCall, delay);

        public string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay) =>
            Hangfire.BackgroundJob.Schedule(methodCall, delay);

        public string Schedule<T>(Expression<Action<T>> methodCall, DateTimeOffset enqueueAt) =>
            Hangfire.BackgroundJob.Schedule(methodCall, enqueueAt);

        public string Schedule<T>(Expression<Func<T, Task>> methodCall, DateTimeOffset enqueueAt) =>
            Hangfire.BackgroundJob.Schedule(methodCall, enqueueAt);

        #endregion

        #region Delete

        public bool Delete(string jobId) =>
            Hangfire.BackgroundJob.Delete(jobId);

        public bool Delete(string jobId, string fromState) =>
            Hangfire.BackgroundJob.Delete(jobId, fromState);

        #endregion

        #region Requeue

        public bool Requeue(string jobId) =>
            Hangfire.BackgroundJob.Requeue(jobId);

        public bool Requeue(string jobId, string fromState) =>
            Hangfire.BackgroundJob.Requeue(jobId, fromState);

        #endregion
    }
}
