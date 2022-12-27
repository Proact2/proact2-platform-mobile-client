using System;
using NUnit.Framework;
using Xamarin.UITest;

namespace Proact.UITest {

    [TestFixture( Platform.Android )]
    [TestFixture( Platform.iOS )]
    public class BaseTest {

        protected IApp app;
        protected Platform platform;
        protected bool OniOS => platform == Platform.iOS;
        protected bool OnAndroid => platform == Platform.Android;

        public BaseTest( Platform platform ) {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest() {
            app = AppInitializer.StartApp( platform );
        }
    }
}
