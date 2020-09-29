using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoinViewTiki;
using CoinViewTiki.Interfaces;
using CoinViewTiki.Models;
using CoinViewTiki.Services;
using FluentAssertions;
using Moq;
using Prism.Common;
using Prism.Navigation;
using Prism.Services;
using Xunit;

namespace CoinViewTikiUnitTests
{
    public class CoinViewTikiUnitTests
    {
        private ICoinGeckoAPIManager _coinGeckoApiManager;
        private IAlertDialogService _alertDialogService;
        private INavigationService _navigationService;
        private IPageDialogService _pageDialogService;
        private MockConnectivity _mockConnectivity;
        private CoinListPageViewModel _coinListVM;

        
        public CoinViewTikiUnitTests()
        {
            _coinGeckoApiManager = new CoinGeckoAPIManager();
            _alertDialogService = new Mock<IAlertDialogService>().Object;
            _navigationService = new Mock<INavigationService>().Object;
            _pageDialogService = new Mock<IPageDialogService>().Object;
            _mockConnectivity = new MockConnectivity();
            _coinListVM = new CoinListPageViewModel(_navigationService,
                                                    _coinGeckoApiManager, 
                                                    _alertDialogService,
                                                    _pageDialogService,
                                                    _mockConnectivity
                                                    );
        }
        
        
        [Fact]
        public async Task CoinsMarketUSDListCanBeLoadedViaApiManagerTest()
        {
            List<MarketUSDCoin> listCoins = new List<MarketUSDCoin>();
            listCoins = await _coinGeckoApiManager.GetCoinsViaUSDMarketAsync();
            listCoins.Should().NotBeEmpty();
        } 
        
        
        [Fact]
        public async Task CoinsMarketUSDListShouldContainBitcoin()
        {
            List<MarketUSDCoin> listCoins = new List<MarketUSDCoin>();
            listCoins =  await _coinGeckoApiManager.GetCoinsViaUSDMarketAsync();
            listCoins.Should().Contain(x => x.Name.ToLower() == "bitcoin");
        }
        
        [Fact]
        public async Task CoinDetailShouldBeCorrect()
        {
            CoinData coinData = new CoinData();
            coinData =  await _coinGeckoApiManager.GetCoinDetailAsync("0-5x-long-algorand-token");
            coinData.Name.Should().Be("0.5X Long Algorand Token");
            coinData.Symbol.Should().Be("algohalf");
            coinData.image.Should().NotBeNull();
            coinData.description.Should().NotBeNull();
            coinData.market_data.current_price.usd.Should().BeGreaterOrEqualTo(0);
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