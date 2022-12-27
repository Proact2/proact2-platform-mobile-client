using System;
using System.Collections.Generic;
using Proact.Mobile.Core;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class NewMessageTypeSelectionPopup : PopupPage {
        public NewMessageTypeSelectionPopup( IOpenNewMessagePagesBehaviour openNewMessagePagesBehaviour ) {
            InitializeComponent();

            BindingContext = new NewMessageTypeSelectionViewModel( openNewMessagePagesBehaviour );
        }

        void OnHeathScobeButtonTapped( System.Object sender, System.EventArgs e ) {
            ShowTypeButtons();
        }

        void OnBackButtonTapped( System.Object sender, System.EventArgs e ) {
            ShowScopeButtons();
        }

        private void ShowTypeButtons() {
            ScopeButtonFrameContainer.IsVisible = false;
            TypeButtonContainer.IsVisible = true;
        }

        private void ShowScopeButtons() {
            ScopeButtonFrameContainer.IsVisible = true;
            TypeButtonContainer.IsVisible = false;
        }

    }
}
