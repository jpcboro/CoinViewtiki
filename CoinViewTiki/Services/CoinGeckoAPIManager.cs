using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinViewTiki.Interfaces;
using CoinViewTiki.Models;
using MonkeyCache.FileStore;
using Refit;
using Xamarin.Essentials;

namespace CoinViewTiki.Services
{
    public class CoinGeckoAPIManager : ICoinGeckoAPIManager
    {
        private readonly ICoinGeckoAPI _coinGeckoApi;
        private const string url = "https://api.coingecko.com/api/v3/coins/list";
        private const string baseUrl = "https://api.coingecko.com/";
        public bool IsBusy { get; set; } = false;

        public CoinGeckoAPIManager()
        {
    
            _coinGeckoApi = RestService.For<ICoinGeckoAPI>(hostUrl: baseUrl);

            Barrel.ApplicationId = "CoinGeckoCache";

        }

        public async Task<List<Coin>> GetCoinsAsync(int days = 1, bool forceRefresh  = false)
        {
            var coinsList = new List<Coin>();

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    coinsList = Barrel.Current.Get<List<Coin>>(key: url);
                }
                else if (!forceRefresh && !Barrel.Current.IsExpired(key: url))
                {
                    coinsList = Barrel.Current.Get<List<Coin>>(key: url);
                 
                }
                
                try
                {
                    if (!coinsList.Any())
                    {
                        coinsList = await _coinGeckoApi.GetCoins();
                        Barrel.Current.Add(key: url, data: coinsList, expireIn:TimeSpan.FromDays(1));

                    }

                return coinsList;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to get data from server: {e}");
                throw;
            }
                
        }

        public async Task<CoinData> GetCoinDetailAsync(string id)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                
            }

            try
            {
                var coinDetail = await _coinGeckoApi.GetCoinData(id);
                
                return coinDetail;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to get data from server: {e}");
                throw;
            }
            
        }
    }
}