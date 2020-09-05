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

    }
}