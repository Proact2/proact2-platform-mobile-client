using System;
namespace Proact.Mobile.Core {
    public interface IOpenNewMessagePagesBehaviour {
        void OpenNewInfoTextMessagePage();
        void OpenNewHealthStateMessagePage();
        void OpenNewVoiceMessagePage();
        void OpenNewVideoMessagePage();
    }
}
