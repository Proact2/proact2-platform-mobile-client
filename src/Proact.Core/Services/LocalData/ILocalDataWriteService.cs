using Proact.Mobile.Core.Models;
using System;
namespace Proact.Mobile.Core {
    public interface ILocalDataWriteService {
        void SetAuthData( AuthDataModel authDataModel );
        void SetUserData( UserModel userModel );
        void SetProjectData( ProjectModel projectModel );
        void SetMedicalTeamData( MedicalTeamModel medicalTeamModel );
        void SetPushNotificationsSettingsModel(
            PushNotificationsSettingsModel pushNotificationsSettingsModel );
        void SetSigninConditionsAccepted( bool accepted );
    }
}
