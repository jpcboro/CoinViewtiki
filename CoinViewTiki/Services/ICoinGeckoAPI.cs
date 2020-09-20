using System.Collections.Generic;
using System.Threading.Tasks;
using CoinViewTiki.Models;
using Refit;

namespace CoinViewTiki.Services
{
    public interface ICoinGeckoAPI
    {
        [Get("/api/v3/coins/list")]
        Task<List<Coin>> GetCoins();
        
        [Get("/api/v3/coins/markets?vs_currency=usd")]
        Task<List<MarketUSDCoin>> GetCoinsByUSDMarket();

        [Get("/api/v3/coins/{id}")]
        Task<CoinData> GetCoinData(string id);

    }
}