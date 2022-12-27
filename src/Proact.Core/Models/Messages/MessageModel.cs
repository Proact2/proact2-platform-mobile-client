using System;
using Xamarin.Forms;

namespace Proact.Mobile.Core.Models {
    public class MessageModel {
        public Guid? MessageId { get; set; }

        public Guid? OriginalMessageId { get; set; }

        public bool IsHandled { get; set; }

        public Guid? AuthorId { get; set; }

        public Guid? MedicalTeamId { get; set; }

        public bool IsRead { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public bool Modified { get; set; }

        public MessageType MessageType { get; set; }

        public MessageMood Emotion { get; set; }

        private string contentModel;

        public string ContentUrl {
            get {
                if ( !string.IsNullOrEmpty( contentModel ) ) {
                    return contentModel.Replace( "http://", "https://" );
                }

                return contentModel;
            }

            set {
                contentModel = value;
            }
        }

        public bool OriginalMessage { get; set; }

        public string Token { get; set; }

        public DateTime? RecordedTime { get; set; }

        public DateTime? UploadedTime { get; set; }

        public string AuthorName { get; set; }

        public string AvatarUrl { get; set; }

        public DateTime CreatedDatetime { get; set; }

        public AttachmentModel Attachment { get; set; }

        public MessageScope MessageScope { get; set; }
        
        public int AnalysisCount { get; set; }
        
        //-- Binding Proprierties

        public bool IsAnalysisCountVisible {
            get => AnalysisCount > 0;
        }

        public bool HasImageAttachment {
            get => ( Attachment != null
                && Attachment.AttachmentType == AttachmentType.IMAGE );
        }

        public bool HasVideoAttachment {
            get => ( Attachment != null
                && Attachment.AttachmentType == AttachmentType.VIDEO );
        }

        public bool HasVoiceAttachment {
            get => ( Attachment != null
                && Attachment.AttachmentType == AttachmentType.VOICE );
        }

        public bool AttachmentIsUploading {
            get => ( Attachment != null
                && Attachment.ContentStatus == MessageContentStatusEnum.Uploading );
        }

        public bool AttachmentIsReady {
            get => ( Attachment != null
                &&   Attachment.ContentStatus == MessageContentStatusEnum.Ready );
        }

        public bool EmotionIsVisible {
            get => Emotion != MessageMood.None;
        }

        public bool IsEmptyMessage {
            get => MessageId == null;
        }

        public string EmotionImageSource {
            get {
                switch ( Emotion ) {
                    case MessageMood.VeryBad:
                        return "ic_moodVeryBad";
                    case MessageMood.Bad:
                        return "ic_moodBad";
                    case MessageMood.Good:
                        return "ic_moodGood";
                    case MessageMood.VeryGood:
                        return "ic_moodVeryGood";
                    default:
                        return "ic_moodVeryBad";
                }
            }
        }

        public bool MessageScopeInfoIsVisible {
            get => MessageScope == MessageScope.Info;
        }

        public bool MessageScopeHealthIsVisible {
            get => MessageScope == MessageScope.Health;
        }

        public string FormattedCreationDatetime {
            get {

                var spanDate = DateTime.Now - CreatedDatetime.ToLocalTime();

                if ( spanDate.Days > 7 ) {
                    return CreatedDatetime.ToLocalTime().ToString( "d" );
                }
                if ( spanDate.Days != 0 ) {
                    return $"{spanDate.Days} {Resources.AppResources.DaysSpan}";
                }
                else if ( spanDate.Hours != 0 ) {
                    return $"{spanDate.Hours} {Resources.AppResources.HoursSpan}";
                }
                else if ( spanDate.Minutes != 0 ) {
                    return $"{spanDate.Minutes} {Resources.AppResources.MinutesSpan}";
                }
                else if ( spanDate.Seconds != 0 ) {
                    return $"{spanDate.Seconds} {Resources.AppResources.SecondsSpan}";
                }
                else {
                    return CreatedDatetime.ToLocalTime().ToString( "d" );
                }
            }
        }

        public string ShortCreationDatetime {
            get => CreatedDatetime.ToLocalTime().ToString( "g" );
        }

        public bool IsOriginalMessage {
            get => !IsBroadcastMessage && (OriginalMessageId == null || OriginalMessageId == Guid.Empty);
        }

        public bool IsBroadcastMessage{
            get => (MessageType == MessageType.Broadcast);
        }

        public bool IsCommentMessage {
            get => !IsBroadcastMessage && !IsOriginalMessage;
        }

        private SolidColorBrush _PatientStrokeColor
            = new SolidColorBrush( Color.FromHex( "#07B4C5" ) );
        private SolidColorBrush _MedicStrokeColor
            = new SolidColorBrush( Color.FromHex( "#CD0F9A" ) );
        private SolidColorBrush _NurseStrokeColor
            = new SolidColorBrush( Color.FromHex( "#66CE00" ) );
        public SolidColorBrush AvatarStrokeColor {
            get {
                switch ( MessageType ) {
                    case MessageType.Patient:
                        return _PatientStrokeColor;
                    case MessageType.Medic:
                    case MessageType.Broadcast:
                        return _MedicStrokeColor;
                    case MessageType.Nurse:
                        return _NurseStrokeColor;
                    default:
                        return SolidColorBrush.White;
                }
            }
        }
    }
}