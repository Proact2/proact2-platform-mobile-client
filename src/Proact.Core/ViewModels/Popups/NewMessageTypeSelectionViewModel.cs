using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using Proact.Mobile.Core.Models;
using Proact.Mobile.Core.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Proact.Mobile.Core {
    public class NewMessageTypeSelectionViewModel : BaseViewModel {

        public const string MESSAGE_MEDIA_SELECTED = "MESSAGE_MEDIA_SELECTED";

        public IMvxCommand<string> OpenNewTextMessagePageCommand { get; private set; }
        public IMvxCommand OpenNewVoiceMessagePageCommand { get; private set; }
        public IMvxCommand OpenNewVideoMessagePageCommand { get; private set; }

        private IOpenNewMessagePagesBehaviour _openNewMessagePagesBehaviour;

        public NewMessageTypeSelectionViewModel( IOpenNewMessagePagesBehaviour openNewMessagePagesBehaviour ) {

            _openNewMessagePagesBehaviour = openNewMessagePagesBehaviour;

            OpenNewTextMessagePageCommand
                = new MvxCommand<string>( OpenNewTextMessagePageActionHandle );

            OpenNewVoiceMessagePageCommand
                = new MvxCommand( OpenNewVoiceMessagePageActionHandle );

            OpenNewVideoMessagePageCommand
                = new MvxCommand( OpenNewVideoMessagePageActionHandle );
        }

        private async void OpenNewTextMessagePageActionHandle( string messageScope ) {

            MessageScope scope = ( MessageScope )Enum.Parse( typeof( MessageScope ), messageScope );

            await PopupNavigation.Instance.PopAllAsync();

            if ( scope == MessageScope.Info ) {
                _openNewMessagePagesBehaviour.OpenNewInfoTextMessagePage();
            }
            else if ( scope == MessageScope.Health ) {
                _openNewMessagePagesBehaviour.OpenNewHealthStateMessagePage();
            }
        }

        private async void OpenNewVoiceMessagePageActionHandle() {
            await PopupNavigation.Instance.PopAllAsync();
            _openNewMessagePagesBehaviour.OpenNewVoiceMessagePage();
        }

        private async void OpenNewVideoMessagePageActionHandle() {
            await PopupNavigation.Instance.PopAllAsync();
            _openNewMessagePagesBehaviour.OpenNewVideoMessagePage();
        }
    }
}
