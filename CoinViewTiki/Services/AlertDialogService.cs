using System.Threading.Tasks;
using CoinViewTiki.Interfaces;
using Prism.Services.Dialogs;

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
           
            _dialogService.ShowDialog("AlertDialog", parameters);
            
        }
    }
}