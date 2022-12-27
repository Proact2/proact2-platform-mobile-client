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
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proact.UI.Pages {
    [XamlCompilation( XamlCompilationOptions.Compile )]
    [MvxContentPagePresentation( WrapInNavigationPage = true, NoHistory = true )]
    public partial class WallMessagesPage : MvxContentPage<WallMessagesViewModel>, IListScrollingBehaviour {

        private int _lastVisibleItemIndex = 0;
        private int _savedFirstVisibleItem = 0;
        private int _savedLastVisibleItem = 0;
        bool _newMessageButtonIsHidden;
        bool _filterMessagesButtonIsHidden;

        private IPopupService _popupService;
        
        public WallMessagesPage() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            SetPopupService();
            base.OnAppearing();
            InitNavigationBarUI();
            SetScrollBehaviour();
        }

        protected override bool OnBackButtonPressed() {
            return _popupService.OnBackButtonPressed();
        }

        private void SetPopupService() {
            _popupService = new PopupService();
            ViewModel.SetPopupService( _popupService );
        }

        private void InitNavigationBarUI() {
            if ( Application.Current.MainPage is NavigationPage navigationPage ) {
                navigationPage.BarTextColor = Color.White;
                navigationPage.BarBackgroundColor
                    = ( Color )Application.Current.Resources["NavigationBarColor"];
            }
        }

        void CollectionView_Scrolled( System.Object sender, ItemsViewScrolledEventArgs scrollEventArgs ) {
            if( scrollEventArgs.LastVisibleItemIndex != _lastVisibleItemIndex ) {
                ViewModel.OnLastVisibleItemList( scrollEventArgs.LastVisibleItemIndex );
                PerformButtonsAnimationOnScrolled( scrollEventArgs );
            }

            if ( scrollEventArgs.FirstVisibleItemIndex  == 0) {
                StartButtonShowAnimation();
            }
        }

        private void SetScrollBehaviour() {
            ViewModel.SetListScrollingBehaviour( this );
        }

        public void ScrollToIndex( int index ) {
            MessagesCollectionView.ScrollTo( index ); 
        }

        private void PerformButtonsAnimationOnScrolled( ItemsViewScrolledEventArgs scrollEventArgs ) {
            if ( scrollEventArgs.FirstVisibleItemIndex > _savedFirstVisibleItem ) {
                StartButtonHideAnimation();
            }
            else if ( scrollEventArgs.LastVisibleItemIndex < _savedLastVisibleItem ) {
                StartButtonShowAnimation();
            }
            _savedFirstVisibleItem = scrollEventArgs.FirstVisibleItemIndex;
            _savedLastVisibleItem = scrollEventArgs.LastVisibleItemIndex;
        }

        private void StartButtonShowAnimation() {
            StartFilterMessageButtonShowAnimation();
            StartNewMessageButtonShowAnimation();
            buttonsGroup.InputTransparent = false;
        }

        private void StartButtonHideAnimation() {
            StartFilterMessageButtonHideAnimation();
            StartNewMessageButtonHideAnimation();
            buttonsGroup.InputTransparent = true;
        }

        private async void StartNewMessageButtonHideAnimation() {
            if ( !_newMessageButtonIsHidden ) {
                _newMessageButtonIsHidden = true;
                await NewMessageButton.TranslateTo( 500, 0, 500, Easing.CubicOut );              
            }
        }

        private async void StartFilterMessageButtonHideAnimation() {
            if ( !_filterMessagesButtonIsHidden ) {
                _filterMessagesButtonIsHidden = true;
                await MessageFilterButton.TranslateTo( -500, 0, 500, Easing.CubicOut );            
            }
        }

        private async void StartNewMessageButtonShowAnimation() {
            if ( _newMessageButtonIsHidden ) {
                _newMessageButtonIsHidden = false;
                await NewMessageButton.TranslateTo( 0, 0, 500, Easing.CubicIn );               
            }
        }

        private async void StartFilterMessageButtonShowAnimation() {
            if ( _filterMessagesButtonIsHidden ) {
                _filterMessagesButtonIsHidden = false;
                await MessageFilterButton.TranslateTo( 0, 0, 500, Easing.CubicIn );            
            }
        }
    }
}
