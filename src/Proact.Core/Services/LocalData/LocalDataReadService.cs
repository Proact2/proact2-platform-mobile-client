using System;
using System.IO;
using Newtonsoft.Json;

namespace Proact.Mobile.Core {
    public class LocalDataReadService : BaseLocalDataService, ILocalDataReadService {

        public AuthDataModel GetAuthData() {
            if ( !File.Exists( GetFileName( _authDataFileName ) ) ) {
                return null;
            }

            string serializedData = File.ReadAllText( GetFileName( _authDataFileName ) );
            if ( string.IsNullOrEmpty( serializedData ) ) {
                return null;
            }

            AuthDataModel authDataModel
                = JsonConvert.DeserializeObject<AuthDataModel>( serializedData );

            return authDataModel;
        }

        public UserModel GetUserData() {
            if ( !File.Exists( GetFileName( _userDataFileName ) ) ) {
                return null;
            }

            string serializedUserData = File.ReadAllText( GetFileName( _userDataFileName ) );
            if ( string.IsNullOrEmpty( serializedUserData ) ) {
                return null;
            }

            UserModel userModel
                = JsonConvert.DeserializeObject<UserModel>( serializedUserData );

            return userModel;
        }

        public MedicalTeamModel GetMedicalTeamModel() {
            if ( !File.Exists( GetFileName( _medicalTeamDataFileName ) ) ) {
                return null;
            }

            string serializedData = File.ReadAllText( GetFileName( _medicalTeamDataFileName ) );
            if ( string.IsNullOrEmpty( serializedData ) ) {
                return null;
            }

            MedicalTeamModel medicalTeamModel
                = JsonConvert.DeserializeObject<MedicalTeamModel>( serializedData );

            return medicalTeamModel;
        }

        public ProjectModel GetProjectModel() {
            if ( !File.Exists( GetFileName( _projectDataFileName ) ) ) {
                return null;
            }

            string serializedData = File.ReadAllText( GetFileName( _projectDataFileName ) );
            if ( string.IsNullOrEmpty( serializedData ) ) {
                return null;
            }

            ProjectModel medicalTeamModel
                = JsonConvert.DeserializeObject<ProjectModel>( serializedData );

            return medicalTeamModel;
        }

        public PushNotificationsSettingsModel GetPushNotificationsSettingsModel() {
            if ( !File.Exists( GetFileName( _pushNotificationsSettingsFileName ) ) ) {
                return new PushNotificationsSettingsModel();
            }

            string serializedData = File.ReadAllText( GetFileName( _pushNotificationsSettingsFileName ) );
            if ( string.IsNullOrEmpty( serializedData ) ) {
                return new PushNotificationsSettingsModel();
            }

            PushNotificationsSettingsModel model
                = JsonConvert.DeserializeObject<PushNotificationsSettingsModel>( serializedData );

            return model;
        }

        public bool IsSigninConditionsAccepted() {
            if ( !File.Exists( GetFileName( _signinConditionsFilename ) ) ) {
                return false;
            }

            string serializedData = File.ReadAllText( GetFileName( _signinConditionsFilename ) );
            if ( string.IsNullOrEmpty( serializedData ) ) {
                return false;
            }

            bool model
                = JsonConvert.DeserializeObject<bool>( serializedData );

            return model;
        }
    }
}
