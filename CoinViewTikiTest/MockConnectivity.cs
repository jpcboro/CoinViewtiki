using System;
using CoinViewTiki.Interfaces;
using Xamarin.Essentials;

namespace CoinViewTikiTest
{
    public class MockConnectivity : IConnectivity
    {
        public bool IsConnectedToInternet() => true;
        public event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;
    }
}