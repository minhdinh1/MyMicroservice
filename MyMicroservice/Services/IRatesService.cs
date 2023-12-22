using MyMicroservice.Models;

namespace MyMicroservice.Services
{
    public interface IRatesService
    {
        Task<ForexApiResponse> GetForexRatesAsync(string symbol);
        Task<CoinMarketCapResponse> GetCoinMarketCapRatesAsync(string id);
    }
}
