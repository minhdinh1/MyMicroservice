using MyMicroservice.Models;
using Newtonsoft.Json;

namespace MyMicroservice.Services
{
    public class RatesService : IRatesService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration configuration;

        public RatesService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            this.configuration = configuration;
        }

        public async Task<CoinMarketCapResponse> GetCoinMarketCapRatesAsync(string id)
        {
            var apiKey = configuration.GetValue<string>("ApiKeys:CoinMarketCap");
            var coinMarketCapUrl = configuration.GetValue<string>("CoinMarketBaseUrl");
            var apiUrl = $"{coinMarketCapUrl}/v2/cryptocurrency/quotes/latest?id={id}";
            _httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", apiKey);
            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var coinMarketCapResponse = JsonConvert.DeserializeObject<CoinMarketCapResponse>(jsonResponse);

                return coinMarketCapResponse;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                return null;
            }
        }

        public async Task<ForexApiResponse> GetForexRatesAsync(string symbol)
        {
            var apiKey = configuration.GetValue<string>("ApiKeys:CurrencyLayer");
            var currencyLayerUrl = configuration.GetValue<string>("CurrencyLayerBaseUrl");
            var apiUrl = $"{currencyLayerUrl}/live?access_key={apiKey}&currencies=usd&source={symbol}";
            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var forexApiResponse = JsonConvert.DeserializeObject<ForexApiResponse>(jsonResponse);

                return forexApiResponse;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                return null;
            }
        }
    }
}
