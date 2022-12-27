using System;
using Proact.Mobile.Core;
using Proact.Mobile.Droid;
using Xamarin.Forms;

[assembly: Dependency( typeof( AudioSession ) )]
namespace Proact.Mobile.Droid {
    public class AudioSession : IAudioSession {
        public void StartSession() {
            //Nothing
        }

        public void StopSession() {
            //nothing
        }
    }
}
