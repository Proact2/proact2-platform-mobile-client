using System.Collections.Generic;

namespace Proact.Mobile.Core.Models {
    public class MessagesContainer {
        public MessageModel OriginalMessage { get; set; }
        public List<MessageModel> ReplyMessages { get; set; }
        public int ReplyMessagesCount { get; set; }

        //-- Binding propriertis

        private int _displayableReply = 3;

        public List<MessageModel> DisplayableReplyMessages {
            get {
                if ( ShowMoreMessage ) {
                    return ReplyMessages.GetRange( 0, _displayableReply );
                }
                else {
                    return ReplyMessages;
                }
            }
        }

        public bool ShowMoreMessage {
            get => ReplyMessagesCount > _displayableReply;
        }

        public MessagesContainer() {
            OriginalMessage = new MessageModel();
            ReplyMessages = new List<MessageModel>();
        }
    }
}
