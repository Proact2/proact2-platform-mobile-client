using System;
using System.Collections.Generic;
using MvvmCross;
using Proact.Mobile.Core;
using Rg.Plugins.Popup.Pages;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class ForceUpdatePopup : PopupPage {
        public ForceUpdatePopup() {
            InitializeComponent();

        }

        public async void UpdateButtonClicked( System.Object sender, System.EventArgs e ) {

            var requiredUpdateService = Mvx.IoCProvider.Resolve<IRequiredUpdateService>();

            var response = await requiredUpdateService.GetLastBuild();
            if ( response.Success ) {
                OpenAppStore( response.data );
            }
        }

        private async void OpenAppStore( RequiredUpdateModel model ) {
            string url;
            if(Device.RuntimePlatform == Device.iOS ) {
                url = model.IosStoreUrl;
            }
            else {
                url = model.AndroidStoreUrl;
            }

            await Launcher.TryOpenAsync( url );
        }

    }
}
