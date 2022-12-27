using System;
using System.Collections.Generic;
using Proact.UI.Resources;
using Xamarin.Forms;

namespace Proact.UI {
    public static class AppThemeHelper {

        public static void ChangeTheme( OSAppTheme appTheme ) {

            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if ( mergedDictionaries != null ) {
                mergedDictionaries.Clear();

                switch ( appTheme ) {

                    case OSAppTheme.Light:
                    case OSAppTheme.Unspecified:
                        mergedDictionaries.Add( new LightTheme() );
                        break;
                    case OSAppTheme.Dark:
                        mergedDictionaries.Add( new DarkTheme() );
                        break;

                }
            }
        }
    }
}
