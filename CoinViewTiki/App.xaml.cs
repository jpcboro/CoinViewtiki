using System;
using CoinViewTiki.Constants;
using CoinViewTiki.Interfaces;
using CoinViewTiki.Services;
using CoinViewTiki.ViewModels;
using CoinViewTiki.Views;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoinViewTiki
{
    public partial class App
    {
        public App(IPlatformInitializer platformInitializer = null) : base(platformInitializer){}
        

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<CoinListPage, CoinListPageViewModel>();
            containerRegistry.RegisterForNavigation<CoinDetailPage, CoinDetailPageViewModel>();
            containerRegistry.Register<ICoinGeckoAPIManager, CoinGeckoAPIManager>();
            containerRegistry.RegisterForNavigation<TestPage, TestPageViewModel>();
        }

        protected override async void OnInitialized()
        {
            // Device.SetFlags(new[] { "Brush_Experimental" });

            InitializeComponent();

            await NavigationService.NavigateAsync("CoinListPage");
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

    

        protected override void OnResume()
        {
        }
    }
}
