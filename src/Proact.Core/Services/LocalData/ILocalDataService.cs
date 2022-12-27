using System;
using System.Collections.Generic;
using System.Text;

namespace Proact.Mobile.Core {
    public interface ILocalDataService {
        UserModel GetUserData();
        void SetUserData( UserModel userModel );
    }
}
