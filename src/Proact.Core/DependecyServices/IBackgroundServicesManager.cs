using System;
using System.IO;

namespace Proact.Mobile.Core {
    public interface IBackgroundServicesManager {

        void InitializeStrings(
            string NotificationTitle,
            string MessageUploadingText,
            string MessageVerificationText,
            string MessageUploadedText );

        void CreateNewMessageWithAttachment(
            FileStream mediaFileStream,
            AttachmentType attachmentType );

        void CreateReplyMessageWithAttachment(
            Guid originalMessageId,
            FileStream mediaFileStream,
            AttachmentType attachmentType );
    }
}
