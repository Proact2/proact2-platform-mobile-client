using System;
namespace Proact.Mobile.Core {
    public class AuthDataModel {
        public string AccessToken { get; set; }
        public string AccountId { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }

        public bool Authorized {
            get {
                return AccessToken != null;
            }
        }

        public bool IsTokenExpired {
            get {
                if ( ExpiresOn == null  || DateTimeOffset.UtcNow > ExpiresOn ) {
                    return true;
                }
                return false;
            }
        }
    }
}
