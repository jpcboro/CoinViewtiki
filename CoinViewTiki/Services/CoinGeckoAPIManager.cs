using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using CoinViewTiki.Annotations;
using CoinViewTiki.Constants;
using CoinViewTiki.Interfaces;
using CoinViewTiki.Models;
using Polly;
using Prism.Services;
using Prism.Services.Dialogs;
using Refit;
using Xamarin.Essentials;

namespace CoinViewTiki.Services
{
    public class CoinGeckoAPIManager : ICoinGeckoAPIManager
    {

        private readonly ICoinGeckoAPI _coinGeckoApi;
        private readonly IBlobCache _blobCache;
        private const string url = "https://api.coingecko.com/api/v3/coins/list";
        private const string baseUrl = "https://api.coingecko.com/";
        public bool IsBusy { get; set; } = false;

        public CoinGeckoAPIManager()
        {
            
            _coinGeckoApi = RestService.For<ICoinGeckoAPI>(hostUrl: baseUrl);

            _blobCache = BlobCache.LocalMachine;

        }
        
        public async Task<List<Coin>> GetCoinsAsync(int days = 1, bool forceRefresh  = true)
        {
            if (forceRefresh)
            {
                await BlobCache.LocalMachine.InvalidateObject<List<Coin>>(CacheConstants.AllCoinsNSList);
                
            }
            
            var coinsFromCache = new List<Coin>();

            try
            {
                
                coinsFromCache = await BlobCache.LocalMachine.GetAndFetchLatest<List<Coin>>(
                    CacheConstants.AllCoinsNSList,
                    GetAndSaveCoinsAsync, offset =>
                    {
                        var elapsed = DateTimeOffset.Now - offset;
                        return elapsed > new TimeSpan(days: days,
                            hours: 0,
                            minutes: 0,
                            seconds: 0);
                    }
                );


                if (coinsFromCache != null)
                {
                    return coinsFromCache;

                }

                return await GetAndSaveCoinsAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to get data from server: {e}");

                throw;
            }

           
        }

        private async Task<List<Coin>> GetAndSaveCoinsAsync()
        {
            try
            {
                List<Coin> coins = await Policy
                    .Handle<HttpRequestException>(exception =>
                    {
                        Console.WriteLine($"API Exception when connection to Coin Gecko API: {exception.Message}");
                        return true;
                    })
                    .WaitAndRetryAsync(
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt =>
                            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Retry exception: {ex.Message}, retrying...");
                        }
                    )
                    .ExecuteAsync(async () => await _coinGeckoApi.GetCoins());
                

                await BlobCache.LocalMachine.InsertObject(CacheConstants.AllCoinsNSList,
                    coins, DateTimeOffset.Now.AddSeconds(80));

               
                return coins;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to get data from server: {ex.Message}");
                throw;
            }
            

        }

        public async Task<CoinData> GetCoinDetailAsync(string id)
        {
            try
            {
                CoinData coinDetail = await Policy.Handle<HttpRequestException>(exception =>
                    {
                        Console.WriteLine($"API Exception when connection to Coin Gecko API: {exception.Message}");
                        return true;
                    })
                    .WaitAndRetryAsync(retryCount: 3,
                        sleepDurationProvider: retryAttempt =>
                            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Retry exception: {ex.Message}, retrying...");
                        })
                    .ExecuteAsync(async () => await _coinGeckoApi.GetCoinData(id));
                
                return coinDetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to get data from server: {ex.Message}");
                throw;
            }
            

        }
    }
}