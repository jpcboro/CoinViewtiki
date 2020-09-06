using Prism.Commands;
using Prism.Navigation;

namespace CoinViewTiki.ViewModels
{
    public class TestPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        
        public DelegateCommand GoBackCommand { get; set; }

        public TestPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            
            GoBackCommand = new DelegateCommand(GoBack);
            
        }

        private void GoBack()
        {
            _navigationService.GoBackAsync();
        }
    }
}