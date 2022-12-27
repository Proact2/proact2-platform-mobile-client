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
    public class NewTextMessageViewModel : BaseViewModel<MessageActionModel, MessagesContainer> {

        public const string MSG_ADD_MESSAGE_WITH_ATTACHMENT = "MSG_ADD_MESSAGE_WITH_IMAGE";
        public const string MSG_ADD_REPLY_WITH_ATTACHMENT = "MSG_ADD_REPLY_WITH_ATTACHMENT";

        public int MessageMaxLenght { get; private set; } = 500;

        public IMvxCommand<string> MoodButtonPressedCommand { get; private set; }
        public IMvxAsyncCommand SubmitButtonPressedCommand { get; private set; }
        public IMvxAsyncCommand AttachImageCommand { get; private set; }

        public string Body { get; set; }

        public double VeryGoodButtonScale { get; private set; }
        public double GoodButtonScale { get; private set; }
        public double BadButtonScale { get; private set; }
        public double VeryBadButtonScale { get; private set; }
        public double VeryGoodButtonBorderWidth { get; private set; }
        public double GoodButtonBorderWidth { get; private set; }
        public double BadButtonBorderWidth { get; private set; }
        public double VeryBadButtonBorderWidth { get; private set; }


        private bool _bodyErrorIsVisible;
        public bool BodyErrorIsVisible {
            get => _bodyErrorIsVisible;
            set => SetProperty( ref _bodyErrorIsVisible, value );
        }

        private bool _moodButtonsAreVisible;
        public bool MoodButtonsAreVisible {
            get => _moodButtonsAreVisible;
            set => SetProperty( ref _moodButtonsAreVisible, value );
        }

        private bool _healthLabelIsVisible;
        public bool HealthLabelIsVisible {
            get => _healthLabelIsVisible;
            set => SetProperty( ref _healthLabelIsVisible, value );
        }

        private string _messageBodyPleceholder;
        public string MessageBodyPleceholder {
            get => _messageBodyPleceholder;
            set => SetProperty( ref _messageBodyPleceholder, value );
        }

        private string _attachImageSource;
        public string AttachImageSource {
            get => _attachImageSource;
            set => SetProperty( ref _attachImageSource, value );
        }

        private bool _attachedImageIsVisible;
        public bool AttachedImageIsVisible {
            get => _attachedImageIsVisible;
            set => SetProperty( ref _attachedImageIsVisible, value );
        }

        private string _attachedImageBase64;
        private MediaFile _attachImageMediaFile;

        private MessageActionModel _messageActionModel;
        private MessageMood _selectedMood = MessageMood.None;

        private IMessagesService _messagesService;
        private IMessageBoxService _messageBoxService;
        private ITakeMediaService _takeMediaService;

        private double _unselectedButtonScale = 1.0;
        private double _selectedButtonScale = 1.2;
        private double _unselectedButtonBorderWidth = 0;
        private double _selectedButtonBorderWidth = 5;

        public NewTextMessageViewModel(
            IMessagesService messagesService,
            IMessageBoxService messageBoxService,
            ITakeMediaService takeMediaService ) {
            _messagesService = messagesService;
            _messageBoxService = messageBoxService;
            _takeMediaService = takeMediaService;

            InitUICommands();
            UnselectMoodButtons();
        }

        private void InitUICommands() {
            MoodButtonPressedCommand = new MvxCommand<string>( MoodButtonPressedActioonHandle );
            SubmitButtonPressedCommand = new MvxAsyncCommand( SubmitButtonPressedActionHandle );
            AttachImageCommand = new MvxAsyncCommand( AttachImageActionHandle );
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
            MoodButtonsAreVisible = true;
            HealthLabelIsVisible = true;
        }

        private void PrepareUIForReplyMessage() {
            PageTitle = Resources.AppResources.ReplyTextMessagePageTitle;
            MessageBodyPleceholder = Resources.AppResources.ReplyTextMessagePlaceholder;
            MoodButtonsAreVisible = false;
            HealthLabelIsVisible = false;
        }

        private void PrepareUIForEditMessage() {
            PageTitle = Resources.AppResources.EditTextMessagePageTitle;
            MessageBodyPleceholder = Resources.AppResources.NewTextMessagePlaceholder;
            MoodButtonsAreVisible = true;
            HealthLabelIsVisible = false; 
        }

        private void MoodButtonPressedActioonHandle( string mood ) {
            UnselectMoodButtons();
            var patientMood = (MessageMood)Enum.Parse( typeof( MessageMood ), mood );

            if ( patientMood == _selectedMood ) {
                _selectedMood = MessageMood.None;
            }
            else {
                _selectedMood = patientMood;
                switch ( _selectedMood ) {

                    case MessageMood.VeryGood:
                        SelectVeryGoodButton();
                    break;

                    case MessageMood.Good:
                        SelectGoodButton();
                    break;

                    case MessageMood.Bad:
                        SelectBadButton();
                    break;

                    case MessageMood.VeryBad:
                        SelectVeryBadButton();
                    break;
                }
            }
        }

        private void UnselectMoodButtons() {
            UnselectBadButton();
            UnselectVeryBadButton();
            UnselectGoodButton();
            UnselectVeryGoodButton();
        }

        private void SelectVeryGoodButton() {
            //VeryGoodButtonScale = _selectedButtonScale;
            VeryGoodButtonBorderWidth = _selectedButtonBorderWidth;
            RaisePropertyChanged( () => VeryGoodButtonScale );
            RaisePropertyChanged( () => VeryGoodButtonBorderWidth );
        }

        private void SelectGoodButton() {
            //GoodButtonScale = _selectedButtonScale;
            GoodButtonBorderWidth = _selectedButtonBorderWidth;
            RaisePropertyChanged( () => GoodButtonScale );
            RaisePropertyChanged( () => GoodButtonBorderWidth );
        }

        private void SelectBadButton() {
            //BadButtonScale = _selectedButtonScale;
            BadButtonBorderWidth = _selectedButtonBorderWidth;
            RaisePropertyChanged( () => BadButtonScale );
            RaisePropertyChanged( () => BadButtonBorderWidth );
        }

        private void SelectVeryBadButton() {
            //VeryBadButtonScale = _selectedButtonScale;
            VeryBadButtonBorderWidth = _selectedButtonBorderWidth;
            RaisePropertyChanged( () => VeryBadButtonScale );
            RaisePropertyChanged( () => VeryBadButtonBorderWidth );
        }

        private void UnselectVeryGoodButton() {
            VeryGoodButtonScale = _unselectedButtonScale;
            VeryGoodButtonBorderWidth = _unselectedButtonBorderWidth;
            RaisePropertyChanged( () => VeryGoodButtonScale );
            RaisePropertyChanged( () => VeryGoodButtonBorderWidth );
        }

        private void UnselectGoodButton() {
            GoodButtonScale = _unselectedButtonScale;
            GoodButtonBorderWidth = _unselectedButtonBorderWidth;
            RaisePropertyChanged( () => GoodButtonScale );
            RaisePropertyChanged( () => GoodButtonBorderWidth );
        }

        private void UnselectBadButton() {
            BadButtonScale = _unselectedButtonScale;
            BadButtonBorderWidth = _unselectedButtonBorderWidth;
            RaisePropertyChanged( () => BadButtonScale );
            RaisePropertyChanged( () => BadButtonBorderWidth );
        }

        private void UnselectVeryBadButton() {
            VeryBadButtonScale = _unselectedButtonScale;
            VeryBadButtonBorderWidth = _unselectedButtonBorderWidth;
            RaisePropertyChanged( () => VeryBadButtonScale );
            RaisePropertyChanged( () => VeryBadButtonBorderWidth );
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
                if ( !AttachedImageIsVisible ) {
                    await CreateNewMessage();
                }
                else {
                    await CreateNewMessageWithAttachment();
                }
                break;
                case MessageActionType.EDIT:
                break;
                case MessageActionType.REPLY:
                if ( !AttachedImageIsVisible ) {
                    await ReplyToMessage();
                }
                else {
                    await ReplyToMessageWithAttachment();
                }
                break;
            }
            await _popupService.CloseAllPopup();
        }

        private async Task CreateNewMessage() {
            var result = await _messagesService.CreateNewMessage( _selectedMood, Body, false, _messageActionModel.MessageScope );

            if ( result.Success ) {
                var messagecontainer = new MessagesContainer();
                messagecontainer.OriginalMessage = result.data;
                await ClosePageAndReturnMessage( messagecontainer );
            }
            else {
                ShowErrorMessage( result.httpResponseMessage );
            }
        }

        private async Task CreateNewMessageWithAttachment() {
            var result = await _messagesService.CreateNewMessageWithImageAttached(
                _selectedMood, Body, _attachImageMediaFile.GetStream(), _messageActionModel.MessageScope );

            if ( result.Success ) {
                var messagecontainer = new MessagesContainer();
                messagecontainer.OriginalMessage = result.data;
                await ClosePageAndSendMessageWithAttachment( messagecontainer );
            }
            else {
                ShowErrorMessage( result.httpResponseMessage );
            }
        }

        private async Task ReplyToMessage() {
            var messageId = (Guid)_messageActionModel.Container.OriginalMessage.MessageId;
            var result = await _messagesService.ReplyToMessage(
                _selectedMood, Body, false, messageId );

            if ( result.Success ) {
                var messagecontainer = PrepareContainerForReplyMessage( result.data );
                await ClosePageAndReturnMessage( messagecontainer );
            }
            else {
                ShowErrorMessage( result.httpResponseMessage );
            }
        }

        private async Task ReplyToMessageWithAttachment() {
            var messageId = (Guid)_messageActionModel.Container.OriginalMessage.MessageId;
            var stream = _attachImageMediaFile.GetStream();

            var result = await _messagesService.ReplyToMessageWithImageAttached( Body, messageId, stream );

            if ( result.Success ) {
                var messagecontainer = PrepareContainerForReplyMessage( result.data );
                await ClosePageAndSendReplyWithAttachment( messagecontainer );
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

        private async Task ClosePageAndSendMessageWithAttachment( MessagesContainer messagesContainer ) {
            await _navigationService.Close( this );

            MessagingCenter.Send<NewTextMessageViewModel, MessagesContainer>(
                this, MSG_ADD_MESSAGE_WITH_ATTACHMENT, messagesContainer );
        }

        private async Task ClosePageAndSendReplyWithAttachment( MessagesContainer messagesContainer ) {
            await _navigationService.Close( this );

            MessagingCenter.Send<NewTextMessageViewModel, MessagesContainer>(
                this, MSG_ADD_REPLY_WITH_ATTACHMENT, messagesContainer );
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

        private async Task AttachImageActionHandle() {
            string resetString = null;
            if ( AttachedImageIsVisible ) {
                resetString = Resources.AppResources.RemoveAttachedImage;
            }

            var response = await Application.Current.MainPage
                .DisplayActionSheet(
                    Resources.AppResources.AttachAnImageToTextMessage,
                    Resources.AppResources.Cancel,
                    resetString,
                    Resources.AppResources.TakePhoto,
                    Resources.AppResources.PickPhoto );

            if ( response == Resources.AppResources.TakePhoto ) {
                _attachImageMediaFile = await _takeMediaService.TakePhoto();
            }
            else if ( response == Resources.AppResources.PickPhoto ) {
                _attachImageMediaFile = await _takeMediaService.PickPhoto();
            }
            else if ( response == Resources.AppResources.RemoveAttachedImage ) {
                AttachedImageIsVisible = false;
                AttachImageSource = null;
                _attachImageMediaFile = null;
                _attachedImageBase64 = null;
            }

            if ( _attachImageMediaFile != null ) {
                _attachedImageBase64= await _takeMediaService.GetBase64( _attachImageMediaFile );
                AttachImageSource = _attachImageMediaFile.Path;
                AttachedImageIsVisible = true;
            }
        }
    }
}
