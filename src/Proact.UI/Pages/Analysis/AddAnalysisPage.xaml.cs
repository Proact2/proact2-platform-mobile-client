using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Forms.Views;
using Proact.Mobile.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proact.Mobile.UI.Pages.Analysis {
    [XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class AddAnalysisPage : MvxContentPage<AddAnalysisViewModel> {
      
        private PopupService _popupService;
        
        public AddAnalysisPage() {
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

