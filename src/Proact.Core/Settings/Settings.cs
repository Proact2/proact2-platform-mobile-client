namespace Proact.Mobile.Core {
    public static class Settings {
        /*
         * WebClient ID
         *
         * Chiave necessaria per la richiesta del tokenId surante il Signin con Google.
         * 
         * Dopo aver creato il progetto su google cloud platform vai su
         * https://console.cloud.google.com/apis/credentials
         *
         * Copia l'Id Client di Web client (auto created by Google Service).
         */

        public const string ANDROID_WEB_CLIENT_ID = "838717493689-59hepuftep9e59mk8pc79dn7jhlrvb7v.apps.googleusercontent.com";

        /*
         * Azure AD B2C
         */

        private static readonly string _tenantName = ""; 
        private static readonly string _tenantId = ""; 
        private static readonly string _clientId = ""; 
        private static readonly string[] _scopes = { "" };
        private static readonly string _policySignin = "";

        private static readonly string _iosKeychainSecurityGroup = "com.upsmart.proactbeta";
        private static readonly string _authorityBase = "";

        public static string ClientId {
            get {
                return _clientId;
            }
        }
        public static string AuthoritySignin {
            get {
                return $"{_authorityBase}{_policySignin}";
            }
        }

        public static string[] Scopes {
            get {
                return _scopes;
            }
        }
        public static string IosKeychainSecurityGroups {
            get {
                return _iosKeychainSecurityGroup;
            }
        }

        /* LICENSE URL */

        public static readonly string TERMS_AND_CONDITIONS_URL = "https://prodproactmediastorage.blob.core.windows.net/terms-and-conditions/PROACT2.0.TermsAndConditions.Ita.pdf";
        public static readonly string LICENSE_URL = "https://www.clickdimensions.com/links/TestPDFfile.pdf";
        public static readonly string PRIVACY_POLICY_URL = "https://prodproactmediastorage.blob.core.windows.net/terms-and-conditions/PROACT2.0.Privacy.Policy.Ita.pdf.pdf";

    }
}
