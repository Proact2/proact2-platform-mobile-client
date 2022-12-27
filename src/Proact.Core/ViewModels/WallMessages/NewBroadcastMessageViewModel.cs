using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using Proact.Mobile.Core.Models;
using Proact.Mobile.Core.Services;
using Proact.Mobile.Core.ViewModels;

namespace Proact.Mobile.Core.ViewModels {
    public class NewBroadcastMessageViewModel : BaseViewModel<MessageActionModel,MessagesContainer> {

        public IMvxAsyncCommand SubmitButtonPressedCommand { get; private set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public bool TitleErrorIsVisible { get; set; }
        public bool BodyErrorIsVisible { get; set; }
        public int MessageMaxLenght { get; private set; } = 1000;

        private IMessageBoxService _messageBoxService;
        private IMessagesService _messagesService;

        public NewBroadcastMessageViewModel(
            IMessageBoxService messageBoxService,
            IMessagesService messagesService) {

            _messageBoxService = messageBoxService;
            _messagesService = messagesService;
        }

        public override void Prepare() {
            base.Prepare();
            InitUI();
            ShowInfoMessage();
            InitUICommands();
        }

        public override void Prepare( MessageActionModel parameter ) {
            //do nothing
        }

        private void InitUI() {
            PageTitle = Resources.AppResources.BradcastMessagePageTitle;
        }

        private void InitUICommands() {
            SubmitButtonPressedCommand
                = new MvxAsyncCommand( CreateMessageActionHandle );
        }

        private void ShowInfoMessage() {
            MessageBoxModel = _messageBoxService.ShowMessageBox(
                Resources.AppResources.BradcastMessagePageTitle,
                Resources.AppResources.BradcastMessageInfo,
                MessageBoxType.INFO );
        }

        private async Task CreateMessageActionHandle() {
            if ( !Validate() ) {
                return;
            }

            var newMessage = await CreateBroadcastMessageAsync();
            if ( newMessage != null ) {
                await ClosePageAndReturnMessage( newMessage );
            }
            else {
                ShowPopupErrorMessage();
            }
        }

        private async Task<MessagesContainer> CreateBroadcastMessageAsync() {
            IsBusy = true;
            _popupService.OpenLoadingPopup();

            var result = await _messagesService
                .CreateBroadcastMessage( Title, Body );

            await _popupService.CloseAllPopup();

            if ( result.Success ) {
                var messageContainer = new MessagesContainer();
                messageContainer.OriginalMessage = result.data;
                return messageContainer;
            }
            return null;
        }

        private bool Validate() {
            bool isValid = true;
            TitleErrorIsVisible = false;
            if ( string.IsNullOrEmpty( Title ) ) {
                TitleErrorIsVisible = true;
                isValid = false;
            }

            BodyErrorIsVisible = false;
            if ( string.IsNullOrEmpty( Body ) ) {
                BodyErrorIsVisible = true;
                isValid = false;
            }

            RaisePropertyChanged( () => TitleErrorIsVisible );
            RaisePropertyChanged( () => BodyErrorIsVisible );

            return isValid;
        }

        private async Task ClosePageAndReturnMessage( MessagesContainer messagesContainer ) {
            await _navigationService
                  .Close<MessagesContainer>( this, messagesContainer );
        }

        private void ShowPopupErrorMessage() {
            var message = new PopupMessageModel();
            message.Type = PopupMessageType.ERROR;
            message.MessageText = Resources.AppResources.NewMessageErrorTitle
                + " " + Resources.AppResources.NewMessageErrorMessage;

            _popupService.OpenMessagePopup( message );
        }

    }
}
