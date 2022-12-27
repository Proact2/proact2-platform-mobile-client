using System;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public interface IRequiredUpdateService {
        Task<bool> RequiredUpdateAvailable();
        Task<ResponseResult<RequiredUpdateModel>> GetLastBuild();
    }
}
