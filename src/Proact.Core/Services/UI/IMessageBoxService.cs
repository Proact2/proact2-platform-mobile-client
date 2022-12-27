using System;
namespace Proact.Mobile.Core {
    public interface IMessageBoxService {

        MessageBoxModel Message { get; }

        MessageBoxModel ShowMessageBox( string iTitle,
                             string iMessage,
                             MessageBoxType iType,
                             bool iClosable = false );

        MessageBoxModel HideMessageBox();
    }
}
