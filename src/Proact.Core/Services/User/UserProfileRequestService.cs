using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public class UserProfileRequestService : IUserProfileRequestService {
        
        private INetworkRequestService _networkRequestService;

        private readonly string _usersApiEntpoint = "Users";

        public UserProfileRequestService( 
            INetworkRequestService networkRequestService ) {
            _networkRequestService = networkRequestService;
        }

        public async Task<ResponseResult<UserModel>> GetCurrenUserProfileData( ) {
            var endPoint = $"{_usersApiEntpoint}/me";

            return await _networkRequestService
                .GetRequestAsync<UserModel>(
                   ProactServerConfigurations.ApiUrl, endPoint );
        }
    }
}
