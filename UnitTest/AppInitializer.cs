using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UnitTest {
    public class AppInitializer {
        public static IApp StartApp( Platform platform ) {
            if ( platform == Platform.Android ) {
                return ConfigureApp.Android.InstalledApp( "com.upsmart.proactbeta" ).StartApp();
            }

            return ConfigureApp
                .iOS
                .Debug()
                .InstalledApp( "com.upsmart.proactbeta" )
                //.AppBundle("/Users/micheleaddante/Project/Mobile/CobraClientKit/SLCobraClientKit/src/SLCobraClientKit.iOS/bin/iPhoneSimulator/Debug/device-builds/iphone 8-14.0/SLCobraClientKit.iOS.app")
                //.DeviceIdentifier("E4E985CC-837B-4A19-A15F-AE1BA7DDE61B") //iphone8 simulator
                .DeviceIdentifier( "464AEBDC-14E3-4EA1-AC0A-AF61B1296841" ) //iPhone 12 Mini
                //.PreferIdeSettings()
                //.Debug()
                .StartApp();
        }
    }
}