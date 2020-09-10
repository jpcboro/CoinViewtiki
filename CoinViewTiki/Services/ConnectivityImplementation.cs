using System;
using CoinViewTiki.Interfaces;
using Xamarin.Essentials;

namespace CoinViewTiki.Services
{
    public class ConnectivityImplementation : IConnectivity
    {
        public static IConnectivity Instance { get; set; } = new ConnectivityImplementation();
        public bool IsConnectedToInternet()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }
        
         event EventHandler<ConnectivityChangedEventArgs> IConnectivity.ConnectivityChanged
        {
            add => Connectivity.ConnectivityChanged += value;
            remove => Connectivity.ConnectivityChanged -= value;
        }

    }
}