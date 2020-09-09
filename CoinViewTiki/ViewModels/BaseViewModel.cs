using System.ComponentModel;
using System.Runtime.CompilerServices;
using CoinViewTiki.Annotations;
using Prism.Mvvm;
using Prism.Navigation;

namespace CoinViewTiki
{
    public class BaseViewModel : BindableBase
    {
        
        private bool _isRunning;

        public bool IsRunning
        {
            get { return _isRunning; }
            set { SetProperty(ref _isRunning, value); }
        }
       
    }
}