using System;
using System.IO;

namespace Proact.Mobile.Core {
    public class BaseLocalDataService {

        protected readonly string _userDataFileName = "userdata.txt";
        protected readonly string _authDataFileName = "authdata.txt";
        protected readonly string _projectDataFileName = "project.txt";
        protected readonly string _medicalTeamDataFileName = "medicalTeam.txt";
        protected readonly string _publicRSAKeyFileName = "publicRSAKey.txt";
        protected readonly string _pushNotificationsSettingsFileName = "notificationsSettings.txt";
        protected readonly string _signinConditionsFilename = "isSigninConditionsAccepted.txt";

        protected string GetFileName( string filename ) {
            return Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData ), filename );
        }
    }
}
