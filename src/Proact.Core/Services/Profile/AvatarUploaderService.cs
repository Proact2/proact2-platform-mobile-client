using System.IO;
using System.Threading.Tasks;

namespace Proact.Mobile.Core.Services {
    public class AvatarUploaderService : IAvatarUploaderService {
        private INetworkRequestService _networkRequestService;

        private const string _uploadAvatarEndPoint = "UserAvatar/Upload";
        private const string _resetAvatarEndPoint = "UserAvatar/Reset";
        private const string _avatarFileParamName = "avatarFile";

        public AvatarUploaderService( INetworkRequestService networkRequestService ) {
            _networkRequestService = networkRequestService;
        }

        public async Task<ResponseResult> UploadAvatar( Stream avatarFile ) {
            var result = await _networkRequestService
                .PostRequestWithMultipartFormData<string>( 
                    ProactServerConfigurations.ApiUrl, _uploadAvatarEndPoint, _avatarFileParamName, avatarFile );

            return result;
        }

        public async Task<ResponseResult> ResetAvatar() {
            var result = await _networkRequestService
                .PostRequestAsync<string>(
                    ProactServerConfigurations.ApiUrl, _resetAvatarEndPoint, null );

            return result;
        }

    }
}
