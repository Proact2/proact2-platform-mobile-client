using Proact.Mobile.Core.Models;
using System.Threading.Tasks;

namespace Proact.Mobile.Core.Services {
    public interface ISettingsRequestService {
        Task<ResponseResult<PublicRSAKeyModel>> GetPublicRSAKey();
    }
}
