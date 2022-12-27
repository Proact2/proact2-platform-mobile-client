using System;
using Proact.Mobile.Core.Models;

namespace Proact.Mobile.Core.ViewModels {
    public class AndroidVideoPlayerViewModel : VideoPlayerViewModel {

        public override void PlayEncryptedVideo() {
           
        }

        public override void StopVideo() {
            MediaFileDecryptModel = null;
        }
    }
}
