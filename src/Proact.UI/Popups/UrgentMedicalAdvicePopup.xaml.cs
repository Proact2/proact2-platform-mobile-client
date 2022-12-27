using System;
using System.Collections.Generic;
using Proact.Mobile.Core;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class UrgentMedicalAdvicePopup : PopupPage {

        private ILandingViewModel _landingViewModel;

        public UrgentMedicalAdvicePopup( ILandingViewModel landingViewModel ) {
            InitializeComponent();
            _landingViewModel = landingViewModel;
        }

   
        void ContinueButtonClicked( System.Object sender, System.EventArgs e ) {
            if ( !Validate() ) {
                return;
            }

            _landingViewModel.UrgentMedicalAdviceAcceptedActionHandle();
        }

        private bool Validate() {
            ErrorLabel.IsVisible = false;

            if ( !AdviceAgreeSwitch.IsToggled ) {
                ErrorLabel.IsVisible = true;
                return false;
            }

            return true;
        }

        void SwitchToggled( System.Object sender, Xamarin.Forms.ToggledEventArgs e ) {
            if ( ErrorLabel.IsVisible ) {
                Validate();
            }
        }
    }
}
