using System.Threading.Tasks;
using CoinViewTiki.Interfaces;
using CoinViewTiki.Models;
using Prism.Commands;
using Prism.Navigation;

namespace CoinViewTiki.ViewModels
{
    public class CoinDetailPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly ICoinGeckoAPIManager _coinGeckoApiManager;
        // private CoinData _coinDetails;
        //
        // public CoinData CoinDetails
        // {
        //     get => _coinDetails;
        //     set
        //     {
        //         _coinDetails = value;
        //         OnPropertyChanged();
        //     }
        //     
        // }

        private CoinData _coinDetails;

        public CoinData CoinDetails
        {
            get { return _coinDetails; }
            set { SetProperty(ref _coinDetails, value); }
        }
        
        public DelegateCommand GoBackCommand { get; set; }

        // public CoinDetailPageViewModel(CoinData coinDetails)
        // {
        //     CoinDetails = coinDetails;
        // }

        public CoinDetailPageViewModel(INavigationService navigationService,
            ICoinGeckoAPIManager coinGeckoApiManager)
        {
            _navigationService = navigationService;
            _coinGeckoApiManager = coinGeckoApiManager;
            GoBackCommand = new DelegateCommand(GoBack);
        }

        private async void GoBack()
        {
           await _navigationService.GoBackAsync();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            var coinDetails = parameters["selectedCoin"] as string;

            await GetCoinDetails(coinDetails);
            
        }

        private async Task GetCoinDetails(string coinDetails)
        {
            IsRunning = true;
            CoinDetails = await _coinGeckoApiManager.GetCoinDetailAsync(coinDetails);
            IsRunning = false;
        }
    }
}