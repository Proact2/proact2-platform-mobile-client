using System;
using AVFoundation;
using Proact.Mobile.Core;
using Proact.Mobile.iOS;
using Xamarin.Forms;

[assembly: Dependency( typeof( AudioSession ) )]
namespace Proact.Mobile.iOS {
    public class AudioSession : IAudioSession {

        public void StartSession() {
            var audioSession = AVAudioSession.SharedInstance();
            var err = audioSession.SetCategory( AVAudioSessionCategory.PlayAndRecord );
            audioSession.SetActive( true );
        }

        public void StopSession() {
            var audioSession = AVAudioSession.SharedInstance();
            audioSession.SetActive( false );
        }
    }
}
