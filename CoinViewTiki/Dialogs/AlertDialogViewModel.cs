using System;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace CoinViewTiki
{
    public class AlertDialogViewModel : BaseViewModel, IDialogAware
    {

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private string _message;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
        public DelegateCommand CloseDialogCommand { get; set; }
        
        public AlertDialogViewModel()
        {
            CloseDialogCommand = new DelegateCommand(()=> RequestClose(null));
        }
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
          
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters.GetValue<string>("message");
            Title = parameters.GetValue<string>("title");
        }

        public event Action<IDialogParameters> RequestClose;
    }
}