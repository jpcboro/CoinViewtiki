using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinViewTiki.Services;
using Refit;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoinViewTiki
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

          await  GetCoins();

        }

        private async Task GetCoins()
        {
            var apiResponse = RestService.For<ICoinGeckoAPI>("https://api.coingecko.com/");
            var coins = await apiResponse.GetCoins();
            coinsCollectionView.ItemsSource = coins;
        }

     
    }
}
