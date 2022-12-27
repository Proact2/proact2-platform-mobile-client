using System;
using System.Threading.Tasks;
using Proact.Mobile.Core.Resources;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Proact.Mobile.Core {
    public class MicrophonePermissionService  : IMicrophonePermissionService {
        public MicrophonePermissionService() {
        }

        public async Task<bool> CheckMicrophonePermissionsGranted() {
            var status = await Permissions.CheckStatusAsync<Permissions.Microphone>();
            return status == PermissionStatus.Granted;
        }

        public async Task<bool> RequestMicrophonePermission() {
            var status = await Permissions.RequestAsync<Permissions.Microphone>();
            return status == PermissionStatus.Granted;
        }

        public async void ShowSettingsUIOnPermissionDenied() {
            bool openSettings = await Application.Current.MainPage
                 .DisplayAlert( AppResources.MicrophonePermissionExceptionTitle,
                 AppResources.MicrophonePermissionExceptionMessage,
                 AppResources.OpenSettings,
                 AppResources.Ok );

            if ( openSettings ) {
                AppInfo.ShowSettingsUI();
            }
        }
    }
}
