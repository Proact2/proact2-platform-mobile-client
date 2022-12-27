using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Proact.Mobile.Core {
    public class RequiredUpdateService : IRequiredUpdateService {

        INetworkRequestService _networkRequestService;
        private RequiredUpdateModel requiredUpdateModel;

        public RequiredUpdateService( INetworkRequestService networkRequestService ) {
            _networkRequestService = networkRequestService;
        }

        public async Task<bool> RequiredUpdateAvailable() {
            var result = await GetLastBuild();
            int currentBuild = 0;
            int.TryParse( VersionTracking.CurrentBuild, out currentBuild );

            if(Device.RuntimePlatform == Device.iOS
                && currentBuild < result.data.IosLastBuildRequired  ) {
                return true;
            }
            else if( Device.RuntimePlatform == Device.Android
                && currentBuild < result.data.AndroidLastBuildRequired ) {
                return true;
            }
            return false;
        }

        public async Task<ResponseResult<RequiredUpdateModel>> GetLastBuild() {
            var result = await _networkRequestService
                .GetRequestAsync<RequiredUpdateModel>(
                    ProactServerConfigurations.ApiUrl, "ApplicationVersion" );

            return result;
        }
    }
}
