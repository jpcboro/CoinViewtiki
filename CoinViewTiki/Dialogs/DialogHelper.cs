using Prism.Services.Dialogs;

namespace CoinViewTiki
{
    public static class DialogHelper
    {
        public static void ShowAlertMessage(this IDialogService dialogService, string title, string message)
        {
            var parameters = new DialogParameters
            {
                {"title", title},
                {"message", message}
            };
            
            dialogService.ShowDialog("AlertDialog", parameters);
        }
    }
}