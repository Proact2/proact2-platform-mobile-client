using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using MvvmCross;
using Proact.Mobile.Core;
using Proact.Mobile.Core.Models;
using Proact.Mobile.Core.Services;
using Proact.Mobile.Core.ViewModels;
using Proact.Mobile.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency( typeof( BackgroundUploadMediaService ) )]
namespace Proact.Mobile.iOS {
    public class BackgroundUploadMediaService: IBackgroundServicesManager {

        private readonly string NEW_MESSAGE_SERVICE_NAME = "create_new_message";
        private readonly string NEW_REPLY_SERVICE_NAME = "create_new_reply";
        private string _notificationTitle = "PROACT 2.0";
        private string _notificationMessageUploading = "Message uploading...";
        private string _notificationMessageVerification = "Message verification...";
        private string _notificationMessageUploaded = "Message Uploaded ok!";
     
        private FileStream _attacchedFileStream;
        private AttachmentType _attachmentType;
        private Guid _originalMessageId;

        private IMessagesService _messagesService;
        private ILocalNotificationManager _localNotificationManager;
        private MessageReadyCheckService _messageReadyCheckService;

        public BackgroundUploadMediaService() {
            _localNotificationManager = new LocalNotificationManager();
            _localNotificationManager.Initialize();
        }

        public void InitializeStrings(
            string NotificationTitle,
            string MessageUploadingText,
            string MessageVerificationText,
            string MessageUploadedText ) {

            _notificationTitle = NotificationTitle;
            _notificationMessageUploading = MessageUploadingText;
            _notificationMessageVerification = MessageVerificationText;
            _notificationMessageUploaded = MessageUploadedText;
        }

        public async void CreateNewMessageWithAttachment(
            FileStream mediaFileStream, AttachmentType attachmentType ) {

            InitCoreServices();
            _attachmentType = attachmentType;
            _attacchedFileStream = mediaFileStream;

            await RunRequestInBackgroundMode(
                NEW_MESSAGE_SERVICE_NAME,
                PerformCreateNewMessageWithAttachment );
        }

        public async void CreateReplyMessageWithAttachment(
           Guid originalMessageId, FileStream mediaFileStream, AttachmentType attachmentType ) {

            InitCoreServices();
            _attachmentType = attachmentType;
            _attacchedFileStream = mediaFileStream;
            _originalMessageId = originalMessageId;

            await RunRequestInBackgroundMode(
               NEW_REPLY_SERVICE_NAME,
               PerformCreateReplyMessageWithAttachment );
        }

        private async Task PerformCreateNewMessageWithAttachment() {
            ShowMessageUploadingNotification();
            ResponseResult<MessageModel> result = null;

            if ( _attachmentType == AttachmentType.VIDEO ) {
                result = await _messagesService
                    .CreateNewMessageWithVideoAttached( _attacchedFileStream );
            }
            else {
                result = await _messagesService
                    .CreateNewMessageWithAudioAttached( _attacchedFileStream );
            }

            if ( result.Success ) {
                NotifyMainApplicationMessageUploaded();
            }
            else {
                string errorMessage = await result.httpResponseMessage.Content
                    .ReadAsStringAsync();
                NotifyMainApplicationErrorMessage( errorMessage );
            }
        }

        private async Task PerformCreateReplyMessageWithAttachment() {
            ShowMessageUploadingNotification();
            ResponseResult<MessageModel> result = null;

            if ( _attachmentType == AttachmentType.VIDEO ) {
                result = await _messagesService.ReplyToMessageWithVideoAttached(
                    _originalMessageId, _attacchedFileStream );
            }
            else {
                result = await _messagesService.ReplyToMessageWithAudioAttached(
                    _originalMessageId, _attacchedFileStream );
            }

            if ( result.Success ) {
                NotifyMainApplicationMessageUploaded();
            }
            else {
                string errorMessage = await result.httpResponseMessage.Content
                    .ReadAsStringAsync();
                NotifyMainApplicationErrorMessage( errorMessage );
            }
        }

        private async Task RunRequestInBackgroundMode( string name, Func<Task> action ) {
            nint taskId = 0;
            taskId = UIApplication.SharedApplication.BeginBackgroundTask( name, () => {
                UIApplication.SharedApplication.EndBackgroundTask( taskId );
            } );
            await Task.Factory.StartNew( async () => {
                await action();
                UIApplication.SharedApplication.EndBackgroundTask( taskId );
            } );
        }

        private void InitCoreServices() {
            _messagesService = Mvx.IoCProvider.Resolve<IMessagesService>();
            _messageReadyCheckService = new MessageReadyCheckService();
        }

        private void NotifyMainApplicationMessageUploaded() {
            NotifyMainApplication( true );
        }

        private void NotifyMainApplicationErrorMessage() {
            NotifyMainApplication( false );
        }

        private void NotifyMainApplication( bool success ) {
            string message = string.Empty;
            if ( _attachmentType == AttachmentType.VIDEO ) {
                message = WallMessagesViewModel.MSG_VIDEO_MESSAGE_UPLOAD_COMPLETE_FROM_BG;
            }
            else if ( _attachmentType == AttachmentType.VOICE ) {
                message = WallMessagesViewModel.MSG_AUDIO_MESSAGE_UPLOAD_COMPLETE_FROM_BG;
            }

            MessagingCenter.Send<IBackgroundServicesManager, bool>(
                this, message, success );
        }

        private void NotifyMainApplicationErrorMessage( string errorMessage ) {
            MessagingCenter.Send<IBackgroundServicesManager, string>(
                this,
                WallMessagesViewModel.MSG_ADD_MESSAGE_WITH_ATTACHMENT_FROM_BG_ERROR,
                errorMessage );
        }

        private void ShowMessageUploadingNotification() {
            _localNotificationManager
                .SendNotification( _notificationTitle, _notificationMessageUploading );
        }

    }


}
