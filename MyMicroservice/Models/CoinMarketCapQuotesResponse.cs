using Newtonsoft.Json;

namespace MyMicroservice.Models
{
    public class CoinMarketCapResponse
    {
        public Status Status { get; set; }
        public Data Data { get; set; }
    }

    public class Status
    {
        public string Timestamp { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public int Elapsed { get; set; }
        public int CreditCount { get; set; }
        public string Notice { get; set; }
    }

    public class Data
    {
        [JsonProperty("5198")]
        public CoinDetail CoinDetail { get; set; }
    }

    public class CoinDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Slug { get; set; }
        public int NumMarketPairs { get; set; }
        public string DateAdded { get; set; }
        public List<Tag> Tags { get; set; }
        public long? MaxSupply { get; set; }
        public long CirculatingSupply { get; set; }
        public long TotalSupply { get; set; }
        public int IsActive { get; set; }
        public bool InfiniteSupply { get; set; }
        public object Platform { get; set; }
        public int CmcRank { get; set; }
        public int IsFiat { get; set; }
        public object SelfReportedCirculatingSupply { get; set; }
        public object SelfReportedMarketCap { get; set; }
        public object TvlRatio { get; set; }
        public DateTime Last_updated { get; set; }
        public Quote Quote { get; set; }
    }

    public class Tag
    {
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }

    public class Quote
    {
        [JsonProperty("USD")]
        public Usd Usd { get; set; }
    }

    public class Usd
    {
        public double Price { get; set; }
        public double Volume24h { get; set; }
        public double VolumeChange24h { get; set; }
        [JsonProperty("percent_change_1h")]
        public double PercentChange1h { get; set; }
        [JsonProperty("percent_change_24h")]
        public double PercentChange24h { get; set; }
        [JsonProperty("percent_change_7d")]
        public double PercentChange7d { get; set; }
        [JsonProperty("percent_change_30d")]
        public double PercentChange30d { get; set; }
        [JsonProperty("percent_change_60d")]
        public double PercentChange60d { get; set; }
        [JsonProperty("percent_change_90d")]
        public double PercentChange90d { get; set; }
        public double MarketCap { get; set; }
        public double MarketCapDominance { get; set; }
        [JsonProperty("fully_diluted_market_cap")]
        public double FullyDilutedMarketCap { get; set; }
        public object Tvl { get; set; }
        public DateTime Last_updated { get; set; }
    }
}
