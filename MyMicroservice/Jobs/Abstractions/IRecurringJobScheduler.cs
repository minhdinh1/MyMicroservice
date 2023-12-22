using Hangfire;

namespace MyMicroservice.Jobs.Abstractions
{
    public interface IRecurringJobScheduler
    {
        Task ScheduleAsync(IRecurringJobManager jobManager, CancellationToken cancellationToken = default);
    }
}
