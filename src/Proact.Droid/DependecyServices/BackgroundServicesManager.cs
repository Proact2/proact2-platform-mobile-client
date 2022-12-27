using System;
using System.IO;
using Android.Content;
using Plugin.CurrentActivity;
using Proact.Mobile.Core;
using Proact.Mobile.Droid;
using Xamarin.Forms;

[assembly: Dependency( typeof( BackgroundServicesManager ) )]
namespace Proact.Mobile.Droid {
    public class BackgroundServicesManager : IBackgroundServicesManager {

        public static string SERVICE_ORIGINAL_MESSAGE_ID_KEY = "original_message_id";
        public static string SERVICE_ATTACHMENT_TYPE_KEY = "media_type";
        public static string SERVICE_ATTACHMENT_NAME_KEY = "media_name";

        public static string SERVICE_NOTIFICATION_TITLE_KEY = "notification_title";
        public static string SERVICE_NOTIFICATION_UPLOADING_TEXT_KEY = "notification_uploading_text";
        public static string SERVICE_NOTIFICATION_VERIFICATION_TEXT_KEY = "notification_verification_text";
        public static string SERVICE_NOTIFICATION_UPLOADED_TEXT_KEY = "notification_uploaded_text";

        private string _notificationTitle = "PROACT 2.0";
        private string _notificationMessageUploading = "Message uploading...";
        private string _notificationMessageVerification = "Message verification...";
        private string _notificationMessageUploaded = "Message Uploaded ok!";

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

        public void CreateNewMessageWithAttachment( 
            FileStream mediaFileStream, AttachmentType attachmentType ) {
            var context  = CrossCurrentActivity.Current.AppContext;

            Intent uploadIntent = new Intent( context, typeof( BackgroundUploadMediaService ) );
            uploadIntent.PutExtra( SERVICE_ATTACHMENT_TYPE_KEY, (int)attachmentType );
            uploadIntent.PutExtra( SERVICE_ATTACHMENT_NAME_KEY, mediaFileStream.Name );
            PutNotificationTextIntoIntent( uploadIntent );
            context.StartForegroundService( uploadIntent );
        }

        public void CreateReplyMessageWithAttachment(
            Guid originalMessageId, FileStream mediaFileStream, AttachmentType attachmentType ) {

            var context = CrossCurrentActivity.Current.AppContext;

            Intent uploadIntent = new Intent( context, typeof( BackgroundUploadMediaService ) );
            uploadIntent.PutExtra( SERVICE_ORIGINAL_MESSAGE_ID_KEY, originalMessageId.ToString() );
            uploadIntent.PutExtra( SERVICE_ATTACHMENT_TYPE_KEY, ( int )attachmentType );
            uploadIntent.PutExtra( SERVICE_ATTACHMENT_NAME_KEY, mediaFileStream.Name );
            PutNotificationTextIntoIntent( uploadIntent );
            context.StartForegroundService( uploadIntent );
        }

        private void PutNotificationTextIntoIntent( Intent uploadIntent ) {
            uploadIntent.PutExtra( SERVICE_NOTIFICATION_TITLE_KEY, _notificationTitle );
            uploadIntent.PutExtra( SERVICE_NOTIFICATION_UPLOADING_TEXT_KEY, _notificationMessageUploading );
            uploadIntent.PutExtra( SERVICE_NOTIFICATION_VERIFICATION_TEXT_KEY, _notificationMessageVerification );
            uploadIntent.PutExtra( SERVICE_NOTIFICATION_UPLOADED_TEXT_KEY, _notificationMessageUploaded );
        }

     
    }
}
