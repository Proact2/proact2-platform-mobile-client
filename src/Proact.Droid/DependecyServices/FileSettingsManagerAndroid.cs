using Android.Content;
using Android.Preferences;
using Plugin.CurrentActivity;
using Proact.Mobile.Core;
using Proact.Mobile.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

[assembly: Dependency( typeof( FileSettingsManager ) )]
namespace Proact.Mobile.Droid {
    public class FileSettingsManager : IFileSettingsManager {
        public FileSettingsManager() {
        }

        public void DeleteFile( string iFileName ) {
            ISharedPreferences prefs 
                = AndroidX.Preference.PreferenceManager
                    .GetDefaultSharedPreferences( CrossCurrentActivity.Current.Activity );
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Clear();

            bool bResult = editor.Commit();

            editor.Apply();
        }

        public string GetLocalFolderPath() {
            return Environment.GetFolderPath( Environment.SpecialFolder.Personal );
        }

        public string LoadFromFile( string iFileName ) {

            ISharedPreferences prefs 
                = AndroidX.Preference.PreferenceManager
                    .GetDefaultSharedPreferences( CrossCurrentActivity.Current.Activity );

            string fileString = prefs.GetString( iFileName, "" );

            return fileString;
        }

        public void SaveOnFile( string iFileName, string iContent ) {

            ISharedPreferences prefs 
                = AndroidX.Preference.PreferenceManager
                    .GetDefaultSharedPreferences( CrossCurrentActivity.Current.Activity );
            
            ISharedPreferencesEditor editor = prefs.Edit();

            editor.PutString( iFileName, iContent );

            bool bResult = editor.Commit();

            editor.Apply();
        }
    }
}