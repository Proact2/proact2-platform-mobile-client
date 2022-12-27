using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Proact.Mobile.Core;
using Proact.Mobile.Core.ViewModels;
using Proact.Mobile.UI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proact.UI.Pages {
    [XamlCompilation( XamlCompilationOptions.Compile )]
    [MvxContentPagePresentation( WrapInNavigationPage = true)]
    public partial class AnalysisPage : MvxContentPage<AnalysisListViewModel>, IListScrollingBehaviour{
       
        private PopupService _popupService;
        
        public AnalysisPage() {
            InitializeComponent();
        }
        
        protected override void OnAppearing() {
            base.OnAppearing();
            SetPopupService();
            SetScrollBehaviour();
        }

        private void SetPopupService() {
            _popupService = new PopupService();
            ViewModel.SetPopupService( _popupService );
        }
        private void SetScrollBehaviour() {
            ViewModel.SetListScrollingBehaviour( this );
        }

        public void ScrollToIndex( int index ) {
            AnalysisCollectionView.ScrollTo( index ); 
        }
    }
}
