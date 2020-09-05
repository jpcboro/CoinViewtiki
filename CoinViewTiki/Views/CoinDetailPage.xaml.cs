using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinViewTiki.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoinViewTiki.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoinDetailPage : ContentPage
    {
        private CoinData _coinData;

        public CoinDetailPage(CoinData coinData)
        {
            InitializeComponent();

            _coinData = coinData;

            img.Source = coinData.image.thumb;

            lblName.Text = coinData.Name;

            lblSymbol.Text = coinData.Symbol;

            lblPrice.Text = coinData.market_data.current_price.usd.ToString();

            lblDesc.Text = coinData.description.en;


        }
    }
}