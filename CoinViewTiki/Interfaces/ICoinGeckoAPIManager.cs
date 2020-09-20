using System.Collections.Generic;
using System.Threading.Tasks;
using CoinViewTiki.Models;

namespace CoinViewTiki.Interfaces
{
    public interface ICoinGeckoAPIManager
    {
        Task<CoinData> GetCoinDetailAsync(string id);
        Task<List<Coin>> GetCoinsAsync(int days = 1, bool forceRefresh = false);
        
        Task<List<MarketUSDCoin>> GetCoinsViaUSDMarketAsync(int days = 1, bool forceRefresh = false);

    }
}