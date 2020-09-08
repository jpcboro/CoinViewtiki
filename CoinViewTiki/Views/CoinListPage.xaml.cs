using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoinViewTiki
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoinListPage : ContentPage
    {
        public CoinListPage()
        {
            InitializeComponent();

            coinListView.ItemSelected += DeselectItem;
        }

        private void DeselectItem(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
  
        }
    }
}