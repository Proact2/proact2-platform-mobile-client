using System.IO;
using Newtonsoft.Json;

namespace Proact.Mobile.Core {
    public class LocalDataWriteService : BaseLocalDataService, ILocalDataWriteService {

        public void SetAuthData( AuthDataModel authDataModel ) {
            string serializedData = JsonConvert.SerializeObject( authDataModel );
            File.WriteAllText( GetFileName( _authDataFileName ), serializedData );
        }

        public void SetUserData( UserModel userModel ) {
            string serializedData = JsonConvert.SerializeObject( userModel );
            File.WriteAllText( GetFileName( _userDataFileName ), serializedData );
        }

        public void SetMedicalTeamData( MedicalTeamModel medicalTeamModel ) {
            string serializedData = JsonConvert.SerializeObject( medicalTeamModel );
            File.WriteAllText( GetFileName( _medicalTeamDataFileName ), serializedData );
        }

        public void SetProjectData( ProjectModel projectModel ) {
            string serializedData = JsonConvert.SerializeObject( projectModel );
            File.WriteAllText( GetFileName( _projectDataFileName ), serializedData );
        }
        
        public void SetPushNotificationsSettingsModel(
            PushNotificationsSettingsModel pushNotificationsSettingsModel ) {

            string serializedData = JsonConvert.SerializeObject( pushNotificationsSettingsModel );
            File.WriteAllText( GetFileName( _pushNotificationsSettingsFileName ), serializedData );
        }

        public void SetSigninConditionsAccepted( bool accepted ) {
            string serializedData = JsonConvert.SerializeObject( accepted );
            File.WriteAllText( GetFileName( _signinConditionsFilename ), serializedData );
        }
    }
}
