using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MvvmCross.Commands;
using Plugin.Media.Abstractions;
using Proact.Mobile.Core.Models;
using Proact.Mobile.Core.Services;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class WallMessageDetailsViewModel : BaseViewModel<MessagesContainer, MessagesContainer> {

        public const string MSG_MESSAGE_UPDATED = "MSG_MESSAGE_UPDATED";
        public const string MSG_MESSAGE_DELETED = "MSG_MESSAGE_DELETED";

        public IMvxAsyncCommand AddNewTextReplyCommand { get; private set; }
        public IMvxAsyncCommand AddNewVoiceReplyCommand { get; private set; }
        public IMvxAsyncCommand AddNewVideoReplyCommand { get; private set; }
        public IMvxCommand<MessageModel> ShowAttachmentCommand { get; private set; }
        public IMvxCommand DeleteCommand { get; private set; }
        public Command OpenAnalysisPageCommand { get; private set; }

        public MessageModel OriginalMessage { get; set; }
        public bool AnalysisIsActive { get; set; }

        private List<MessageModel> _replyMessages;
        public List<MessageModel> ReplyMessages {
            get => _replyMessages.OrderBy( x => x.CreatedDatetime ).ToList();
            set => SetProperty( ref _replyMessages, value );
        }

        private int _replyMessagesCount;
        public int ReplyMessagesCount {
            get => _replyMessagesCount;
            set => SetProperty( ref _replyMessagesCount, value );
        }

        private bool _messageUploading;
        public bool MessageUploading {
            get => _messageUploading;
            set => SetProperty( ref _messageUploading, value );
        }

        private bool _deleteButtonIsVisible;
        public bool DeleteButtonIsVisible {
            get => _deleteButtonIsVisible;
            set => SetProperty( ref _deleteButtonIsVisible, value );
        }

        private bool _messageDeteted;

        private MessagesContainer _messageContainer;
        private IImageViewerService _imageViewerService;
        private ITakeMediaService _takeMediaService;
        private IMessagesService _messagesService;
        private ILocalDataReadService _localDataReadService;
        private IProjectPropertiesService _projectPropertiesService;

        public WallMessageDetailsViewModel(
            IImageViewerService imageViewerService,
            ITakeMediaService takeMediaService,
            IMessagesService messagesService,
            ILocalDataReadService localDataReadService,
            IProjectPropertiesService projectPropertiesService ) {
            _imageViewerService = imageViewerService;
            _takeMediaService = takeMediaService;
            _messagesService = messagesService;
            _localDataReadService = localDataReadService;
            _projectPropertiesService = projectPropertiesService;
        }

        public override void Prepare( MessagesContainer messageContainer ) {
            _messageContainer = messageContainer;
            OriginalMessage = messageContainer.OriginalMessage;
            ReplyMessages = messageContainer.ReplyMessages;
            ReplyMessagesCount = messageContainer.ReplyMessagesCount;
            PageTitle = Resources.AppResources.MessageDetailsPageTitle;
            
            InitUi();
            InitUiCommands();
            SubscribeMessagesInMessagingCenter();
            CheckDeleteButtonVisibility();
        }

        public async override Task Initialize() {
            await base.Initialize();
            await RetriveMessageFromNetwork();
        }

        private void InitUi() {
            CheckIfAnalysisConsoleIsAvailable();
        }
        private void CheckIfAnalysisConsoleIsAvailable() {
            var analystConsoleIsActive 
                = _projectPropertiesService
                .IsAnalystConsoleActive();

            var currentUserIsMedic
                = _localDataReadService
                .GetUserData()
                .UserIsInRole( UserRolesModel.MedicalProfessional );

            var currentUserIsResearcher
                = _localDataReadService
                .GetUserData()
                .UserIsInRole( UserRolesModel.Researcher );

            AnalysisIsActive
                = analystConsoleIsActive
                && (currentUserIsMedic || currentUserIsResearcher );

            RaisePropertyChanged( () => AnalysisIsActive );
        }

        private void InitUiCommands() {
            AddNewTextReplyCommand
                = new MvxAsyncCommand( OpenTextReplyPageActionHandle );
            AddNewVoiceReplyCommand
                = new MvxAsyncCommand( OpenVoiceReplyPageActionHandle );
            AddNewVideoReplyCommand
                = new MvxAsyncCommand( OpenVideoReplyPageActionHandle );
            ShowAttachmentCommand
                = new MvxCommand<MessageModel>( ShowAttachmentActionHandle );
            DeleteCommand
                = new MvxCommand( DeleteMessage );
            OpenAnalysisPageCommand = new Command( OpenAnalysisPage );
        }

        private void SubscribeMessagesInMessagingCenter() {
            MessagingCenter.Subscribe<NewTextMessageViewModel, MessagesContainer>(
                this, NewTextMessageViewModel.MSG_ADD_REPLY_WITH_ATTACHMENT,
                TextReplyWithAttachmentAddedCallaback );

            MessagingCenter.Subscribe<IBackgroundServicesManager, bool>(
                this, WallMessagesViewModel.MSG_VIDEO_MESSAGE_UPLOAD_COMPLETE_FROM_BG,
                AddNewVideoMessageFromForegroundService );

            MessagingCenter.Subscribe<IBackgroundServicesManager, bool>(
                this, WallMessagesViewModel.MSG_AUDIO_MESSAGE_UPLOAD_COMPLETE_FROM_BG,
                AddNewAudioMessageFromForegroundService );

          MessagingCenter.Subscribe<IBackgroundServicesManager, string>(
              this, WallMessagesViewModel.MSG_ADD_MESSAGE_WITH_ATTACHMENT_FROM_BG_ERROR,
              OnAddMessageFromForegroundError );
        }

        private async Task RetriveMessageFromNetwork() {
            IsBusy = true;
            var result = await _messagesService.GetMessage( (Guid)OriginalMessage.MessageId );
            if ( result.Success ) {
                UpdateRepliesList( result.data );
            }
            IsBusy = false;
        }

        private async Task OpenTextReplyPageActionHandle() {
            var isValid = await CurrentUserCanReplyToMessages();
            if ( !isValid ) {
                return;
            }

            if ( !_projectPropertiesService.MessageCanBeReplied( OriginalMessage ) ) {
                ShowMessageCantBeRepliedAlert();
                return;
            }

            var messageAction = new MessageActionModel();
            messageAction.ActionType = MessageActionType.REPLY;
            messageAction.Container = new MessagesContainer {
                ReplyMessages = ReplyMessages,
                OriginalMessage = OriginalMessage
            };

            var messageContainer = await _navigationService
                .Navigate<NewTextMessageViewModel,
                          MessageActionModel,
                          MessagesContainer>( messageAction );

            UpdateRepliesList( messageContainer );
        }

        private void TextReplyWithAttachmentAddedCallaback(
            NewTextMessageViewModel arg1, MessagesContainer messageContainer ) {
            UpdateRepliesList( messageContainer );
        }

        private void AddNewVideoMessageFromForegroundService(
            IBackgroundServicesManager sender, bool uploadSuccess ) {
            MessageUploading = false;
        }

        private async void AddNewAudioMessageFromForegroundService(
            IBackgroundServicesManager arg1, bool arg2 ) {
            MessageUploading = false;
            await RetriveMessageFromNetwork();
        }

        private void OnAddMessageFromForegroundError( IBackgroundServicesManager arg1, string arg2 ) {
            MessageUploading = false;
        }

        private void UpdateRepliesList( MessagesContainer messageContainer ) {
            if ( messageContainer != null ) {

                _messageContainer = messageContainer;
                UpdateCounterOnNewReplyAdded();
                ReplyMessages
                    = new List<MessageModel>( _messageContainer.ReplyMessages );
            }
        }

        private void UpdateCounterOnNewReplyAdded() {
            _messageContainer.ReplyMessagesCount
                = _messageContainer.ReplyMessages.Count();
            ReplyMessagesCount = _messageContainer.ReplyMessagesCount;
        }

        private async Task OpenVideoReplyPageActionHandle() {
            var isValid = await CurrentUserCanReplyToMessages();
            if ( !isValid ) {
                return;
            }

            if ( !_projectPropertiesService.MessageCanBeReplied( OriginalMessage ) ) {
                ShowMessageCantBeRepliedAlert();
                return;
            }
            var video = await _takeMediaService.TakeVideoAsync();
            if ( video != null ) {
                MessageUploading = true;
                bool success = await ReplyWithAVideoMessage( video );
                if ( !success ) {
                    MessageUploading = false;
                    ShowAddMessageErrorPopup();
                }
            }
        }

        private IBackgroundServicesManager GetBackgroundServicesManager() {
            var bgServiceManager = DependencyService.Get<IBackgroundServicesManager>();
            bgServiceManager.InitializeStrings(
                Resources.AppResources.NotificationMediaUploadTitle,
                Resources.AppResources.NotificationMessageMediaUploading,
                Resources.AppResources.NotificationMessageMediaVerification,
                Resources.AppResources.NotificationMessageMediaUploaded
            );

            return bgServiceManager;
        }

        private async Task<bool> ReplyWithAVideoMessage( MediaFile videoMediaFile ) {
            var stream = (FileStream)videoMediaFile.GetStream();
            var originalMessageId = (Guid)_messageContainer.OriginalMessage.MessageId;
            var bgServiceManager = GetBackgroundServicesManager();
            bgServiceManager.CreateReplyMessageWithAttachment( originalMessageId, stream, AttachmentType.VIDEO );
            return true;
        }

        private async Task OpenVoiceReplyPageActionHandle() {
            var isValid = await CurrentUserCanReplyToMessages();
            if ( !isValid ) {
                return;
            }

            if ( !_projectPropertiesService.MessageCanBeReplied( OriginalMessage ) ) {
                ShowMessageCantBeRepliedAlert();
                return;
            }
            var messageAction = new MessageActionModel() {
                ActionType = MessageActionType.REPLY,
                Container = _messageContainer
            };

            MessageUploading = await _navigationService
                .Navigate<VoiceRecordingViewModel, MessageActionModel, bool>( messageAction );
        }

        private void ShowAttachmentActionHandle( MessageModel message ) {
            switch ( message.Attachment.AttachmentType ) {
                case AttachmentType.IMAGE:
                ShowAttachedImage( message.Attachment.Url );
                break;
                case AttachmentType.VOICE:
                ShowAttachedVoice( message.MessageId );
                break;
                case AttachmentType.VIDEO:
                ShowAttachedVideo( message.MessageId );
                break;
            }
        }

        private void ShowAttachedImage( string url ) {
            _imageViewerService.ShowImage( url );
        }

        private async void ShowAttachedVideo( Guid? messageId ) {
            if ( messageId == null ) {
                return;
            }
            await PlayNotEncryptedVideo( messageId.ToString() );
        }

        private async void ShowAttachedVoice( Guid? messageId ) {
            if ( messageId == null ) {
                return;
            }
            var mediaFileModel = await GetMediaFileModel( messageId.ToString() );
            if ( mediaFileModel != null ) {
                _popupService.OpenNotEncryptedAudioPlayerPopup( mediaFileModel );
            }
        }

        private async Task PlayNotEncryptedVideo( string originalMessageID ) {
            var mediaFileModel = await GetMediaFileModel( originalMessageID );

            if ( mediaFileModel != null ) {
                await _navigationService
                   .Navigate<NotEncryptedVideoPlayerViewModel, MediaFileDecryptModel>( mediaFileModel );
            }
        }

        private async Task<MediaFileDecryptModel> GetMediaFileModel( string originalMessageId ) {
            _popupService.OpenLoadingPopup();
            Guid guid = new Guid( originalMessageId );
            var responseResult = await _messagesService.GetMediaDecryptTokenKey( guid );
            await _popupService.CloseAllPopup();

            return responseResult.data;
        }

        public async override Task ClosePage() {
            await _navigationService.Close( this );
            if ( _messageDeteted ) {
                UpdateMessagesListOnMessageDelete();
            }
            else {
                UpdateMessagesListOnMessageUpdate();
            }
        }

        public void UpdateMessagesListOnMessageUpdate() {
            MessagingCenter.Send<WallMessageDetailsViewModel, MessagesContainer>(
                this, MSG_MESSAGE_UPDATED, _messageContainer );
        }

        public void UpdateMessagesListOnMessageDelete() {
            MessagingCenter.Send<WallMessageDetailsViewModel, MessagesContainer>(
                this, MSG_MESSAGE_DELETED, _messageContainer );
        }

        private void ShowAddMessageSuccessPopup() {
            PopupService.OpenMessagePopup( new PopupMessageModel() {
                Type = PopupMessageType.SUCCESS,
                MessageText = Resources.AppResources.MessageSentSuccessfullyAlert
            } );
        }

        private void ShowAddMessageErrorPopup() {
            PopupService.OpenMessagePopup( new PopupMessageModel() {
                Type = PopupMessageType.ERROR,
                MessageText = Resources.AppResources.NewMessageErrorTitle
               + " " + Resources.AppResources.NewMessageErrorMessage
            } );
        }

        private async void DeleteMessage() {
            if ( OriginalMessage.MessageId != null ) {
                if ( !_projectPropertiesService.MessageCanBeDeleted( OriginalMessage ) ) {
                    ShowNotDeletableMessageAlert();
                    return;
                }
            
                bool confirmDelete = await ShowDeleteConfirmAlertDialog();
                if (  confirmDelete ) {
                    _popupService.OpenLoadingPopup();
                    var result = await _messagesService
                        .DeleteMessage( ( Guid )OriginalMessage.MessageId );
                    await _popupService.CloseAllPopup();

                    if ( result.Success ) {
                        _messageDeteted = true;
                        await ClosePage();
                    }
                    else {
                        ShowDeleteErrorPopupMessage();
                    }
                }
            }
        }

        private void CheckDeleteButtonVisibility() {
            DeleteButtonIsVisible = ( OriginalMessage.AuthorId
                == _localDataReadService.GetUserData().UserId );
        }

        private void ShowDeleteErrorPopupMessage() {
            var errorModel = new PopupMessageModel() {
                Type = PopupMessageType.ERROR,
                MessageText = Resources.AppResources.GenericLoadingError
            };
            _popupService.OpenMessagePopup( errorModel );
        }

        private async Task<bool> ShowDeleteConfirmAlertDialog() {
            return await Application.Current.MainPage.DisplayAlert(
                Resources.AppResources.DeleteMessageDialogTitle,
                Resources.AppResources.DeleteMessageDialogMessage,
                Resources.AppResources.Delete,
                Resources.AppResources.Cancel );
        }

        private async void ShowNotDeletableMessageAlert() {
            await Application.Current.MainPage.DisplayAlert(
                Resources.AppResources.NotDeletableMessageAlertTitle,
                Resources.AppResources.NotDeletableMessageAlertMessage,
                Resources.AppResources.Ok );
        }

        private async void ShowMessageCantBeRepliedAlert() {
            var message = string.Format(
                Resources.AppResources.CantReplyToMessageAlertMessage,
                _projectPropertiesService.MinutesAfterWitchMessageCantBeDeleted() );

            await Application.Current.MainPage.DisplayAlert(
                Resources.AppResources.CantReplyToMessageAlertTitle,
                message,
                Resources.AppResources.Ok );
        }
        
        private async void ShowMessageCantBeAnalyzedAlert() {
            var message = string.Format(
                Resources.AppResources.CantAnalyzeMessageAlertMessage,
                _projectPropertiesService.MinutesAfterWitchMessageCanBeAnalyzed() );

            await Application.Current.MainPage.DisplayAlert(
                Resources.AppResources.CantAnalyzeMessageAlertTitle,
                message,
                Resources.AppResources.Ok );
        }
        
        private async void OpenAnalysisPage() {
            if ( _projectPropertiesService.MessageCanBeAnalyzed( _messageContainer.OriginalMessage ) ) {
                var pageParams = new AnalysisPageParams() {
                    ProjectId = _localDataReadService
                        .GetProjectModel().ProjectId,
                    MedicalTeamId = _localDataReadService
                        .GetMedicalTeamModel().MedicalTeamId,
                    MessageModel = _messageContainer.OriginalMessage
                };
                await _navigationService.Navigate<AnalysisListViewModel, AnalysisPageParams>( pageParams );
            }
            else {
                ShowMessageCantBeAnalyzedAlert();
            }
        }

        private async Task<bool> CurrentUserCanReplyToMessages() {
            var  localUserdata = _localDataReadService.GetUserData();
            bool isValid = !localUserdata.UserIsInRole( UserRolesModel.Researcher );

            if ( !isValid ) {
                await Application.Current.MainPage.DisplayAlert(
                    string.Empty,
                    Resources.AppResources.UserCantReplyToMessage,
                    Resources.AppResources.Ok );
            }

            return isValid;
        }

    }
}
