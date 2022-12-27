using System;
using Xamarin.Forms;
using Proact.Mobile.Core.Models;

namespace Proact.UI {
    public class ReplyMessagesTemplateSelector : DataTemplateSelector {
        public DataTemplate MessageTemplate { get; set; }
        public DataTemplate MessageWithImageTemplate { get; set; }
        public DataTemplate MessageWithVideoTemplate { get; set; }
        public DataTemplate MessageWithVoiceTemplate { get; set; }

        public ReplyMessagesTemplateSelector() {
            MessageTemplate = new DataTemplate( typeof( ReplyMessageContentView ) );
            MessageWithImageTemplate = new DataTemplate( typeof( ReplyMessageWithImageContentView ) );
            MessageWithVideoTemplate = new DataTemplate( typeof( ReplyMessageWithVideoContentView ) );
            MessageWithVoiceTemplate = new DataTemplate( typeof( ReplyMessageWithVoiceContentView ) );
        }

        protected override DataTemplate OnSelectTemplate(
            object item, BindableObject container ) {

            var cellModel = ( MessageModel )item;
        
            if ( cellModel.Attachment != null
                        && cellModel.Attachment.AttachmentType
                        == Mobile.Core.AttachmentType.IMAGE ) {
                return MessageWithImageTemplate;
            }
            else if ( cellModel.Attachment != null
                     && cellModel.Attachment.AttachmentType
                     == Mobile.Core.AttachmentType.VIDEO ) {
                return MessageWithVideoTemplate;
            }
            else if ( cellModel.Attachment != null
                    && cellModel.Attachment.AttachmentType
                    == Mobile.Core.AttachmentType.VOICE ) {
                return MessageWithVoiceTemplate;
            }
            else {
                return MessageTemplate;
            }
        }
    }
}
