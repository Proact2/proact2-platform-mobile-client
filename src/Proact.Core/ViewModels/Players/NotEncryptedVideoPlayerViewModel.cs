using System;
using MediaManager;
using MediaManager.Library;
using MvvmCross.Commands;
using Proact.Mobile.Core.Models;

namespace Proact.Mobile.Core.ViewModels {
    public class NotEncryptedVideoPlayerViewModel : BaseViewModel<MediaFileDecryptModel> {

        private MediaFileDecryptModel _mediaFileDecryptModel;
        public MediaFileDecryptModel MediaFileDecryptModel {
            get => _mediaFileDecryptModel;
            set => SetProperty( ref _mediaFileDecryptModel, value );
        }

        public override void Prepare( MediaFileDecryptModel mediaFileDecrypt ) {
            MediaFileDecryptModel = mediaFileDecrypt;
            InitUI();
        }

        private void InitUI() {
            PageTitle = Resources.AppResources.VideoPlayerPageTitle;
        }

        public async void PlayVideo() {
            var mediaitem = await CrossMediaManager.Current.Extractor
                .CreateMediaItem( MediaFileDecryptModel.ContentUrl );
            mediaitem.MediaType = MediaManager.Library.MediaType.Video;

            await CrossMediaManager.Current.Play( mediaitem );
        }

        public void StopVideo() {
            var player = CrossMediaManager.Current.MediaPlayer;
            player.Stop();
        }

        public override void ViewAppeared() {
            base.ViewAppeared();
            PlayVideo();
        }

        public override void ViewDisappeared() {
            base.ViewDisappeared();
            StopVideo();
        }

    }
}
