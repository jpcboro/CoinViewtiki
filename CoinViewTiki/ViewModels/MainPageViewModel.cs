using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using CoinViewTiki.Models;
using CoinViewTiki.Services;
using MvvmHelpers;

using Xamarin.Forms;

namespace CoinViewTiki.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly ICoinGeckoAPI _coinGeckoApi;
        private ObservableRangeCollection<Grouping<string, Coin>> _coins;
        public ObservableRangeCollection<Grouping<string, Coin>> Coins
        {
            get => _coins;
            set
            {
                _coins = value;
                OnPropertyChanged();
            }
        }
        

        private ICommand _searchCommand;

        public ICommand SearchCommand => _searchCommand ?? new Command<string>(async (text) =>
        {
            text = text ?? String.Empty;
         
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
                
                        Coins.Clear();
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

        public List<Coin> FilteredCoinList { get; set; }


        private string _searchText { get; set; }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                }
                
                OnPropertyChanged();
            }
        }

        public MainPageViewModel(ICoinGeckoAPI coinGeckoApi)
        {
            _coinGeckoApi = coinGeckoApi;
            Coins = new ObservableRangeCollection<Grouping<string, Coin>>();
            FilteredCoinList = new List<Coin>();
            Task.Run(async () => await GetCoinList());
        }

        public async Task GetCoinList()
        {
            var coinList = await _coinGeckoApi.GetCoins();
            
            var sortedCoins = from item in coinList
                orderby item.Name
                group item by item.Name[0].ToString().ToUpperInvariant()
                into itemGroup
                select new Grouping<string, Coin>(itemGroup.Key, itemGroup);

            _coins.ReplaceRange(sortedCoins);
            

        }
        
        
    }
}