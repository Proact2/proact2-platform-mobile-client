using System;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Proact.Mobile.Core.Resources;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Proact.Mobile.Core {
    public class TakeMediaService : ITakeMediaService {

        private int _videoDurationInSeconds = 30;

        public async Task<MediaFile> PickPhoto() {
            await CrossMedia.Current.Initialize();
            MediaFile file = null;

            if ( !CrossMedia.Current.IsPickPhotoSupported ) {
                await Application.Current.MainPage.DisplayAlert(
                    AppResources.PickPhotoUnsupportedTitle,
                    AppResources.PickPhotoUnsupportedMessage,
                    AppResources.Ok );
                return null;
            }

            try {
                file = await CrossMedia.Current
                    .PickPhotoAsync( new PickMediaOptions {
                        PhotoSize = PhotoSize.Medium,
                        RotateImage = true,
                        SaveMetaData = false
                    } );
            }
            catch ( MediaPermissionException ) {
                bool openSettings = await Application.Current.MainPage
                    .DisplayAlert( AppResources.PickPhotoUnsupportedTitle,
                    AppResources.PickPhotoPermissionExceptionMessage,
                    AppResources.OpenSettings,
                    AppResources.Ok );

                if ( openSettings ) {
                    AppInfo.ShowSettingsUI();
                }

                return null;
            }

            return file;
        }

        public async Task<MediaFile> TakePhoto() {
            await CrossMedia.Current.Initialize();
            MediaFile file = null;

            if ( !CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported ) {

                await Application.Current.MainPage.DisplayAlert(
                    AppResources.TakePhotoCameraUnsupportedTitle,
                    AppResources.TakePhotoCameraUnsupportedMessage,
                    AppResources.Ok );
                return null;
            }

            try {
                file = await CrossMedia.Current
                    .TakePhotoAsync( new StoreCameraMediaOptions {
                        Directory = AppResources.AppName,
                        PhotoSize = PhotoSize.Medium,
                        DefaultCamera = CameraDevice.Rear,
                        RotateImage = true,
                        AllowCropping = true,
                        SaveMetaData = false
                    } );
            }
            catch ( MediaPermissionException e ) {
                bool openSettings = await Application.Current.MainPage.DisplayAlert(
                    AppResources.TakePhotoCameraUnsupportedTitle,
                    AppResources.TakePhotoCameraPermissionExceptionMessage,
                    AppResources.OpenSettings,
                    AppResources.Ok );

                if ( openSettings ) {
                    AppInfo.ShowSettingsUI();
                }

                return null;
            }

            return file;
        }

        public Task<MediaFile> TakeVideoAsync() {
            var options = GetVideoOptions( _videoDurationInSeconds );
            return TakeVideoAsyncWithOptions( options );
        }

        private async Task<MediaFile> TakeVideoAsyncWithOptions( StoreVideoOptions options ) {
            await CrossMedia.Current.Initialize();
            MediaFile file = null;

            if ( !CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported ) {
                await Application.Current.MainPage.DisplayAlert(
                    AppResources.TakePhotoCameraUnsupportedTitle,
                    AppResources.TakePhotoCameraUnsupportedMessage,
                    AppResources.Ok );

                return null;
            }

            try {
                file = await CrossMedia.Current.TakeVideoAsync( options );
            }
            catch ( MediaPermissionException e ) {
                bool openSettings = await Application.Current.MainPage.DisplayAlert(
                    AppResources.TakePhotoCameraUnsupportedTitle,
                    AppResources.TakePhotoCameraPermissionExceptionMessage,
                    AppResources.OpenSettings,
                    AppResources.Ok );

                if ( openSettings ) {
                    AppInfo.ShowSettingsUI();
                }

                return null;
            }

            return file;
        }

        private StoreVideoOptions GetVideoOptions( int videoDurationInSeconds ) {
            var videoQuality = VideoQuality.High;
            
            if ( Device.RuntimePlatform == Device.iOS ) {
                videoQuality = VideoQuality.High;
            }

            var options = new StoreVideoOptions {
                Directory = AppResources.AppName,
                Quality = videoQuality,
                DefaultCamera = CameraDevice.Front,
                RotateImage = true,
                AllowCropping = true,
                SaveMetaData = false
            };

            if ( videoDurationInSeconds != 0 ) {
                options.DesiredLength = TimeSpan.FromSeconds( videoDurationInSeconds );
            }

            return options;
        }

        public async Task<string> GetBase64( MediaFile iMediaFile ) {
            var stream = iMediaFile.GetStreamWithImageRotatedForExternalStorage();
            var bytes = new byte[stream.Length];

            await stream.ReadAsync( bytes, 0, (int)stream.Length );
            return Convert.ToBase64String( bytes );
        }
    }
}
