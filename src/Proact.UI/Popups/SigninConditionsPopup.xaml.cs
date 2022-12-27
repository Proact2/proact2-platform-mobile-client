using System;
using System.Collections.Generic;
using Proact.Mobile.Core;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Proact.UI {
    public partial class SigninConditionsPopup : PopupPage {

        private ILandingViewModel _landingViewModel;

        public SigninConditionsPopup( ILandingViewModel landingViewModel ) {
            InitializeComponent();
            _landingViewModel = landingViewModel;

            Link1Text.Text = _landingViewModel
                .GetTermsAndConditionsModel()
                .Title;

            Link2Text.Text = _landingViewModel
                .GetPrivacyPolicyModel()
                .Title;

            Switch1ErrorLabel.Text = Mobile.Core.Resources.AppResources
                .TermsAndConditionsError;

            Switch2ErrorLabel.Text = Mobile.Core.Resources.AppResources
               .PrivacyPolicyError;
        }

        void Link1Tapped( System.Object sender, System.EventArgs e ) {
            _landingViewModel.OpenTermsAndConditionsPage();
        }

        void Link2Tapped( System.Object sender, System.EventArgs e ) {
            _landingViewModel.OpenPrivacyPolicyPage();
        }

        void ContinueButtonClicked( System.Object sender, System.EventArgs e ) {
            if ( !Validate() ) {
                return;
            }

            _landingViewModel.TermsAndConditionAcceptedActionHandle();
        }

        private bool Validate() {
            bool isValid = true;
            Switch1ErrorLabel.IsVisible = false;
            Switch2ErrorLabel.IsVisible = false;

            if ( !switch1.IsToggled ) {
                Switch1ErrorLabel.IsVisible = true;
                isValid = false;
            }

            if ( !switch2.IsToggled ) {
                Switch2ErrorLabel.IsVisible = true;
                isValid = false;
            }

            return isValid;
        }

        void Switch1Toggled( System.Object sender, Xamarin.Forms.ToggledEventArgs e ) {
            if ( Switch1ErrorLabel.IsVisible ) {
                Validate();
            }      
        }

        void Switch2Toggled( System.Object sender, Xamarin.Forms.ToggledEventArgs e ) {
            if ( Switch2ErrorLabel.IsVisible ) {
                Validate();
            }
        }
    }
}
