using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaManager;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using OneSignalSDK.Xamarin;
using OneSignalSDK.Xamarin.Core;
using Proact.UI.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation( XamlCompilationOptions.Compile )]
namespace Proact.UI {
    public partial class App : Application {

        private string _androidAppCenterSecretKey = "7f96be62-17bd-49d4-bbef-18920338c777";
        private string _iosAppCenterSecretKey = "79861d2d-ba08-4813-8e5a-e44713fbee61";
        private string _oneSignalAppId = "7e2f4f83-bc0d-4d67-b637-8c71040c2914";

        private string _openMessageDetailNotificationKey = "OpenMessageDetail";
        private string _openAssignedSurveyNotificationKey = "OpenAssignedSurvey";
        private string _openNotCompiledSurveysNotificationKey = "OpenNotCompiledSurveys";

        public App() {
            InitializeComponent();

            AppThemeHelper.ChangeTheme( Application.Current.RequestedTheme );
            Application.Current.RequestedThemeChanged += ( s, a ) => {
                AppThemeHelper.ChangeTheme( a.RequestedTheme );
            };

            InitOneSignalPushNotification();
        }

        protected override void OnStart() {
            base.OnStart();

            AppCenter.Start(
                "android=" + _androidAppCenterSecretKey + ";" +
                "ios=" + _iosAppCenterSecretKey + ";",
                typeof( Analytics ), typeof( Crashes ) );
        }

        protected override void OnSleep() {
            base.OnSleep();
            try {
                CrossMediaManager.Current.Pause();
            }
            catch ( Exception ex ) { };

        }

        private void InitOneSignalPushNotification() {
            OneSignal.Default.Initialize( _oneSignalAppId );
            OneSignal.Default.PromptForPushNotificationsWithUserResponse();
            OneSignal.Default.NotificationOpened += HandleNotificationOpened;
        }

        private void HandleNotificationOpened( NotificationOpenedResult result ) {
            try {
                var rawPayload = result.notification.rawPayload;
                var payload = ( Dictionary<string, object> )Json.Deserialize( rawPayload );
                var rawCustom = payload["custom"].ToString();
                var customData = ( Dictionary<string, object> )Json.Deserialize( rawCustom );
                var additionalData = ( Dictionary<string, object> )customData["a"];

                if ( additionalData != null ) {
                    CheckOpenMessageDetail( additionalData );
                    CheckOpenSurveyDetail( additionalData );
                    CheckOpenNotCompiledSurveysList( additionalData );
                }
            }
            catch ( Exception ) { }
        }

        private void CheckOpenMessageDetail( Dictionary<string, object> notificationData ) {
            try {
                if ( notificationData.ContainsKey(
                    _openMessageDetailNotificationKey ) ) {

                    Guid messageId;
                    Guid.TryParse( notificationData[_openMessageDetailNotificationKey].ToString(), out messageId );

                    WallMessagesPage wallMessagesPage = GetWallMessagesPage();
                    wallMessagesPage.ViewModel.OnPushNotificationNewMessageReceived( messageId );
                }
            }
            catch ( Exception ) { }
        }

        private void CheckOpenSurveyDetail( Dictionary<string, object> notificationData ) {
            try {
                if ( notificationData.ContainsKey(
                    _openAssignedSurveyNotificationKey ) ) {

                    Guid assignationId;
                    Guid.TryParse(
                        notificationData[_openAssignedSurveyNotificationKey].ToString(),
                        out assignationId );

                    WallMessagesPage wallMessagesPage = GetWallMessagesPage();
                    wallMessagesPage.ViewModel.OpenNotCompiledSurvey( assignationId );
                }
            }
            catch ( Exception ) { }
        }

        private void CheckOpenNotCompiledSurveysList( Dictionary<string, object> notificationData ) {
            try {
                if ( notificationData.ContainsKey(
                    _openNotCompiledSurveysNotificationKey ) ) {
                    WallMessagesPage wallMessagesPage = GetWallMessagesPage();
                    wallMessagesPage.ViewModel.OpenNotCompiledSurveysList();
                }
            }
            catch ( Exception ex ) { }
        }

        private Page GetCurrentPage() {
            var currentPage = Application.Current.MainPage.Navigation
                .NavigationStack.Last();
            if ( Application.Current.MainPage.Navigation.ModalStack.Count > 0 ) {
                var currentModal = Application.Current.MainPage.Navigation
                    .ModalStack.Last();
                if ( currentModal.GetType() == typeof( NavigationPage )
                        || currentModal.GetType() == typeof( WallMessagesPage ) ) {
                    currentPage = ( ( NavigationPage )currentModal ).CurrentPage;
                }
            }
            return currentPage;
        }

        private WallMessagesPage GetWallMessagesPage() {
            foreach ( Page page in Application.Current.MainPage.Navigation.NavigationStack ) {
                if ( page.GetType() == typeof( WallMessagesPage ) ) {
                    return ( WallMessagesPage )page;
                }
            }
            return null;
        }

    }

  

}
