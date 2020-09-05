﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinViewTiki.Models;
using CoinViewTiki.Services;
using CoinViewTiki.Views;
using MvvmHelpers;
using Refit;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoinViewTiki
{
    public partial class MainPage : ContentPage
    {
        private readonly ObservableRangeCollection<Grouping<string,Coin>> _coins = new ObservableRangeCollection<Grouping<string, Coin>>();
        public ObservableCollection<Grouping<string, Coin>> Coins => _coins;
        public MainPage()
        {
            InitializeComponent();
            
            apiResponse = RestService.For<ICoinGeckoAPI>("https://api.coingecko.com/");
        }

        public ICoinGeckoAPI apiResponse { get; set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

          await  GetCoins();

        }

    
        private async Task GetCoins()
        {
           
            var coins = await apiResponse.GetCoins();
            var sortedCoins = from item in coins
                orderby item.Name
                group item by item.Name[0].ToString().ToUpperInvariant()
                into itemGroup
                select new Grouping<string, Coin>(itemGroup.Key, itemGroup);
            
            _coins.ReplaceRange(sortedCoins);
           
            // coinsCollectionView.ItemsSource = Coins;
            // coinsCollectionView.IsGrouped = true;

            coinListView.ItemsSource = Coins;

        }


        private async void CoinListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedCoin = e.SelectedItem as Coin;

            var coinData = await apiResponse.GetCoinData(selectedCoin.Id);

            Navigation.PushAsync(new CoinDetailPage(coinData));

        }
    }
}
