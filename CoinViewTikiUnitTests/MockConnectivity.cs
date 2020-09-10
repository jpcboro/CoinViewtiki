using System;
using CoinViewTiki.Interfaces;
using Xamarin.Essentials;

namespace CoinViewTikiUnitTests
{
    public class MockConnectivity : IConnectivity
    {
        public bool IsConnectedToInternet() => true;
      

        public event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;
    }
}