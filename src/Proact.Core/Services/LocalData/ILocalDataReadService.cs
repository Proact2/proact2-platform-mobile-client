using System;
namespace Proact.Mobile.Core {
    public interface ILocalDataReadService {
        AuthDataModel GetAuthData();
        UserModel GetUserData();
        ProjectModel GetProjectModel();
        MedicalTeamModel GetMedicalTeamModel();
        PushNotificationsSettingsModel GetPushNotificationsSettingsModel();
        bool IsSigninConditionsAccepted();

    }
}
