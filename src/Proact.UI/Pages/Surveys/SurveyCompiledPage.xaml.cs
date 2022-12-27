using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Proact.Mobile.Core;
using Proact.Mobile.Core.ViewModels;
using Proact.Mobile.UI;
using Xamarin.Forms.Xaml;

namespace Proact.UI.Pages {
    [XamlCompilation( XamlCompilationOptions.Compile )]
    [MvxContentPagePresentation( WrapInNavigationPage = true )]
    public partial class SurveyCompiledPage : MvxContentPage<SurveyCompiledViewModel> {

        private IPopupService _popupService;

        public SurveyCompiledPage() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            SetPopupService();
        }

        protected override bool OnBackButtonPressed() {
            return _popupService.OnBackButtonPressed();
        }

        private void SetPopupService() {
            _popupService = new PopupService();
            ViewModel.SetPopupService( _popupService );
        }
    }
}