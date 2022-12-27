using System;
namespace Proact.Mobile.Core {

    public enum MessageBoxType {

          SUCCESS
        , ERROR
        , WARNING
        , INFO
        , NONE
    }

    public class MessageBoxModel {

        public MessageBoxModel() {
            IsVisible = false;
            Type = MessageBoxType.NONE;
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsVisible { get; set; }
        public MessageBoxType Type { get; set; }
        public bool Closable { get; set; }
    }

    public class MessageBoxService : IMessageBoxService {

        public MessageBoxModel Message { get; private set; }

        public MessageBoxModel ShowMessageBox( string iTitle,
                                    string iMessage,
                                    MessageBoxType iType,
                                    bool iClosable = false) {

            Message = new MessageBoxModel() {
                Title = iTitle,
                Message = iMessage,
                IsVisible = true,
                Type = iType,
                Closable = iClosable
            };

            return Message;
        }

        public MessageBoxModel HideMessageBox() {
            if ( Message != null ) {
                Message.IsVisible = false;
            }

            return Message;
        }

    }
}
