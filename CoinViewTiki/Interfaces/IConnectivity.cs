using System;
using Xamarin.Essentials;

namespace CoinViewTiki.Interfaces
{
    public interface IConnectivity
    {
        bool IsConnectedToInternet();
        event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;
    }
}