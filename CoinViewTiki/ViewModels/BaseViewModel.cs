using System.ComponentModel;
using System.Runtime.CompilerServices;
using CoinViewTiki.Annotations;
using Prism.Mvvm;
using Prism.Navigation;

namespace CoinViewTiki
{
    public class BaseViewModel : BindableBase
    {
        // public event PropertyChangedEventHandler PropertyChanged;
        //
        // [NotifyPropertyChangedInvocator]
        // protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        // {
        //     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        // }

        private bool _isRunning;

        public bool IsRunning
        {
            get { return _isRunning; }
            set { SetProperty(ref _isRunning, value); }
        }
       
    }
}