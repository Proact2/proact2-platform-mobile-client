using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MvvmCross.Commands;
using Plugin.Media.Abstractions;
using Proact.Mobile.Core.Models;
using Proact.Mobile.Core.Services;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class NewInfoRequestTextMessageViewModel : BaseViewModel<MessageActionModel, MessagesContainer> {

        public IMvxAsyncCommand SubmitButtonPressedCommand { get; private set; }

        public int MessageMaxLenght { get; private set; } = 500;
        public string Body { get; set; }

        private bool _bodyErrorIsVisible;
        public bool BodyErrorIsVisible {
            get => _bodyErrorIsVisible;
            set => SetProperty( ref _bodyErrorIsVisible, value );
        }

        private string _messageBodyPleceholder;
        public string MessageBodyPleceholder {
            get => _messageBodyPleceholder;
            set => SetProperty( ref _messageBodyPleceholder, value );
        }

        private MessageActionModel _messageActionModel;
        private IMessagesService _messagesService;
        private IMessageBoxService _messageBoxService;

        public NewInfoRequestTextMessageViewModel(
            IMessagesService messagesService,
            IMessageBoxService messageBoxService ) {
            _messagesService = messagesService;
            _messageBoxService = messageBoxService;
            InitUICommands();
        }

        private void InitUICommands() {
            SubmitButtonPressedCommand = new MvxAsyncCommand( SubmitButtonPressedActionHandle );
        }

        public override void Prepare( MessageActionModel messageModel ) {
            _messageActionModel = messageModel;

            switch ( messageModel.ActionType ) {
                case MessageActionType.CREATE:
                    PrepareUIForCreateMessage();
                    break;
                case MessageActionType.EDIT:
                    PrepareUIForEditMessage();
                    break;
                case MessageActionType.REPLY:
                    PrepareUIForReplyMessage();
                    break;
            }
        }

        private void PrepareUIForCreateMessage() {
            PageTitle = Resources.AppResources.NewTextMessagePageTitle;
            MessageBodyPleceholder = Resources.AppResources.NewTextMessagePlaceholder;
        }

        private void PrepareUIForReplyMessage() {
            PageTitle = Resources.AppResources.ReplyTextMessagePageTitle;
            MessageBodyPleceholder = Resources.AppResources.ReplyTextMessagePlaceholder;
        }

        private void PrepareUIForEditMessage() {
            PageTitle = Resources.AppResources.EditTextMessagePageTitle;
            MessageBodyPleceholder = Resources.AppResources.NewTextMessagePlaceholder;
        }

        private bool Validate() {
            bool isValid = true;
            BodyErrorIsVisible = false;
            if ( string.IsNullOrEmpty( Body ) ) {
                BodyErrorIsVisible = true;
                isValid = false;
            }
            return isValid;
        }

        private async Task SubmitButtonPressedActionHandle() {
            if ( IsBusy || !Validate() ) {
                return;
            }

            _popupService.OpenLoadingPopup();
            switch ( _messageActionModel.ActionType ) {

                case MessageActionType.CREATE:
                    await CreateNewMessage();
                    break;
                case MessageActionType.EDIT:
                    break;
                case MessageActionType.REPLY:
                    await ReplyToMessage();
                    break;
            }
            await _popupService.CloseAllPopup();
        }

        private async Task CreateNewMessage() {
            var result = await _messagesService.CreateNewMessage(
                MessageMood.None, Body, false, _messageActionModel.MessageScope );

            if ( result.Success ) {
                var messagecontainer = new MessagesContainer();
                messagecontainer.OriginalMessage = result.data;
                await ClosePageAndReturnMessage( messagecontainer );
            }
            else {
                ShowErrorMessage( result.httpResponseMessage );
            }
        }

        private async Task ReplyToMessage() {
            var messageId = ( Guid )_messageActionModel.Container.OriginalMessage.MessageId;
            var result = await _messagesService.ReplyToMessage(
                MessageMood.None, Body, false, messageId );

            if ( result.Success ) {
                var messagecontainer = PrepareContainerForReplyMessage( result.data );
                await ClosePageAndReturnMessage( messagecontainer );
            }
            else {
                ShowErrorMessage( result.httpResponseMessage );
            }
        }

        private MessagesContainer PrepareContainerForReplyMessage( MessageModel replyMessage ) {
            var messagecontainer = _messageActionModel.Container;
            messagecontainer.ReplyMessages.Add( replyMessage );
            messagecontainer.ReplyMessages = messagecontainer.ReplyMessages
                .OrderByDescending( x => x.CreatedDatetime ).ToList();
            messagecontainer.ReplyMessagesCount = messagecontainer.ReplyMessages.Count;
            return messagecontainer;
        }

        private async Task ClosePageAndReturnMessage( MessagesContainer messagesContainer ) {
            await _navigationService
                  .Close<MessagesContainer>( this, messagesContainer );
        }

        private async void ShowErrorMessage( HttpResponseMessage httpResponseMessage ) {
            string errorMessage
                = await httpResponseMessage.Content.ReadAsStringAsync();

            MessageBoxModel = _messageBoxService.ShowMessageBox(
                 Resources.AppResources.NewMessageErrorTitle,
                 errorMessage,
                 MessageBoxType.ERROR,
                 true );
        }
    }
}
