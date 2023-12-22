using Hangfire;
using MyMicroservice.Jobs.Abstractions;

namespace MyMicroservice.Jobs
{
    public class JobHostedService : IHostedService
    {
        private readonly IEnumerable<IRecurringJobScheduler> recurringJobSchedulers;
        private readonly IRecurringJobManager recurringJobManager;
        public JobHostedService(
            IEnumerable<IRecurringJobScheduler> recurringJobSchedulers,
            IRecurringJobManager recurringJobManager)
        {
            this.recurringJobSchedulers = recurringJobSchedulers;
            this.recurringJobManager = recurringJobManager;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var scheduler in recurringJobSchedulers)
            {
                await scheduler.ScheduleAsync(recurringJobManager);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
