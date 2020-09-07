using System;
using Newtonsoft.Json;

namespace CoinViewTiki.Models
{
    public class CoinData : Coin
    {
        [JsonProperty(PropertyName = "image")]
        public Image image { get; set; }
        
        [JsonProperty(PropertyName = "description")]
        public Description description { get; set; }

        [JsonProperty(PropertyName = "market_data")]
        public MarketData market_data { get; set; }
    }

    public class MarketData
    {
        [JsonProperty(PropertyName = "current_price")]
        public CurrentPrice current_price { get; set; }
    }

    public class Image
    {
        [JsonProperty(PropertyName = "thumb")]
        public string thumb { get; set; }
        [JsonProperty(PropertyName = "small")]
        public string small { get; set; }
        [JsonProperty(PropertyName = "large")]
        public string large { get; set; }
    }

    public class Description
    {
        [JsonProperty(PropertyName = "en")]
        public string en { get; set; }
    }

    public class CurrentPrice
    {
        [JsonProperty(PropertyName = "usd")]
        public double usd { get; set; }
    }
}
