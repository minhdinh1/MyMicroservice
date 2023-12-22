using MyMicroservice.DbContextSpace;
using MyMicroservice.Jobs.Abstractions;
using MyMicroservice.Models;
using MyMicroservice.Services;
using System;

namespace MyMicroservice.Jobs.PersistRatesJob
{
    public class PersistRatesJob : IRecurringJob
    {
        const string CTC_COINMARKETCAP_ID = "5198";
        const string NGN_SYMBBOL = "NGN";

        private readonly ILogger<PersistRatesJob> logger;
        private readonly BloggingContext dbContext;
        private readonly IConfiguration configuration;
        private readonly int jobLockTimeoutInMinutes;
        private readonly IRatesService ratesService;

        public string Name => nameof(PersistRatesJob);

        public PersistRatesJob(
            ILogger<PersistRatesJob> logger,
            IConfiguration configuration,
            IRatesService ratesService)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.dbContext = new BloggingContext(configuration); ;
            jobLockTimeoutInMinutes = configuration.GetValue<int>("Jobs:PersistRatesJob:LockTimeout");
            this.ratesService = ratesService;
        }
        public async Task ExecuteAsync()
        {
            logger.LogInformation("Starting {jobname} job.", Name);
            try
            {
                var coinMarketCapData = await ratesService.GetCoinMarketCapRatesAsync(CTC_COINMARKETCAP_ID);

                var forexData = await ratesService.GetForexRatesAsync(NGN_SYMBBOL);

                SaveDataToDatabase(coinMarketCapData, forexData);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "{jobname} job failed.", Name);
                throw;
            }

            logger.LogInformation("Completed {jobname} job.", Name);
        }

        private void SaveDataToDatabase(CoinMarketCapResponse coinMarketCapData, ForexApiResponse forexData)
        {
            dbContext.Rates.Add(new Rate()
            {
                Id = new Guid(),
                Name = coinMarketCapData.Data.CoinDetail.Name,
                Symbol = coinMarketCapData.Data.CoinDetail.Symbol,
                Price = coinMarketCapData.Data.CoinDetail.Quote.Usd.Price,
                LastUpdated = coinMarketCapData.Data.CoinDetail.Quote.Usd.Last_updated,
                CreatedDateTime = DateTime.UtcNow,
            });
            var datetime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dbContext.Rates.Add(new Rate()
            {
                Id = new Guid(),
                Name = forexData.Source,
                Symbol = forexData.Source,
                Price = forexData.Quotes.GetValueOrDefault("NGNUSD"),
                LastUpdated = datetime.AddSeconds(long.Parse(forexData.Timestamp)),
                CreatedDateTime = DateTime.UtcNow,
            });

            dbContext.SaveChanges();
        }
    }
}
