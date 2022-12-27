using System;

namespace Proact.Mobile.Core.Models {
    public class EditMessageRequestModel : CreateNewMessageRequestModel {
        public Guid MessageId { get; set; }
    }
}
