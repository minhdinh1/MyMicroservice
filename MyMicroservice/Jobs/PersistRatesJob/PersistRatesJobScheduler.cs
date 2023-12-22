using Hangfire;
using MyMicroservice.Jobs.Abstractions;

namespace MyMicroservice.Jobs.PersistRatesJob
{
    public class PersistRatesJobScheduler : IRecurringJobScheduler
    {
        private readonly IConfiguration configuration;

        public PersistRatesJobScheduler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task ScheduleAsync(IRecurringJobManager jobManager, CancellationToken cancellationToken = default)
        {
            var cron = configuration.GetValue<string>($"Jobs:{nameof(PersistRatesJob)}:Cron", null);

            jobManager.AddOrUpdate<PersistRatesJob>(nameof(PersistRatesJob), j => j.ExecuteAsync(), cron);

            return Task.CompletedTask;
        }
    }
}
