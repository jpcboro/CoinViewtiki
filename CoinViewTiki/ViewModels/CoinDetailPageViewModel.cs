using System;
using System.Threading.Tasks;
using CoinViewTiki.Interfaces;
using CoinViewTiki.Models;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;

namespace CoinViewTiki.ViewModels
{
    public class CoinDetailPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly ICoinGeckoAPIManager _coinGeckoApiManager;
        private readonly IPageDialogService _pageDialogService;

        private CoinData _coinDetails;

        public CoinData CoinDetails
        {
            get { return _coinDetails; }
            set { SetProperty(ref _coinDetails, value); }
        }
        
        public DelegateCommand GoBackCommand { get; set; }
        

        public CoinDetailPageViewModel(INavigationService navigationService,
            ICoinGeckoAPIManager coinGeckoApiManager,
            IPageDialogService pageDialogService
            )
        {
            _navigationService = navigationService;
            _coinGeckoApiManager = coinGeckoApiManager;
            _pageDialogService = pageDialogService;
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
            
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    IsRunning = true;
                    CoinDetails = await _coinGeckoApiManager.GetCoinDetailAsync(coinDetails);
                    IsRunning = false;
                }
                catch (Exception ex)
                {
                    await _pageDialogService.DisplayAlertAsync("Error has occured",
                        ex.Message,
                        "Ok");
                }
               
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync("No Internet",
                    "Please check your internet connection.",
                    "Ok");
            }
            
        }
    }
}