using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoinViewTiki;
using CoinViewTiki.Interfaces;
using CoinViewTiki.Models;
using CoinViewTiki.Services;
using FluentAssertions;
using Prism.Navigation;
using Xunit;

namespace CoinViewTikiUnitTests
{
    public class CoinViewTikiUnitTests
    {
        private ICoinGeckoAPIManager _coinGeckoApiManager;
        private IAlertDialogService _alertDialogService;
        private INavigationService _navigationService;
        private CoinListPageViewModel _coinListVM;
        private MockConnectivity _mockConnectivity;
        private INavigationParameters _navigationParameters;
        
        public CoinViewTikiUnitTests()
        {
            _coinGeckoApiManager = new CoinGeckoAPIManager();
            _mockConnectivity = new MockConnectivity();
            
            _coinListVM = new CoinListPageViewModel(_navigationService,
                                                    _coinGeckoApiManager, 
                                                    _alertDialogService,
                                                    _mockConnectivity);
        }
        
       [Fact]
        public async Task CoinsListCanBeLoadedViaApiManagerTest()
        {
            List<Coin> listCoins = new List<Coin>();
            listCoins = await _coinGeckoApiManager.GetCoinsAsync();
            listCoins.Should().NotBeEmpty();
        } 
        
        [Fact]
        public async Task Init_CoinsListCanBeLoaded()
        {
            var mockNavigationParams = new NavigationParameters();
            _coinListVM.Initialize(mockNavigationParams);
            Thread.Sleep(6000);
            _coinListVM.Coins.Should().NotBeNull();
        } 
        
        [Fact]
        public async Task CoinsListShouldContainBitcoin()
        {
            List<Coin> listCoins = new List<Coin>();
             listCoins =  await _coinGeckoApiManager.GetCoinsAsync();
            listCoins.Should().Contain(x => x.Name.ToLower() == "bitcoin");
        }
        
        [Fact]
        public async Task CoinDetailShouldBeCorrect()
        {
            CoinData coinData = new CoinData();
            coinData =  await _coinGeckoApiManager.GetCoinDetailAsync("0-5x-long-algorand-token");
            coinData.Name.Should().Be("0.5X Long Algorand Token");
        }

        [Fact]
        public async Task CoinSymbolShouldBeCorrect()
        {
            CoinData coinData = new CoinData();
            coinData = await _coinGeckoApiManager.GetCoinDetailAsync("0-5x-long-algorand-token");
            coinData.Symbol.Should().Be("algohalf");
        }

        [Fact]
        public async Task SearchShouldContainBitcoin()
        {

            await _coinListVM.SearchTextAsync("Bitcoin");

            var coins = _coinListVM.Coins;

            coins.Should().NotBeEmpty();
        }

        
        
        
        
    }
}