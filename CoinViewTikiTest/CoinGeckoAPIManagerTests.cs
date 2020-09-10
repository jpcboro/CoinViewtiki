using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinViewTiki;
using CoinViewTiki.Interfaces;
using CoinViewTiki.Models;
using CoinViewTiki.Services;
using FluentAssertions;
using Moq;
using Prism.Navigation;
using Xamarin.Essentials;
using Xunit;

namespace CoinViewTikiTest
{
    public class CoinGeckoAPIManagerTests
    {
        private ICoinGeckoAPIManager _coinGeckoApiManager;
        private IAlertDialogService _alertDialogService;
        private INavigationService _navigationService;
        private CoinListPageViewModel _coinListVM;
        private MockConnectivity _mockConnectivity;
        
        public CoinGeckoAPIManagerTests()
        {
            _coinGeckoApiManager = new CoinGeckoAPIManager();
            _mockConnectivity = new MockConnectivity();

            _coinListVM = new CoinListPageViewModel(_navigationService,
                _coinGeckoApiManager, 
                _alertDialogService,
               _mockConnectivity);
        }

      
        [Fact]
        public async Task CoinsListCanBeLoadedTest()
        {
            List<Coin> listCoins = await _coinGeckoApiManager.GetCoinsAsync();
            listCoins.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CoinsListShouldContainBitcoin()
        {
            List<Coin> listCoins = await _coinGeckoApiManager.GetCoinsAsync();
            listCoins.Should().Contain(x => x.Name.ToLower() == "bitcoin");
        }

        [Fact]
        public async Task CoinDetailShouldBeCorrect()
        {
            CoinData coinData = await _coinGeckoApiManager.GetCoinDetailAsync("0-5x-long-algorand-token");
            coinData.Name.Should().Be("0.5X Long Algorand Token");
            coinData.Symbol.Should().Be("algohalf");
            coinData.image.Should().NotBeNull();
            coinData.description.Should().NotBeNull();
            coinData.market_data.current_price.usd.Should().BeGreaterOrEqualTo(0);

        }

        [Fact]
        public async Task CoinSymbolShouldBeCorrect()
        {
            CoinData coinData = await _coinGeckoApiManager.GetCoinDetailAsync("0-5x-long-algorand-token");
            coinData.Symbol.Should().Be("algohalf");
        }

      [Fact]
        public async Task SearchShouldContainBitcoin()
        {
            await _coinListVM.SearchTextAsync("Bitcoin");
            
            var coins = _coinListVM.Coins;

            var firstResults = coins.First().Items;

            firstResults.Should().NotBeEmpty();
            
        }
        
    }
}