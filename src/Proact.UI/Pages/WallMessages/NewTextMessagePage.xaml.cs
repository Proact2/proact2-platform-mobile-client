using System;
using System.Collections.Generic;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Proact.Mobile.Core;
using Proact.Mobile.Core.ViewModels;
using Proact.Mobile.UI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Proact.UI.Pages {

    [XamlCompilation( XamlCompilationOptions.Compile )]
    [MvxModalPresentation]
    public partial class NewTextMessagePage : MvxContentPage<NewTextMessageViewModel> {

        private IPopupService _popupService;
        private int _messageMaxLenght;

        public NewTextMessagePage() {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            SetPopupService();
            InitCharacterCounter();
        }

        protected override bool OnBackButtonPressed() {
            return _popupService.OnBackButtonPressed();
        }

        private void SetPopupService() {
            _popupService = new PopupService();
            ViewModel.SetPopupService( _popupService );
        }

        private void InitCharacterCounter() {
            _messageMaxLenght = ViewModel.MessageMaxLenght;
            MessageCharsCounter.Text = _messageMaxLenght.ToString();
        }

        private void UpdateCharacterCounter() {
            MessageCharsCounter.Text=
                ( _messageMaxLenght - MessageEditor.Text.Length )
                .ToString(); 
        }

        void EditorTextChanged( System.Object sender, Xamarin.Forms.TextChangedEventArgs e ) {
            if ( string.IsNullOrEmpty( e.NewTextValue ) ) {
                InitCharacterCounter();
                return;
            }

            if ( e.NewTextValue.Length > _messageMaxLenght ) {
                ( ( Editor )sender ).Text = e.OldTextValue;                
            }
            UpdateCharacterCounter();
        }
    }

}
