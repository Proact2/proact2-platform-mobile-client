using System;
using System.Collections.Generic;
using Proact.Mobile.Core;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class DebugMessagePopup : PopupPage {
        public DebugMessagePopup( System.Net.Http.HttpResponseMessage httpResponseMessage ) {
            InitializeComponent();
            UpdateUI( httpResponseMessage );
        }

        private async void UpdateUI( System.Net.Http.HttpResponseMessage httpResponseMessage ) {
            try {
                ContentLabel.Text = await httpResponseMessage.Content.ReadAsStringAsync();
                StatusCodeLabel.Text = httpResponseMessage.StatusCode.ToString();
            }
            catch ( Exception ) { };
            
        }
    }
}
