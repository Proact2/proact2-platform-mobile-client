using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Proact.Mobile.Core;
using UIKit;

namespace Proact.Mobile.iOS {
    public class FileSettingsManagerIos : IFileSettingsManager {

        public void SaveOnFile( string iFileName, string iContent ) {

            NSUserDefaults.StandardUserDefaults.SetString( iContent, iFileName );
            NSUserDefaults.StandardUserDefaults.Synchronize();
        }

        public string LoadFromFile( string iFileName ) {
            string fileString = NSUserDefaults.StandardUserDefaults.StringForKey( iFileName );

            return fileString;
        }

        public void DeleteFile( string iFileName ) {
            NSUserDefaults.StandardUserDefaults.RemoveObject( iFileName );
        }

        public string GetLocalFolderPath() {
            return Environment.GetFolderPath( Environment.SpecialFolder.Personal );
        }
    }
}