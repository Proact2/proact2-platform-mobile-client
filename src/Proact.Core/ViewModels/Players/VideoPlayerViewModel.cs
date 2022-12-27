using System;
using MediaManager;
using MediaManager.Library;
using MvvmCross.Commands;
using Proact.Mobile.Core.Models;
using Proact.Mobile.Core.ViewModels;

namespace Proact.Mobile.Core.ViewModels {
    public abstract class VideoPlayerViewModel : BaseViewModel<MediaFileDecryptModel> {

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

        public abstract void PlayEncryptedVideo();
        public abstract void StopVideo();

        public override void ViewAppeared() {
            base.ViewAppeared();
            PlayEncryptedVideo();
        }

        public override void ViewDisappeared() {
            base.ViewDisappeared();
            StopVideo();
        }

    }
}
