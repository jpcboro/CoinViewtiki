using System;
using System.Threading.Tasks;
using CoinViewTiki.Interfaces;
using CoinViewTiki.Models;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Xamarin.Essentials;

namespace CoinViewTiki.ViewModels
{
    public class CoinDetailPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly ICoinGeckoAPIManager _coinGeckoApiManager;
        private readonly IAlertDialogService _alertDialogService;

        private CoinData _coinDetails;

        public CoinData CoinDetails
        {
            get { return _coinDetails; }
            set { SetProperty(ref _coinDetails, value); }
        }
        
        public DelegateCommand GoBackCommand { get; set; }
        

        public CoinDetailPageViewModel(INavigationService navigationService,
            ICoinGeckoAPIManager coinGeckoApiManager,
            IPageDialogService pageDialogService,
            IDialogService dialogService,
            IAlertDialogService alertDialogService)
        {
            _navigationService = navigationService;
            _coinGeckoApiManager = coinGeckoApiManager;
            _alertDialogService = alertDialogService;
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
                    _alertDialogService.ShowAlertMessage(title: "Error has occured",
                        message: ex.Message);
                    await  _navigationService.GoBackAsync();
                }
               
            }
            else
            {
                 _alertDialogService.ShowAlertMessage(title: "No internet",
                    message: "Please check your internet connection and try again.");
               
              await  _navigationService.GoBackAsync();
            }
            
        }
    }
}