using System;
using Newtonsoft.Json;

namespace CoinViewTiki.Models
{
    public class Coin
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }
      
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
