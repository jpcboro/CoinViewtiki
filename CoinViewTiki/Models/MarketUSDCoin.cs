using Newtonsoft.Json;

namespace CoinViewTiki.Models
{
    public class MarketUSDCoin : Coin
    {
        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }
    }
}