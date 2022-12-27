using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Proact.Mobile.Core {
    public class LocalDataService : ILocalDataService {

        private string _fileName = "userdata.txt";

        private string GetFileName() {
            return Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData ), _fileName );
        }

        public UserModel GetUserData() {
            if ( !File.Exists( GetFileName() ) ) {
                SetUserData( new UserModel() );
            }

            string serializedUserData = File.ReadAllText( GetFileName() );

            if ( string.IsNullOrEmpty( serializedUserData ) ) {
                SetUserData( new UserModel() );

                serializedUserData = File.ReadAllText( GetFileName() );
            }

            UserModel userModel
                = JsonConvert.DeserializeObject<UserModel>( serializedUserData );

            return userModel;
        }

        public void SetUserData( UserModel userModel ) {
            string serializedUserData = JsonConvert.SerializeObject( userModel );

            File.WriteAllText( GetFileName(), serializedUserData );
        }
    }
}
