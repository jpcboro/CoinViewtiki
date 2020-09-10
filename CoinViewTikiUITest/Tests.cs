using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace CoinViewTikiUITest
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }


        // [Test]
        // public void AppLaunches()
        // {
        //     app.Screenshot("First screen.");
        // }
        
        [Test]
        public void CoinListPageLaunches()
        {
            var coinListPageExists =  app.WaitForElement(x => x.Marked("CoinListPageID"));
            Assert.IsTrue(coinListPageExists.Any());
        }

        [Test]
        public void ScrollDownListToLastPart()
        {
            var coinListPageExists =  app.WaitForElement(x => x.Marked("CoinListPageID"));
            Assert.IsTrue(coinListPageExists.Any());
            app.ScrollDown(strategy:ScrollStrategy.Gesture);
            
        }

        [Test]
        public void ViewAndTapCoinListGoesToCoinDetailsPage()
        {
            
            var CoinListViewExists = app.Query(x => x.Marked("CoinListView")
                .Child());
            Assert.Greater(CoinListViewExists.Length, 0);

          
            var firstCellInListView = GetFirstCellInListView();
            
            app.Tap(firstCellInListView);
            
            Thread.Sleep(1000);
            var coinDetailPageExists = app.WaitForElement(c => c.Marked("CoinDetailPageID"));
            
            Assert.IsTrue(coinDetailPageExists.Any());
        }

        private Func<AppQuery, AppQuery> GetFirstCellInListView(int timeOutInSeconds = 15)
        {
            Func<AppQuery, AppQuery> firstRowInListView = null;

            if (platform == Platform.Android)
            {
                firstRowInListView = x => x.Class("ViewCellRenderer_ViewCellContainer").Index(1);
            }

            app.WaitForElement(firstRowInListView,
                "Timed out waiting for item",
                TimeSpan.FromSeconds(timeOutInSeconds));

            return firstRowInListView;

        }
    }
}
