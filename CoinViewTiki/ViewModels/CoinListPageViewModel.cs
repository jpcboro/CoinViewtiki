using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CoinViewTiki.Interfaces;
using CoinViewTiki.Models;
using CoinViewTiki.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Prism.Commands;
using Prism.Navigation;

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
        private ObservableRangeCollection<Grouping<string, Coin>> _coins;
         CoinGeckoAPIManager apiManager = new CoinGeckoAPIManager();
        public ObservableRangeCollection<Grouping<string, Coin>> Coins
        {
            get => _coins;
            set
            {
                _coins = value;
            }
        }
        

        private ICommand _searchCommand;

        public ICommand SearchCommand => _searchCommand ?? new Xamarin.Forms.Command<string>(async (text) =>
        {
            text = text ?? string.Empty;

                if (text.Length >= 1)
                {
                    Coins.Clear();
                    await GetCoinList();
                
                    var suggestions = Coins.Where(x => x.Items
                        .Any(p => p.Name.ToLowerInvariant().StartsWith(text.ToLowerInvariant()) )).ToList();


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
                            select new Grouping<string, Coin>(itemGroup.Key, itemGroup);
                
 
                        _coins.ReplaceRange(newSortedCoins);

                    }
                    else
                    {
                        Coins.Clear();
                    }
       
                }
                else
                {
                    Coins.Clear();
                    await GetCoinList();
                }

        });

        public DelegateCommand<object> ItemTappedCommand { get; set; }
        public List<Coin> FilteredCoinList { get; set; }


        private string _searchText;
        

        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

       

        public CoinListPageViewModel(INavigationService navigationService, 
                                     ICoinGeckoAPIManager coinGeckoApiManager)
        {
            _navigationService = navigationService;
            _coinGeckoApiManager = coinGeckoApiManager;
            Coins = new ObservableRangeCollection<Grouping<string, Coin>>();
            FilteredCoinList = new List<Coin>();
            
            ItemTappedCommand = new DelegateCommand<object>(ItemTapped);
        }

        private async void ItemTapped(object obj)
        {
            var coin = obj as Coin;
            var coinId = coin.Id;
            
            var navigationParams = new NavigationParameters();
            navigationParams.Add("selectedCoin", coinId);
            await _navigationService.NavigateAsync("CoinDetailPage", navigationParams);
        }

        public async Task GetCoinList()
        {
            IsRunning = true;
         
            var coinList = await apiManager.GetCoinsAsync();

            if (coinList != null)
            {
                var sortedCoins = from item in coinList
                    orderby item.Name
                    group item by item.Name[0].ToString().ToUpperInvariant()
                    into itemGroup
                    select new Grouping<string, Coin>(itemGroup.Key, itemGroup);

                _coins.ReplaceRange(sortedCoins);
            }


            IsRunning = false;
            
        }


        public void Initialize(INavigationParameters parameters)
        {
           
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            SearchText = null;
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            Task.Run(async () => await GetCoinList());
        }
    }
}