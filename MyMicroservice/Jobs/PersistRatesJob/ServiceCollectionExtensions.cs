using MyMicroservice.Jobs.Abstractions;

namespace MyMicroservice.Jobs.PersistRatesJob
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistRatesJobServices(this IServiceCollection services)
        {
            services.AddTransient<IRecurringJobScheduler, PersistRatesJobScheduler>();
            services.AddTransient<PersistRatesJob>();

            return services;
        }
    }
}
