using System;
namespace Proact.Mobile.Core {

    public enum PopupMessageType {
        SUCCESS,
        ERROR,
        INFO
    }

    public class PopupMessageModel {
        public PopupMessageType Type { get; set; }
        public string MessageText { get; set; }
    }
}
