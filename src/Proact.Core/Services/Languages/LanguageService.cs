using System;
using Xamarin.Forms;

namespace Proact.Mobile.Core {
    public class LanguageService : ILanguageService {

        public const string IT_TAG = "it-IT";
        public const string EN_TAG = "en-US";
        public const string FR_TAG = "fr-FR";
        public const string ES_TAG = "es-ES";
        public const string DE_TAG = "de-DE";
        public const string NL_TAG = "nl-NL";

        public const string IT_CODE = "it";
        public const string EN_CODE = "en";
        public const string FR_CODE = "fr";
        public const string ES_CODE = "es";
        public const string DE_CODE = "de";
        public const string NL_CODE = "nl";

        public string GetCurrentLanguageTag() {
            var ci = DependencyService
                .Get<ILocalization>().GetCurrentCultureInfo();

            if ( ci.IetfLanguageTag.Contains( IT_CODE ) ) {
                return IT_TAG;
            }
            else if ( ci.IetfLanguageTag.Contains( EN_CODE ) ) {
                return EN_TAG;
            }
            else if ( ci.IetfLanguageTag.Contains( FR_CODE ) ) {
                return FR_TAG;
            }
            else if ( ci.IetfLanguageTag.Contains( ES_CODE ) ) {
                return ES_TAG;
            }
            else if ( ci.IetfLanguageTag.Contains( DE_CODE ) ) {
                return DE_TAG;
            }
            else if ( ci.IetfLanguageTag.Contains( NL_CODE ) ) {
                return NL_TAG;
            }
            else {
                return EN_TAG;
            }
        }
    }
}
