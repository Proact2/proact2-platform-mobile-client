using System;
namespace Proact.Mobile.Core.Models {
    public class MessageActionModel {
        public MessagesContainer Container { get; set; }
        public MessageActionType ActionType { get; set; }
        public MessageScope MessageScope { get; set; }

        public MessageActionModel() {
            Container = new MessagesContainer();
        }
    }
}
