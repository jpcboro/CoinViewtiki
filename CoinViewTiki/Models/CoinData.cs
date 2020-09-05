using System;
namespace CoinViewTiki.Models
{
    public class CoinData : Coin
    {
        public Image image { get; set; }

        public Description description { get; set; }

        public MarketData market_data { get; set; }
    }

    public class MarketData
    {
        public CurrentPrice current_price { get; set; }
    }

    public class Image
    {
        public string thumb { get; set; }
    }

    public class Description
    {
        public string en { get; set; }
    }

    public class CurrentPrice
    {
        public double usd { get; set; }
    }
}
