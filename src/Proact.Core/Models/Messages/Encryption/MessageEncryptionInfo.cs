using System;

namespace Proact.Mobile.Core.Models {
    public class MessageEncryptionInfo {
        public Guid MessageEncryptionId { get; set; }
        public byte[] EncryptedIV { get; set; }

        public byte[] EncryptedKey { get; set; }
    }
}
