using System;
using Android.App;
using Android.Content;
using Android.OS;
using Proact.Mobile.Core.Services;
using Proact.Mobile.Core;
using System.IO;
using Proact.Mobile.Core.Models;
using Xamarin.Forms;
using Proact.Mobile.Core.ViewModels;
using MvvmCross;

namespace Proact.Mobile.Droid {
    [Service]
	public class BackgroundUploadMediaService : Service, IBackgroundServicesManager {
		private readonly int _notificationId = 10000;
		private readonly string _notificiationChannelId = "proact_chanel_id";
		private string _notificationTitle;
		private string _notificationMessageUploading;
		private string _notificationMessageVerification;
		private string _notificationMessageUploaded;

		private IMessagesService _messagesService;

		private bool _isStarted;
		private Intent _notificationIntent;

		private FileStream _attacchedFileStream;
		private AttachmentType _attachmentType;
		private Guid _originalMessageId;
		private bool _isNewTopicMessageCreation;

		private void InitNotificationSettings() {
			_notificationIntent = new Intent( this, typeof( MainActivity ) );
			_notificationIntent.SetFlags( ActivityFlags.NewTask | ActivityFlags.SingleTop );
		}

		public override void OnCreate() {
			base.OnCreate();
			InitNotificationSettings();

			_messagesService = Mvx.IoCProvider.Resolve<IMessagesService>();
		}

		public override IBinder OnBind( Intent intent ) {
			return null;
		}

		public override StartCommandResult OnStartCommand( Intent intent, StartCommandFlags flags, int id ) {
			if ( !_isStarted ) {
				_isStarted = true;
				RetrieveExtraParamsFromIntent( intent );
				var notification = UpdateUploadingNotification( 
					_notificationMessageUploading, Android.Resource.Drawable.StatSysUpload );

				StartForeground( _notificationId, notification );
			

				if ( _isNewTopicMessageCreation ) {
					CreateNewMessageWithAttachment();
				}
				else {
					CreateReplyMessageWithAttachment();
				}
			}

			return StartCommandResult.Sticky;
		}

		public override void OnDestroy() {
			_isStarted = false;
						
			RemoveNotification();
			base.OnDestroy();
		}

		private void RetrieveExtraParamsFromIntent( Intent intent ) {

			_notificationTitle = intent?.GetStringExtra(
				BackgroundServicesManager.SERVICE_NOTIFICATION_TITLE_KEY );
			_notificationMessageUploading = intent?.GetStringExtra(
				BackgroundServicesManager.SERVICE_NOTIFICATION_UPLOADING_TEXT_KEY );
			_notificationMessageVerification = intent?.GetStringExtra(
				BackgroundServicesManager.SERVICE_NOTIFICATION_VERIFICATION_TEXT_KEY );
			_notificationMessageUploaded = intent?.GetStringExtra(
				BackgroundServicesManager.SERVICE_NOTIFICATION_UPLOADED_TEXT_KEY );

			string originalMessageId = intent?.GetStringExtra( 
				BackgroundServicesManager.SERVICE_ORIGINAL_MESSAGE_ID_KEY );

			if ( string.IsNullOrEmpty( originalMessageId ) ) {
				_isNewTopicMessageCreation = true;
			}
			else {
				_isNewTopicMessageCreation = false;
				_originalMessageId = new Guid( originalMessageId );
			}

			string attachmentPositionPath = intent?
				.GetStringExtra( BackgroundServicesManager.SERVICE_ATTACHMENT_NAME_KEY );

			_attachmentType = (AttachmentType)intent?
				.GetIntExtra( BackgroundServicesManager.SERVICE_ATTACHMENT_TYPE_KEY, 0 );

			_attacchedFileStream = new FileStream(
				attachmentPositionPath, FileMode.OpenOrCreate, FileAccess.Read );
		}

		private async void CreateNewMessageWithAttachment() {
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
				UpdateUploadingNotification(
					_notificationMessageVerification, Android.Resource.Drawable.StatSysUpload );

				OnMessageReady();
			}
			else {
				string errorMessage = await result.httpResponseMessage.Content
					.ReadAsStringAsync();
				PerformSendingErrorBehaviour( errorMessage );
			}
		}

		private async void CreateReplyMessageWithAttachment() {
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
				UpdateUploadingNotification( 
					_notificationMessageVerification, Android.Resource.Drawable.StatSysUpload );

				OnMessageReady();
			}
			else {
				string errorMessage = await result.httpResponseMessage.Content
				  .ReadAsStringAsync();
				PerformSendingErrorBehaviour( errorMessage );
			}
		}

		private Notification UpdateUploadingNotification( string message, int iconId ) {
			var pendingIntent = PendingIntent.GetActivity( this, 0, _notificationIntent, 0 );

			NotificationManager notificationManager 
				= (NotificationManager)GetSystemService( NotificationService );

			NotificationChannel notChannel = new NotificationChannel( 
				_notificiationChannelId, "Proact 2", NotificationImportance.Low );

			notificationManager.CreateNotificationChannel( notChannel );

			var notification = new Notification
				.Builder( this, _notificiationChannelId )
					.SetContentTitle( _notificationTitle )
					.SetContentText( message )
					.SetSmallIcon( iconId )
					.SetContentIntent( pendingIntent )
					.SetOngoing( true )
					.Build();

			notificationManager.Notify( _notificationId, notification );

			return notification;
		}

		private void RemoveNotification() {
			var notificationManager
				= (NotificationManager)GetSystemService( NotificationService );
			notificationManager.Cancel( _notificationId );
		}

		private void StopCurrentService() {
			UpdateUploadingNotification(
				_notificationMessageUploaded, Android.Resource.Drawable.StatSysUploadDone );

			StopForeground( true );
			StopSelf();
		}

		private void OnMessageReady( ) {
			NotifyMainApplicationMessageUploaded();
			StopCurrentService();
		}

		private void PerformSendingErrorBehaviour() {
			NotifyMainApplicationErrorMessage();
			StopCurrentService();
		}

		private void NotifyMainApplicationMessageUploaded() {
			NotifyMainApplication( true );
		}

		private void PerformSendingErrorBehaviour( string errorMessage ) {
			NotifyMainApplicationErrorMessage( errorMessage );
			StopCurrentService();
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

		public void InitializeStrings(
			string NotificationTitle,
			string MessageUploadingText,
			string MessageVerificationText,
			string MessageUploadedText ) {
            //Do nothing on Android
        }

        public void CreateNewMessageWithAttachment(
			FileStream mediaFileStream, AttachmentType attachmentType ) {
			//Do nothing on Android
		}

		public void CreateReplyMessageWithAttachment(
			Guid originalMessageId,
			FileStream mediaFileStream,
			AttachmentType attachmentType ) {
			//Do nothing on Android
		}

		private void NotifyMainApplicationErrorMessage( string errorMessage ) {
			MessagingCenter.Send<IBackgroundServicesManager, string>(
			 this,
			 WallMessagesViewModel.MSG_ADD_MESSAGE_WITH_ATTACHMENT_FROM_BG_ERROR,
			 errorMessage );
		}
	}
}

