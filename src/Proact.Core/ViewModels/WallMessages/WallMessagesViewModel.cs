using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediaManager;
using MvvmCross.Commands;
using Plugin.AudioRecorder;
using Plugin.Media.Abstractions;
using Proact.Mobile.Core.Models;
using Proact.Mobile.Core.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Proact.Mobile.Core.ViewModels {
    public class WallMessagesViewModel : BaseViewModel, IOpenNewMessagePagesBehaviour {

        public const string MSG_VIDEO_MESSAGE_UPLOAD_COMPLETE_FROM_BG = "MSG_VIDEO_MESSAGE_UPLOAD_COMPLETE";
        public const string MSG_AUDIO_MESSAGE_UPLOAD_COMPLETE_FROM_BG = "MSG_AUDIO_MESSAGE_UPLOAD_COMPLETE";

        public const string MSG_ADD_MESSAGE_WITH_ATTACHMENT_FROM_BG_ERROR = "MSG_ADD_MESSAGE_WITH_ATTACHMENT_FROM_BG_ERROR";

        public MessagesContainer SelectedMessage { get; set; }

        public Command ListRefreshCommand { get; private set; }
        public Command SelectionChanged { get; set; }
        public IMvxAsyncCommand<MessageModel> AddNewTextReplyCommand { get; private set; }
        public IMvxAsyncCommand<MessageModel> AddNewVoiceReplyCommand { get; private set; }
        public IMvxAsyncCommand<MessageModel> AddNewVideoReplyCommand { get; private set; }
        public IMvxAsyncCommand<string> OpenVideoAttachmentCommand { get; private set; }
        public IMvxAsyncCommand<string> OpenPatientProfileCommand { get; private set; }
        public IMvxCommand<string> OpenVoiceAttachmentCommand { get; private set; }
        public IMvxCommand<string> OpenImageAttachmentCommand { get; private set; }
        public IMvxCommand OpenProfilePageCommand { get; private set; }
        public IMvxCommand OpenSearchPageCommand { get; private set; }
        public IMvxCommand NewMessageButtonCommand { get; private set; }
        public IMvxCommand ListMessagesToggleCommand { get; private set; }

        private ILocalDataReadService _localDataReadService;
        private IMessagesService _messagesService;
        private ITakeMediaService _takeMediaService;
        private IImageViewerService _imageViewerService;
        private IPushNotificationsService _pushNotificationService;
        private ISurveysService _surveyService;
        private IProjectPropertiesService _projectPropertiesService;
        private IRequiredUpdateService _requiredUpdateService;
        private UserModel _localUserDataModel;

        private bool _showUnrepliedMessageOnly;
        private IListScrollingBehaviour _listScrollingBehaviour;

        private ObservableCollection<MessagesContainer> _messages;
        public ObservableCollection<MessagesContainer> Messages {
            get => _messages;
            set => SetProperty( ref _messages, value );
        }

        private bool _unreadNotifications;
        public bool UnreadNotifications {
            get => _unreadNotifications;
            set => SetProperty( ref _unreadNotifications, value );
        }

        private string _userProfileImageSource;
        public string UserProfileImageSource {
            get => _userProfileImageSource;
            set => SetProperty( ref _userProfileImageSource, value );
        }

        private string _listMessagesToggleButtonText;
        public string ListMessagesToggleButtonText {
            get => _listMessagesToggleButtonText;
            set => SetProperty( ref _listMessagesToggleButtonText, value );
        }

        private bool _isRefreshing;
        public bool IsRefreshing {
            get => _isRefreshing;
            set => SetProperty( ref _isRefreshing, value );
        }

        private bool _isLoadingNextPage;
        public bool IsLoadingNextPage {
            get => _isLoadingNextPage;
            set => SetProperty( ref _isLoadingNextPage, value );
        }

        private bool _messageUploading;
        public bool MessageUploading {
            get => _messageUploading;
            set => SetProperty( ref _messageUploading, value );
        }

        public bool _messageListIsEmpty;
        public bool MessageListIsEmpty {
            get => _messageListIsEmpty;
            set => SetProperty( ref _messageListIsEmpty, value );
        }

        public bool _currentUserIsInMedicalTeam;
        public bool CurrentUserIsInMedicalTeam {
            get => _currentUserIsInMedicalTeam;
            set => SetProperty( ref _currentUserIsInMedicalTeam, value );
        }

        public string NewMessageButtonText { get; private set; }
        public bool NewMessageButtonIsVisible { get; private set; }

        private int _pagingCount = 0;
        private bool _pagingNoMoreItems;
        private ResponseResult<List<MessagesContainer>> _messagesResponse;

        public WallMessagesViewModel(
            ILocalDataReadService localDataReadService,
            IMessagesService messagesService,
            ITakeMediaService takeMediaService,
            IImageViewerService imageViewerService,
            IPushNotificationsService pushNotificationService,
            ISurveysService surveysService,
            IProjectPropertiesService projectPropertiesService,
            IRequiredUpdateService requiredUpdateService ) {
            _localDataReadService = localDataReadService;
            _messagesService = messagesService;
            _takeMediaService = takeMediaService;
            _imageViewerService = imageViewerService;
            _pushNotificationService = pushNotificationService;
            _surveyService = surveysService;
            _projectPropertiesService = projectPropertiesService;
            _requiredUpdateService = requiredUpdateService;
        }

        public override void Prepare() {
            base.Prepare();
            InitUICommands();
            InitNotificationStatus();
            SubscribeMessagesInMessagingCenter();
        }

        public async override Task Initialize() {
            await base.Initialize();

            LoadLocalUserData();
            UpdateUIWithLocalUserData();
            InitToggleMessagesButton();
       
            await RetrieveMessagesFromNetwork();
            UpdateUiWithMessagesFromNetwork();
            PushNotificationServiceRegistration();
        }

        public override void ViewAppearing() {
            base.ViewAppearing();
            DeselectListViewCell();
            UpdateUIProfilePicture();
            UpdateNotificationsStatus();
            CheckRequiredUpdateAvailable();
        }

        private void LoadLocalUserData() {
            _localUserDataModel = _localDataReadService.GetUserData();
        }

        private void UpdateUIWithLocalUserData() {
            if( _localUserDataModel.UserIsInRole( UserRolesModel.Researcher ) ) {
                CurrentUserIsInMedicalTeam = false;
                NewMessageButtonIsVisible = false;
            }
            else if ( _localUserDataModel.UserIsInRole( UserRolesModel.Patient ) ) {
                NewMessageButtonText
                    = Resources.AppResources.NewMessageButton;
                CurrentUserIsInMedicalTeam = false;
                NewMessageButtonIsVisible = true;
            }
            else { 
                NewMessageButtonText
                    = Resources.AppResources.NewBroadcastMessageButton;
                CurrentUserIsInMedicalTeam = true;
                NewMessageButtonIsVisible = true;
            }

            RaisePropertyChanged( () => NewMessageButtonText );
            RaisePropertyChanged( () => NewMessageButtonIsVisible );
        }

        private void InitUICommands() {
            SelectionChanged
                = new Command( SelectionChangedActionHandle );
            AddNewTextReplyCommand
                = new MvxAsyncCommand<MessageModel>( AddNewTextReplyActionHandle );
            AddNewVoiceReplyCommand
                = new MvxAsyncCommand<MessageModel>( AddNewVoiceReplyActionHandle );
            AddNewVideoReplyCommand
                = new MvxAsyncCommand<MessageModel>( AddNewVideoReplyActionHandle );
            OpenProfilePageCommand
                = new MvxCommand( OpenProfilePageActionHandle );
            OpenSearchPageCommand
                = new MvxCommand( OpenSearchPageActionHandle );
            ListRefreshCommand
                = new Command( ListRefreshActionHandle );
            OpenVideoAttachmentCommand
                = new MvxAsyncCommand<string>( ShowAttachedVideoActionHandle );
            OpenVoiceAttachmentCommand
                = new MvxCommand<string>( ShowAttachedAudioActionHandle );
            OpenImageAttachmentCommand
                = new MvxCommand<string>( ShowAttachedImageActionHandle );
            NewMessageButtonCommand
                = new MvxCommand( NewMessageButtonActionHandle );
            ListMessagesToggleCommand
                = new MvxCommand( ListMessagesToggleActionHandle );
            OpenPatientProfileCommand
                = new MvxAsyncCommand<string>( OpenPatientProfile );
        }

        private void UpdateUIProfilePicture() {
            var localUserData = _localDataReadService.GetUserData();
            UserProfileImageSource = localUserData.AvatarUrl;
        }

        private void InitToggleMessagesButton() {
            _showUnrepliedMessageOnly = false;
            UpdateToggleMessagesButton();
        }

        private void UpdateToggleMessagesButton() {
            if ( _showUnrepliedMessageOnly ) {
                ListMessagesToggleButtonText
                    = Resources.AppResources.AllMessages;
            }
            else {
                ListMessagesToggleButtonText
                    = Resources.AppResources.NewMessages;
            }
        }

        private void SubscribeMessagesInMessagingCenter() {
            MessagingCenter.Subscribe<WallMessageDetailsViewModel, MessagesContainer>(
                this, WallMessageDetailsViewModel.MSG_MESSAGE_UPDATED,
                MessageItemUpdatedCallaback );

            MessagingCenter.Subscribe<WallMessageDetailsViewModel, MessagesContainer>(
                this, WallMessageDetailsViewModel.MSG_MESSAGE_DELETED,
                MessageItemDeletedCallaback );

            MessagingCenter.Subscribe<NewTextMessageViewModel, MessagesContainer>(
                this, NewTextMessageViewModel.MSG_ADD_MESSAGE_WITH_ATTACHMENT,
                InsertMessageWithImageCallaback );

            MessagingCenter.Subscribe<NewTextMessageViewModel, MessagesContainer>(
                this, NewTextMessageViewModel.MSG_ADD_REPLY_WITH_ATTACHMENT,
                MessageWithAttachmentUpdatedCallaback );

            MessagingCenter.Subscribe<IBackgroundServicesManager, bool>(
                this, MSG_AUDIO_MESSAGE_UPLOAD_COMPLETE_FROM_BG,
                OnAudioMessageaddedFromBGService );

            MessagingCenter.Subscribe<IBackgroundServicesManager, bool>(
               this, MSG_VIDEO_MESSAGE_UPLOAD_COMPLETE_FROM_BG,
               OnVideoMessageAddedFromBGService );

                  MessagingCenter.Subscribe<IBackgroundServicesManager, string>(
              this, MSG_ADD_MESSAGE_WITH_ATTACHMENT_FROM_BG_ERROR,
              OnAddMessageFromForegroundError );

        }

      
        private void NewMessageButtonActionHandle() {
            if ( _localUserDataModel.UserIsInRole( UserRolesModel.Patient ) ) {
                OpenNewMessageTypePopupSelection();
            }
            else {
                OpenNewBroadcastMessagePage();
            }
        }

        private async void SelectionChangedActionHandle() {
            if ( SelectedMessage == null
                || SelectedMessage.OriginalMessage.MessageType == MessageType.Broadcast ) {

                return;
            }
            else {
                await _navigationService
                 .Navigate<WallMessageDetailsViewModel,
                 MessagesContainer, MessagesContainer>( SelectedMessage );
            }
        }

        public async void OpenNewInfoTextMessagePage() {
            var messageAction = new MessageActionModel();
            messageAction.ActionType = MessageActionType.CREATE;
            messageAction.MessageScope = MessageScope.Info;
            MessagesContainer newMessage = await _navigationService
                .Navigate<NewInfoRequestTextMessageViewModel,
                MessageActionModel, MessagesContainer>( messageAction );

            if ( newMessage != null ) {
                UpdateListViewOnNewMessageAdded( newMessage );
                ShowAddMessageSuccessPopup();
            }
        }

        public async void OpenNewHealthStateMessagePage() {
            var messageAction = new MessageActionModel();
            messageAction.ActionType = MessageActionType.CREATE;
            messageAction.MessageScope = MessageScope.Health;
            MessagesContainer newMessage = await _navigationService
                .Navigate<NewTextMessageViewModel,
                MessageActionModel, MessagesContainer>( messageAction );

            if ( newMessage != null ) {
                UpdateListViewOnNewMessageAdded( newMessage );
                ShowAddMessageSuccessPopup();
            }
        }

        public async void OpenNewVoiceMessagePage() {
            var messageAction = new MessageActionModel();
            messageAction.ActionType = MessageActionType.CREATE;
            MessageUploading = await _navigationService
               .Navigate<VoiceRecordingViewModel, MessageActionModel, bool>( messageAction );
        }

        public async void OpenNewVideoMessagePage() {
            await OpenCameraAndTakevideo( CreateNewVideoMessage );
        }

        private async void OpenNewBroadcastMessagePage() {
            var messageAction = new MessageActionModel();
            messageAction.ActionType = MessageActionType.CREATE;
            MessagesContainer newMessage = await _navigationService
                .Navigate<NewBroadcastMessageViewModel,
                MessageActionModel, MessagesContainer>( messageAction );

            if ( newMessage != null ) {
                UpdateListViewOnNewMessageAdded( newMessage );
                ShowAddMessageSuccessPopup();
            }
        }

        private async Task OpenCameraAndTakevideo( Func<MediaFile, Task<bool>> callback ) {
            var video = await _takeMediaService.TakeVideoAsync();

            if ( video != null ) {
                PopupService.OpenLoadingPopup();
                bool success = await callback( video );
                await PopupService.CloseAllPopup();
                if ( !success ) {
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

        private async Task<bool> CreateNewVideoMessage( MediaFile videoMediaFile ) {
            MessageUploading = true;

            var stream = (FileStream)videoMediaFile.GetStream();
            var bgServiceManager = GetBackgroundServicesManager();
            bgServiceManager.CreateNewMessageWithAttachment( stream, AttachmentType.VIDEO );
            return true;
        }

        private async Task<bool> ReplyWithAVideoMessage( MediaFile videoMediaFile, Guid originalMessageId ) {
            MessageUploading = true;
            var stream = (FileStream)videoMediaFile.GetStream();
            var bgServiceManager = GetBackgroundServicesManager();
            bgServiceManager.CreateReplyMessageWithAttachment( originalMessageId, stream, AttachmentType.VIDEO );
            return true;
        }

        private async Task OpenReplyMessagePage( Guid originalMessageId ) {
            var messageToreply = Messages
                .FirstOrDefault( m => m.OriginalMessage.MessageId == originalMessageId );

            var messageAction = new MessageActionModel();
            messageAction.ActionType = MessageActionType.REPLY;
            messageAction.Container = messageToreply;

            MessagesContainer updatedMessage = await _navigationService
                .Navigate<NewTextMessageViewModel, MessageActionModel, MessagesContainer>( messageAction );

            if ( updatedMessage != null ) {
                UpdateMessageViewOnNewReplyAdded( updatedMessage );
                ShowAddMessageSuccessPopup();
            }
        }

        private async Task AddNewTextReplyActionHandle( MessageModel messageModel ) {
            var isValid = await CurrentUserCanReplyToMessages();
            if ( !isValid ) {
                return;
            }

            if ( messageModel != null && messageModel.MessageId != null ) {
                if ( !_projectPropertiesService.MessageCanBeReplied( messageModel ) ) {
                    ShowMessageCantBeRepliedAlert();
                    return;
                }
                await OpenReplyMessagePage( ( Guid )messageModel.MessageId );
            }
        }

        private async Task AddNewVoiceReplyActionHandle( MessageModel messageModel ) {
            var isValid = await CurrentUserCanReplyToMessages();
            if ( !isValid ) {
                return;
            }

            if ( messageModel != null ) {
                if ( !_projectPropertiesService.MessageCanBeReplied( messageModel ) ) {
                    ShowMessageCantBeRepliedAlert();
                    return;
                }
                
                var messageToreply = Messages
                    .FirstOrDefault( m => m.OriginalMessage.MessageId == messageModel.MessageId );

                var messageAction = new MessageActionModel();
                messageAction.ActionType = MessageActionType.REPLY;
                messageAction.Container = messageToreply;

                MessageUploading = await _navigationService
                    .Navigate<VoiceRecordingViewModel, MessageActionModel, bool>( messageAction );

            }
        }

        private async Task AddNewVideoReplyActionHandle( MessageModel messageModel ) {
            var isValid = await CurrentUserCanReplyToMessages();
            if ( !isValid ) {
                return;
            }

            if ( messageModel != null && messageModel.MessageId != null ) {
                if ( !_projectPropertiesService.MessageCanBeReplied( messageModel ) ) {
                    ShowMessageCantBeRepliedAlert();
                    return;
                }
                await OpenCameraAndTakevideo( videoMediaFile => {
                    return ReplyWithAVideoMessage( videoMediaFile, ( Guid )messageModel.MessageId );
                } );
            }
        }

        private void ShowAttachedImageActionHandle( string originalMessageId ) {
            var attachmentUrl
                = GetOriginalMessageAttachmentUrl( originalMessageId );

            if ( attachmentUrl != null ) {
                _imageViewerService.ShowImage( attachmentUrl );
            }
        }

        private async Task ShowAttachedVideoActionHandle( string originalMessageId ) {
            await PlayNotEncryptedVideo( originalMessageId );
        }

        private async Task<MediaFileDecryptModel> GetMediaFileModel( string originalMessageId ) {
            _popupService.OpenLoadingPopup();
            Guid guid = new Guid( originalMessageId );
            var responseResult = await _messagesService.GetMediaDecryptTokenKey( guid );
            await _popupService.CloseAllPopup();

            return responseResult.data;
        }

        private async Task PlayNotEncryptedVideo( string originalMessageID ) {
            var mediaFileModel = await GetMediaFileModel( originalMessageID );

            if ( mediaFileModel != null ) {
                await _navigationService
                   .Navigate<NotEncryptedVideoPlayerViewModel, MediaFileDecryptModel>( mediaFileModel );
            }        
        }

        private async Task PlayEncryptedVideo( string originalMessageId ) {
            var mediaFileModel = await GetMediaFileModel( originalMessageId );

            if ( mediaFileModel != null ) {
                if ( Device.RuntimePlatform == Device.Android ) {
                    await _navigationService
                        .Navigate<AndroidVideoPlayerViewModel, MediaFileDecryptModel>( mediaFileModel );
                }
                else if ( Device.RuntimePlatform == Device.iOS ) {
                    await _navigationService
                        .Navigate<IosVideoPlayerViewModel, MediaFileDecryptModel>( mediaFileModel );
                }
            }
        }

        private async void ShowAttachedAudioActionHandle( string originalMessageId ) {
            _popupService.OpenLoadingPopup();
            Guid guid = new Guid( originalMessageId );

            var responseResult = await _messagesService.GetMediaDecryptTokenKey( guid );
            await _popupService.CloseAllPopup();

            if ( responseResult.data != null ) {
                PopupService.OpenNotEncryptedAudioPlayerPopup( responseResult.data );
            }
        }

        private string GetOriginalMessageAttachmentUrl( string originalMessageId ) {
            var message = Messages
               .FirstOrDefault( m => m.OriginalMessage
               .MessageId.ToString() == originalMessageId );

            if ( message != null
                && message.OriginalMessage.Attachment != null ) {
                return message.OriginalMessage.Attachment.Url;
            }

            return null;
        }

        private void MessageItemUpdatedCallaback(
            WallMessageDetailsViewModel arg1, MessagesContainer message ) {
            UpdateMessageViewOnNewReplyAdded( message );
        }

        private void MessageItemDeletedCallaback(
            WallMessageDetailsViewModel arg1, MessagesContainer message ) {

            if ( message != null ) {
                var updatedMessage = Messages
                     .FirstOrDefault( x => x.OriginalMessage
                     .MessageId == message.OriginalMessage.MessageId );

                var messageIndex = Messages.IndexOf( updatedMessage );
                if ( messageIndex >= 0 ) {
                    Messages.RemoveAt( messageIndex );
                }
            }
        }

        private void InsertMessageWithImageCallaback(
            NewTextMessageViewModel arg1, MessagesContainer message ) {
            UpdateListViewOnNewMessageAdded( message );
            ShowAddMessageSuccessPopup();
        }

        private void MessageWithAttachmentUpdatedCallaback(
            NewTextMessageViewModel arg1, MessagesContainer message ) {
            UpdateMessageViewOnNewReplyAdded( message );
        }

        private void UpdateListViewOnNewMessageAdded( MessagesContainer newMessage ) {
            if ( newMessage != null ) {
                int index = 0;               
                Messages.Insert( index, newMessage );
                ScrollToIndex( index );
                UpdateEmptyMessageView();
            }
        }

        private void UpdateMessageViewOnNewReplyAdded( MessagesContainer message ) {
            if ( message != null ) {
                var updatedMessage = Messages
                     .FirstOrDefault( x => x.OriginalMessage
                     .MessageId == message.OriginalMessage.MessageId );

                var messageIndex = Messages.IndexOf( updatedMessage );
                if ( messageIndex >= 0 ) {
                    Messages[messageIndex] = message;
                }
            }
        }

        private void OnVideoMessageAddedFromBGService(
            IBackgroundServicesManager sender, bool uploadSuccess ) {
            MessageUploading = false;
            if ( uploadSuccess ) {
                ShowAddViedeoMessageSuccessPopup();
            }
            else {
                ShowAddMessageErrorPopup();
            }
        }

        private void OnAudioMessageaddedFromBGService(
            IBackgroundServicesManager sender, bool uploadSuccess ) {
            MessageUploading = false;
            ListRefreshCommand.Execute(null);
            if ( uploadSuccess ) {
                ShowAddMessageSuccessPopup();
            }
            else {
                ShowAddMessageErrorPopup();
            }
        }

        private void OnAddMessageFromForegroundError(
            IBackgroundServicesManager sender, string errorMessage ) {
            MessageUploading = false;

            ShowAddMessageErrorPopup( errorMessage );
        }

        private async void OpenProfilePageActionHandle() {
            await _navigationService.Navigate<ProfileViewModel>();
        }

        private async void OpenSearchPageActionHandle() {
            await _navigationService.Navigate<SearchMessagesViewModel>();
        }

        private void ListMessagesToggleActionHandle() {
            _showUnrepliedMessageOnly = !_showUnrepliedMessageOnly;
            ScrollToIndex( 0 );
            UpdateToggleMessagesButton();
            ListRefreshActionHandle();
        }

        private async Task RetrieveMessagesFromNetwork() {
            if ( _showUnrepliedMessageOnly ) {
                await RetrieveUnrepliedMessagesFromNetwork();
            }
            else {
                await RetrieveAllMessagesFromNetwork();
            }
        }

        private async Task RetrieveAllMessagesFromNetwork() {
            IsBusy = true;
            _messagesResponse = await _messagesService.GetMessages( _pagingCount );

            if ( !_messagesResponse.Success ) {
                ShowDebugHttpErrorMessage( _messagesResponse.httpResponseMessage );
            }
        }

        private async Task RetrieveUnrepliedMessagesFromNetwork() {
            IsBusy = true;
            _messagesResponse = await _messagesService.GetUnrepliedMessages( _pagingCount );

            if ( !_messagesResponse.Success ) {
                ShowDebugHttpErrorMessage( _messagesResponse.httpResponseMessage );
            }
        }

        private void UpdateUiWithMessagesFromNetwork() {
            if ( _pagingCount == 0 ) {
                Messages = new ObservableCollection<MessagesContainer>();
            }

            if ( _messagesResponse.data != null ) {
                if ( _messagesResponse.data.Count == 0 ) {
                    _pagingNoMoreItems = true;
                }

                foreach ( var message in _messagesResponse.data ) {
                    Messages.Add( message );
                }
            }
            IsBusy = false;
            UpdateEmptyMessageView();
        }

        private void UpdateEmptyMessageView() {
            MessageListIsEmpty = Messages.Count == 0;
        }

        private async void ListRefreshActionHandle() {
            if ( IsBusy ) {
                return;
            }
            _pagingNoMoreItems = false;
            _pagingCount = 0;
            IsRefreshing = true;
            await RetrieveMessagesFromNetwork();
            UpdateUiWithMessagesFromNetwork();
            IsRefreshing = false;
        }

        public async void OnLastVisibleItemList( int lastItemIndex ) {
            if ( IsBusy || _pagingNoMoreItems ) {
                return;
            }
            if ( lastItemIndex == Messages.Count - 1 ) {
                _pagingCount++;
                IsLoadingNextPage = true;
                await RetrieveMessagesFromNetwork();
                UpdateUiWithMessagesFromNetwork();
                IsLoadingNextPage = false;
            }
        }

        private void ShowAddMessageSuccessPopup() {
            PopupService.OpenMessagePopup( new PopupMessageModel() {
                Type = PopupMessageType.SUCCESS,
                MessageText = Resources.AppResources.MessageSentSuccessfullyAlert
            } );
        }

        private void ShowAddViedeoMessageSuccessPopup() {
            PopupService.OpenMessagePopup( new PopupMessageModel() {
                Type = PopupMessageType.SUCCESS,
                MessageText = Resources.AppResources.VideoMessageSentSuccessfullyAlert
            } );
        }

        private void ShowAddMessageErrorPopup() {
            PopupService.OpenMessagePopup( new PopupMessageModel() {
                Type = PopupMessageType.ERROR,
                MessageText = Resources.AppResources.NewMessageErrorTitle
                    + " " + Resources.AppResources.NewMessageErrorMessage
            } );
        }

        private void ShowAddMessageErrorPopup( string errorMessage ) {
            PopupService.OpenMessagePopup( new PopupMessageModel() {
                Type = PopupMessageType.ERROR,
                MessageText = errorMessage
            } );
        }

        private void OpenNewMessageTypePopupSelection() {
            _popupService.OpenNewMessageTypeSelectionPopup( this );
        }

        private void DeselectListViewCell() {
            SelectedMessage = null;
            RaisePropertyChanged( () => SelectedMessage );
        }

        public void SetListScrollingBehaviour( IListScrollingBehaviour listScrollingBehaviour ) {
            _listScrollingBehaviour = listScrollingBehaviour;
        }

        public void ScrollToIndex( int index ) {
            InvokeOnMainThread( () => {
                try {
                    if ( _listScrollingBehaviour != null ) {
                        _listScrollingBehaviour.ScrollToIndex( index );
                    }
                }
                catch ( Exception e ) {
                    Console.WriteLine( e.Message );
                }
            } );
        }

        private void PushNotificationServiceRegistration() {
            _pushNotificationService.RegisterPlayerId();
        }

        public async void OnPushNotificationNewMessageReceived( Guid originalMessageGuid ) {
            ScrollToIndex( 0 );
            ListRefreshCommand.Execute( null );
            _popupService.OpenLoadingPopup();
            var messageContainer = await GetMessageDetailsAsync( originalMessageGuid );
            await _popupService.CloseAllPopup();

            if ( messageContainer != null ) {
                await _navigationService
                 .Navigate<WallMessageDetailsViewModel,
                 MessagesContainer, MessagesContainer>( messageContainer );
            }
        }

        public async void OpenNotCompiledSurvey( Guid assignationSurveyId ) {
            var parameters = new SurveysListParameter() {
                SurveysListType = SurveysListType.TO_BE_COMPLETED,
                SurveyAssignationIdToOpen = assignationSurveyId
            };

            await _navigationService
                 .Navigate<SurveysListViewModel,SurveysListParameter>( parameters );            
        }

        public async void OpenNotCompiledSurveysList(  ) {
            var parameters = new SurveysListParameter() {
                SurveysListType = SurveysListType.TO_BE_COMPLETED
            };

            await _navigationService
                 .Navigate<SurveysListViewModel, SurveysListParameter>( parameters );
        }

        private async Task<MessagesContainer> GetMessageDetailsAsync( Guid originalMessageId ) {
            var result = await _messagesService.GetMessage( originalMessageId );
            if ( result.Success ) {
                return result.data;
            }
            return null;
        }

        private async Task OpenPatientProfile( string messageId ) {
            if ( CurrentUserIsInMedicalTeam ) {
                var messagesContainer = Messages
             .FirstOrDefault( m => m.OriginalMessage.MessageId.ToString() == messageId );
                if ( messagesContainer.OriginalMessage.MessageType == MessageType.Patient ) {
                    await _navigationService
                    .Navigate<PatientDetailsViewModel, string>( messagesContainer.OriginalMessage.AuthorId.ToString() );
                }
            }
        }

        private void InitNotificationStatus() {
            UnreadNotifications = false;
        }

        private async void UpdateNotificationsStatus() {
            var surveysCount = await _surveyService.GetNotCompiledSurveysCount();
            UnreadNotifications = surveysCount > 0;
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

        private async Task<bool> CurrentUserCanReplyToMessages() {
            bool isValid = !_localUserDataModel.UserIsInRole( UserRolesModel.Researcher );

            if ( !isValid ) {
               await Application.Current.MainPage.DisplayAlert(
                   string.Empty,
                   Resources.AppResources.UserCantReplyToMessage,
                   Resources.AppResources.Ok );
            }

            return isValid;
        }

        private async void CheckRequiredUpdateAvailable() {
            var updateAvailable = await _requiredUpdateService.RequiredUpdateAvailable();
            if ( updateAvailable ) {
                _popupService.OpenForceUpdatePopup();
            }
        }
    }
}
