using System;
using System.Collections.Generic;

using MvvmCross.Forms.Views;
using Proact.Mobile.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MvvmCross.Forms.Presenters.Attributes;
using Proact.Mobile.UI;
using Proact.Mobile.Core;

namespace Proact.UI.Pages {

    [XamlCompilation( XamlCompilationOptions.Compile )]
    [MvxContentPagePresentation( WrapInNavigationPage = true )]
    public partial class ContactPage : MvxContentPage<ContactViewModel> {

        private IPopupService _popupService;

        public ContactPage() {
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
    }
}
