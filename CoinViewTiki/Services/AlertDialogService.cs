using System.Threading;
using System.Threading.Tasks;
using CoinViewTiki.Interfaces;
using Prism.Services.Dialogs;
using Xamarin.Forms;

namespace CoinViewTiki.Services
{
    public class AlertDialogService : IAlertDialogService
    {
        private readonly IDialogService _dialogService;

        public AlertDialogService(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }
        public void ShowAlertMessage(string title, string message)
        {
            var parameters = new DialogParameters
            {
                {"title", title},
                {"message", message}
            };
           
            Device.BeginInvokeOnMainThread(() =>
            {
                _dialogService.ShowDialog("AlertDialog", parameters);

            });
            
        }
    }
}