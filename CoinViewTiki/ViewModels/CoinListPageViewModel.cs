using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CoinViewTiki.Interfaces;
using CoinViewTiki.Models;
using CoinViewTiki.Services;
using MvvmHelpers;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Xamarin.Essentials;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace CoinViewTiki
{
    public class CoinListPageViewModel : BaseViewModel, IInitialize, INavigationAware
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        
        private readonly INavigationService _navigationService;
        private readonly ICoinGeckoAPIManager _coinGeckoApiManager;
        private readonly IAlertDialogService _alertDialogService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IConnectivity _connectivity;

        private ObservableRangeCollection<Grouping<string, MarketUSDCoin>> _coins;
        
        public ObservableRangeCollection<Grouping<string, MarketUSDCoin>> Coins
        {
            get => _coins;
            set
            {
                _coins = value;
            }
        }
        

        private ICommand _searchCommand;

        public DelegateCommand<string> SearchCommand => 
            new DelegateCommand<string>(
                async (text) => await SearchTextAsync(text));
     

        public async Task SearchTextAsync(string text)
        {
            text = text ?? string.Empty;

            if (text.Length >= 1)
            {

                if (!_coinsCachedPresent)
                {
                    await RefreshCoinsList();
                }
                else
                {
                    await GetCoinList();

                }
              
            
                if (Coins.Any())
                {
                    var suggestions = Coins.Where(x => x.Items
                        .Any(p => p.Name.ToLowerInvariant().StartsWith(text.ToLowerInvariant()))).ToList();


                    if (suggestions.Any())
                    {
                        foreach (var coin in suggestions)
                        {
                            FilteredCoinList = (from list in coin
                                where list.Name.ToLowerInvariant().StartsWith(text.ToLowerInvariant())
                                select list).ToList();
                        }

                        var newSortedCoins = from item in FilteredCoinList
                            orderby item.Name
                            group item by item.Name[0].ToString().ToUpperInvariant()
                            into itemGroup
                            select new Grouping<string, MarketUSDCoin>(itemGroup.Key, itemGroup);


                        _coins.ReplaceRange(newSortedCoins);
                    }
                    else
                    {
                        Coins.Clear();
                    }
                }
                else
                {
                    //offline or an error occured in fetching coins
                    //clear search
                   
                    SearchText = string.Empty;
                }
            }
            else
            {
                Coins.Clear();
                await GetCoinList();
            }
        }

        public DelegateCommand<object> ItemTappedCommand { get; set; }

        private bool _isRefreshing;

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }
        public ICommand RefreshCommand => new Command(async () =>
        {
            await RefreshCoinsList();
        });

        private async Task RefreshCoinsList()
        {
            IsRefreshing = true;

            if (_connectivity.IsConnectedToInternet())
            {
                await GetCoinList(isForceRefresh: true);
            }
            else
            {
                _alertDialogService.ShowAlertMessage(title: "No internet",
                    message: "Please check your internet connection and try again.");
            }

            IsRefreshing = false;
        }

        public List<MarketUSDCoin> FilteredCoinList { get; set; }


        private string _searchText;
        

        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

       

        public CoinListPageViewModel(INavigationService navigationService, 
                                     ICoinGeckoAPIManager coinGeckoApiManager,
                                     IAlertDialogService alertDialogService,
                                    IPageDialogService pageDialogService,
                                     IConnectivity connectivity)
        {
            _navigationService = navigationService;
            _coinGeckoApiManager = coinGeckoApiManager;
            _alertDialogService = alertDialogService;
            _pageDialogService = pageDialogService;
            _connectivity = connectivity;

            Coins = new ObservableRangeCollection<Grouping<string, MarketUSDCoin>>();
            FilteredCoinList = new List<MarketUSDCoin>();
            
            ItemTappedCommand = new DelegateCommand<object>(ItemTapped);
            
        }

       

        private async void ItemTapped(object obj)
        {
            if (isTapped)
            {
                return;
            }

            isTapped = true;
            var coin = obj as Coin;
            var coinId = coin.Id;
            
            var navigationParams = new NavigationParameters();
            navigationParams.Add("selectedCoin", coinId);
            await _navigationService.NavigateAsync("CoinDetailPage", navigationParams);

            isTapped = false;
        }

        private bool isTapped = false;
        private bool _coinsCachedPresent = false;

        private async Task GetCoinList(bool isForceRefresh = false)
        {
            List<MarketUSDCoin> coinList;
            try
            {
                IsRunning = true;

                coinList = await _coinGeckoApiManager.GetCoinsViaUSDMarketAsync(forceRefresh:isForceRefresh);

                if (coinList != null)
                {
                    _coinsCachedPresent = true;
                    var sortedCoins = from item in coinList
                        orderby item.Name
                        group item by item.Name[0].ToString().ToUpperInvariant()
                        into itemGroup
                        select new Grouping<string, MarketUSDCoin>(itemGroup.Key, itemGroup);

                    _coins.ReplaceRange(sortedCoins);
                }
                else
                {
                    _coinsCachedPresent = false;
                }

                IsRunning = false;  
            }
            catch (Exception ex)
            {
                Coins.Clear();
                    
                _alertDialogService.ShowAlertMessage(title: "Error",
                    message: $"Something went wrong: {ex.Message} ");

                IsRunning = false;
            }

            
        }

        public void Initialize(INavigationParameters parameters)
        {
          
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            SearchText = null;
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (!Coins.Any())
            {
                if (_connectivity.IsConnectedToInternet())
                {
                    Task.Run(async () => await RefreshCoinsList());

                }
                else
                {
                   await _pageDialogService.DisplayAlertAsync("No Internet Connection Detected", "Please connect your device to an internet service and refresh. App will now use device cache if it is present.", "Ok");

                    Task.Run(async () => await GetCoinList(isForceRefresh: false));


                }
                
            }
        }
    }
}