using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public interface IUserProfileRequestService {
        Task<ResponseResult<UserModel>> GetCurrenUserProfileData();
    }
}
