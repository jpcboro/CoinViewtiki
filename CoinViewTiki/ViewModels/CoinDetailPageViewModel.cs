using CoinViewTiki.Models;

namespace CoinViewTiki.ViewModels
{
    public class CoinDetailPageViewModel : BaseViewModel
    {
        private CoinData _coinDetails;

        public CoinData CoinDetails
        {
            get => _coinDetails;
            set
            {
                _coinDetails = value;
                OnPropertyChanged();
            }
            
        }

        public CoinDetailPageViewModel(CoinData coinDetails)
        {
            CoinDetails = coinDetails;
        }
        
        
    }
}