using System;
namespace Proact.Mobile.Core.Models {
    public class AttachmentModel {
        public string Url { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public string ThumbnailUrl { get; set; }
        public int DurationInMilliseconds { get; set; }
        public MessageContentStatusEnum? ContentStatus { get; set; }

        /*Proprietà per il binding*/

        public string FormattedDuration {
            get {
                TimeSpan time = TimeSpan.FromMilliseconds( DurationInMilliseconds );
                return time.ToString( @"mm\:ss" );
            }
        }
    }
}
