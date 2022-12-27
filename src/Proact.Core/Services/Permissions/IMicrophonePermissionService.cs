using System;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public interface IMicrophonePermissionService {

        Task<bool> CheckMicrophonePermissionsGranted();
        Task<bool> RequestMicrophonePermission();
        void ShowSettingsUIOnPermissionDenied();
    }
}
