using System.IO;
using System.Threading.Tasks;

namespace Proact.Mobile.Core.Services {
    public interface IAvatarUploaderService {
        Task<ResponseResult> UploadAvatar( Stream avatarFile );
        Task<ResponseResult> ResetAvatar();
    }
}
