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
    [MvxContentPagePresentation]
    public partial class WallMessageDetailsPage : MvxContentPage<WallMessageDetailsViewModel> {

        private IPopupService _popupService;

        public WallMessageDetailsPage() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            SetPopupService();
        }

        private void SetPopupService() {
            _popupService = new PopupService();
            ViewModel.SetPopupService( _popupService );
        }

        protected override bool OnBackButtonPressed() {
            ViewModel.UpdateMessagesListOnMessageUpdate();
            return _popupService.OnBackButtonPressed();
        }
    }
}