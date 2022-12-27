using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Proact.Mobile.Core.Models;
using Proact.Mobile.Core.ViewModels;
using Proact.Mobile.UI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proact.UI.Pages {
    [XamlCompilation( XamlCompilationOptions.Compile )]
    [MvxContentPagePresentation( WrapInNavigationPage = true)]
    public partial class LexiconLabelsListPage : MvxContentPage<LexiconLabelsListViewModel> {
       
        private PopupService _popupService;
        
        public LexiconLabelsListPage() {
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
        private void ListView_OnItemSelected( object sender, SelectedItemChangedEventArgs eventArgs ) {
            ViewModel.ItemSelectedCommand.Execute( (LexiconLabelModel)eventArgs.SelectedItem );
        }
        private void LabelSearchBar_OnTextChanged( object sender, TextChangedEventArgs eventArgs ) {
            ViewModel.PerformSearchCommand.Execute( eventArgs.NewTextValue );
        }
    }
}
