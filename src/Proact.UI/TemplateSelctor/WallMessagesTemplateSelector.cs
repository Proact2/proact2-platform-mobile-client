using System;
using Xamarin.Forms;
using Proact.Mobile.Core.Models;

namespace Proact.UI {
    public class WallMessagesTemplateSelector : DataTemplateSelector {
        public DataTemplate MessageTemplate { get; set; }
        public DataTemplate MessageWithImageTemplate { get; set; }
        public DataTemplate MessageWithVideoTemplate { get; set; }
        public DataTemplate MessageWithVoiceTemplate { get; set; }
        public DataTemplate BroadcastMessageTemplate { get; set; }


        public WallMessagesTemplateSelector() {
            MessageTemplate = new DataTemplate( typeof( WallMessageViewCell ) );
            MessageWithImageTemplate = new DataTemplate( typeof( WallMessageWithImageViewCell ) );
            MessageWithVideoTemplate = new DataTemplate( typeof( WallMessageWithVideoViewCell ) );
            MessageWithVoiceTemplate = new DataTemplate( typeof( WallMessageWithVoiceViewCell ) );
            BroadcastMessageTemplate = new DataTemplate( typeof( BroadcastMessageViewCell ) );
        }
            
        protected override DataTemplate OnSelectTemplate(
            object item, BindableObject container ) {

            var cellModel = ( MessagesContainer )item;
            if ( cellModel.OriginalMessage.MessageType == MessageType.Broadcast ) {
                return BroadcastMessageTemplate;
            }
            else if ( cellModel.OriginalMessage.Attachment != null
                        && cellModel.OriginalMessage.Attachment.AttachmentType
                        == Mobile.Core.AttachmentType.IMAGE ) {
                return MessageWithImageTemplate;
            }
            else if ( cellModel.OriginalMessage.Attachment != null
                     && cellModel.OriginalMessage.Attachment.AttachmentType
                     == Mobile.Core.AttachmentType.VIDEO ) {
                return MessageWithVideoTemplate;
            }
            else if ( cellModel.OriginalMessage.Attachment != null
                    && cellModel.OriginalMessage.Attachment.AttachmentType
                    == Mobile.Core.AttachmentType.VOICE ) {
                return MessageWithVoiceTemplate;
            }
            else {
                return MessageTemplate;
            }

        }
    }
}
