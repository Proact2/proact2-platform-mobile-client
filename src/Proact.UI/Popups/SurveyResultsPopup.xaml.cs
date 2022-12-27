using System;
using System.Collections.Generic;
using Proact.Mobile.Core;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Proact.UI {

    public enum SurveyResults {
        GOOD,
        BAD
    }

    public partial class SurveyResultsPopup : PopupPage {

        private SurveyResults _result;
        private ISurveyResultsPopupCallback _callback;

        public SurveyResultsPopup( SurveyResults result, ISurveyResultsPopupCallback callback ) {
            InitializeComponent();
            _result = result;
            _callback = callback;
            UpdateUI();
        }

        private void UpdateUI() {
            if ( _result == SurveyResults.GOOD ) {
                SetGoodImage();
                SetGoodLabelText();
            }
            else {
                SetBadImage();
                SetBadLabelText();
            }
        }

        private void SetGoodImage() {
            resultImage.Source = "ic_surveyResultGood";
        }

        private void SetBadImage() {
            resultImage.Source = "ic_surveyResultBad";
        }

        private void SetGoodLabelText() {
            resultLabel.Text = Mobile.Core.Resources
                .AppResources.SurveyResultsGoodMessage;
        }

        private void SetBadLabelText() {
            resultLabel.Text = Mobile.Core.Resources
              .AppResources.SurveyResultsBadMessage;
        }

        void OkButtonClicked( System.Object sender, System.EventArgs e ) {
            _callback.OnPopupClosed();
        }
    }
}
