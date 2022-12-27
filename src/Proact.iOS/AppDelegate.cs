using System;
using System.Linq;
using AVFoundation;
using Foundation;
using MediaManager;
using Microsoft.Identity.Client;
using MvvmCross.Forms.Platforms.Ios.Core;
using Proact.UI;
using UIKit;
using UserNotifications;

namespace Proact.Mobile.iOS {
    [Register( nameof( AppDelegate ) )]
    public partial class AppDelegate : MvxFormsApplicationDelegate<Setup, Core.App, App> {
        public override bool FinishedLaunching( UIApplication uiApplication, NSDictionary launchOptions ) {

            //ios ui test init
            //Xamarin.Calabash.Start();
            Rg.Plugins.Popup.Popup.Init();
            Stormlion.PhotoBrowser.iOS.Platform.Init();
            CrossMediaManager.Current.Init();
            Plugin.InputKit.Platforms.iOS.Config.Init();

            Xamarin.Forms.DependencyService.Register<FileSettingsManagerIos>();
            Xamarin.Forms.DependencyService.Register<Localization>();
            Xamarin.Forms.DependencyService.Register<BackgroundUploadMediaService>();
            Xamarin.Forms.DependencyService.Register<LocalNotificationManager>();
            Xamarin.Forms.DependencyService.Register<AudioSession>();

            // Set a delegate to handle incoming notifications.
            UNUserNotificationCenter.Current.Delegate = new LocalNotificationReciever();

            var result = base.FinishedLaunching( uiApplication, launchOptions );

            global::Xamarin.Forms.FormsMaterial.Init();
          
            return result;
        }

        public override bool OpenUrl( UIApplication app, NSUrl url, NSDictionary options ) {
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs( url );
            return base.OpenUrl( app, url, options );
        }



    }
}
