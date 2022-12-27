using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Proact.Mobile.Core;
using Proact.Mobile.Core.ViewModels;
using Proact.Mobile.UI;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proact.UI.Pages {

    [XamlCompilation( XamlCompilationOptions.Compile )]
    [MvxModalPresentation]
    public partial class VoiceRecordingPage : MvxContentPage<VoiceRecordingViewModel>, IPulseViewBehaviour {

        private IPopupService _popupService;

        public VoiceRecordingPage() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            SetPopupService();
            SetPulseViewBehaviour();
        }

        protected override bool OnBackButtonPressed() {
            bool closePage = !_popupService.OnBackButtonPressed();

            if ( closePage ) {
                ViewModel.ClosePage();
                return true;
            }
            else {
                return false;
            }
        }

        private void SetPopupService() {
            _popupService = new PopupService();
            ViewModel.SetPopupService( _popupService );
        }

        private void SetPulseViewBehaviour() {
            ViewModel.SetPulseViewBehaviour( this );
        }

        public void StartPulse() {
            if ( !pulseView.IsRun ) {
                pulseView.start();
            }
        }

        public void StopPulse() {
            if ( pulseView.IsRun ) {
                pulseView.Stop();
            }
        }
    }
}
