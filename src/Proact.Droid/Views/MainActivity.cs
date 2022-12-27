using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Forms.Platforms.Android.Views;
using Plugin.CurrentActivity;
using Proact.Mobile.Core.ViewModels.Main;
using Xamarin.Forms;
using MvvmCross.Forms;
using Microsoft.Identity.Client;
using Proact.Mobile.Core;
using AndroidX.Core.Content;
using AndroidX.Core.App;
using Android;
using Android.Content.PM;
using MediaManager;

namespace Proact.Mobile.Droid {
    [Activity(
        Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = Android.Content.PM.ConfigChanges.UiMode )]
    public class MainActivity : MvxFormsAppCompatActivity<MainViewModel> {
        protected override void OnCreate( Bundle bundle ) {
            TabLayoutResource = MvvmCross.Forms.Resource.Layout.Tabbar;
            ToolbarResource = MvvmCross.Forms.Resource.Layout.Toolbar;

            CrossCurrentActivity.Current.Init( this, bundle );
            Xamarin.Forms.FormsMaterial.Init( this, bundle );
            Xamarin.Essentials.Platform.Init( this, bundle );
            Stormlion.PhotoBrowser.Droid.Platform.Init( this );
            CrossMediaManager.Current.Init( this );

            base.OnCreate( bundle );
        
            Rg.Plugins.Popup.Popup.Init( this );
            Plugin.InputKit.Platforms.Droid.Config.Init( this, bundle );
            App.UIParent = this;
        }

        public override void OnRequestPermissionsResult( int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults ) {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult( requestCode, permissions, grantResults );
            base.OnRequestPermissionsResult( requestCode, permissions, grantResults );
        }

        protected override void OnActivityResult( int requestCode, Result resultCode, Intent data ) {
            base.OnActivityResult( requestCode, resultCode, data );
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs( requestCode, resultCode, data );
        }
    }
}
