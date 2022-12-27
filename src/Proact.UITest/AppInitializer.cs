using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Proact.UITest {
    public class AppInitializer {

        private const string APP_BUNDLE_ID = "com.studioleaves.startupkit";
        private const string APP_PATH = "../../../BinariesForUITest/SLCobraClientKit.iOS.app";

        public static IApp StartApp( Platform platform ) {
            // TODO: If the iOS or Android app being tested is included in the solution 
            // then open the Unit Tests window, right click Test Apps, select Add App Project
            // and select the app projects that should be tested.
            //
            // The iOS project should have the Xamarin.TestCloud.Agent NuGet package
            // installed. To start the Test Cloud Agent the following code should be
            // added to the FinishedLaunching method of the AppDelegate:
            //
            //    #if ENABLE_TEST_CLOUD
            //    Xamarin.Calabash.Start();
            //    #endif
            if ( platform == Platform.Android ) {
                return ConfigureApp
                    .Android
                    .InstalledApp( APP_BUNDLE_ID )
                    // TODO: Update this path to point to your Android app and uncomment the
                    // code if the app is not included in the solution.
                    //.ApkFile ("../../../Droid/bin/Debug/xamarinforms.apk")
                    .StartApp();
            }

            return ConfigureApp
                .iOS
                //.InstalledApp( APP_BUNDLE_ID )
                // TODO: Update this path to point to your iOS app and uncomment the
                // code if the app is not included in the solution.
                .AppBundle( APP_PATH )
                .StartApp();
        }
    }
}
