using Proact.Mobile.Core.Models;
using System.Threading.Tasks;

namespace Proact.Mobile.Core.Services {
    public class SettingsRequestService : ISettingsRequestService {
        
        private INetworkRequestService _networkRequestService;
        private string _settingsApiEndpoint = "Settings/";
        private string _publicKeyForEncryptionEndPoint = "publickeyformsgencryption";

        public SettingsRequestService( INetworkRequestService networkRequestService ) {
            _networkRequestService = networkRequestService;
        }

        public async Task<ResponseResult<PublicRSAKeyModel>> GetPublicRSAKey() {
            string endPoint = _settingsApiEndpoint + _publicKeyForEncryptionEndPoint;

            var result = await _networkRequestService
                .GetRequestAsync<PublicRSAKeyModel>( 
                    ProactServerConfigurations.ApiUrl, endPoint );

            return result;
        }
    }
}
