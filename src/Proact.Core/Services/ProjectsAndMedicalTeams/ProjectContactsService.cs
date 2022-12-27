using System;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public class ProjectContactsService : IProjectContactsService {

        private ILocalDataReadService _localDataReadService;
        private INetworkRequestService _networkRequestService;

        public ProjectContactsService(
            INetworkRequestService networkRequestService,
            ILocalDataReadService localDataReadService ) {

            _networkRequestService = networkRequestService;
            _localDataReadService = localDataReadService;
        }

        public async Task<ResponseResult<ProjectContactsModel>> GetContacts() {
            var projectId = _localDataReadService.GetProjectModel().ProjectId;
            var endPoint = $"ProjectContacts/{projectId}";

            return await _networkRequestService
                .GetRequestAsync<ProjectContactsModel>(
                   ProactServerConfigurations.ApiUrl, endPoint );
        }
    }
}
