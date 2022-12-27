using System;
namespace Proact.Mobile.Core {
    public class SigninResult {

        public AuthDataModel authData { get; set; }
        public UserModel userData { get; set; }

        public bool Authorized {
            get {
                if(authData != null
                    && authData.Authorized
                        && userData != null ) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
    }
}
