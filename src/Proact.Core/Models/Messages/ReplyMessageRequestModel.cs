using System;

namespace Proact.Mobile.Core.Models {
    public class ReplyMessageRequestModel : MessageRequestModel {
        public Guid OriginalMessageId { get; set; }
    }
}
