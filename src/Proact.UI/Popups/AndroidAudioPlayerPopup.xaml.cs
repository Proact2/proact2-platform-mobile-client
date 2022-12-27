using System;
using System.Collections.Generic;
using MediaManager;
using Proact.Mobile.Core;
using Proact.Mobile.Core.Models;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class AndroidAudioPlayerPopup : PopupPage {

        public AndroidAudioPlayerPopup( MediaFileDecryptModel mediaFileDecrypt ) {
            InitializeComponent();
            player.MediaFileDecrypt = mediaFileDecrypt;
        }
    }
}
