using System;
using MediaManager;
using MediaManager.Library;

namespace Proact.Mobile.Core.ViewModels {
    public class IosVideoPlayerViewModel : VideoPlayerViewModel {

        private const string _headerAuthorizationKey = "Authorization";

        public IosVideoPlayerViewModel() {
        }

        public async override void PlayEncryptedVideo() {
            AddTokenTorequestHeader();
          
            var mediaitem = await CrossMediaManager.Current.Extractor
                .CreateMediaItem( MediaFileDecryptModel.ContentUrl );
            mediaitem.MediaType = MediaType.Hls;

            await CrossMediaManager.Current.Play( mediaitem );
        }

        private void AddTokenTorequestHeader() {
            CrossMediaManager.Current.RequestHeaders
                .Remove( _headerAuthorizationKey );

            CrossMediaManager.Current
               .RequestHeaders.Add(
               _headerAuthorizationKey,
               $"Bearer {MediaFileDecryptModel.DecryptToken}" );
        }

        public override void StopVideo() {
            var player = CrossMediaManager.Current.MediaPlayer;
            player.Stop();
        }

    }
}
