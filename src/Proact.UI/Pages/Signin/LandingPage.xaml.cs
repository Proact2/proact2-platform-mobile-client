using System;
using System.Collections.Generic;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Proact.Mobile.Core;
using Proact.Mobile.Core.ViewModels;
using Proact.Mobile.UI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proact.UI {
    [XamlCompilation( XamlCompilationOptions.Compile )]
    [MvxContentPagePresentation( WrapInNavigationPage = true )]
    public partial class LandingPage : MvxContentPage<LandingViewModel> {

        private IPopupService _popupService;

        public LandingPage() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            SetPopupService();
        }

        protected override bool OnBackButtonPressed() {
            if ( ViewModel.OpenAfterLogout ) {
                return true;
            }
            
            return _popupService.OnBackButtonPressed();
        }

        private void SetPopupService() {
            _popupService = new PopupService();
            ViewModel.SetPopupService( _popupService );
        }

    }
}
