using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public class ProjectsRequestService : IProjectsRequestService {

        private INetworkRequestService _networkRequestService;
        private string _projectsApiEndpoint = "Projects"; 

        public ProjectsRequestService( INetworkRequestService networkRequestService ) {
            _networkRequestService = networkRequestService;
        }

        public async Task<ResponseResult<List<ProjectModel>>> GetCurrentUserProjects() {
            var endPoint = $"{_projectsApiEndpoint}";

            return await _networkRequestService
                .GetRequestAsync<List<ProjectModel>>(
                   ProactServerConfigurations.ApiUrl, endPoint );
        }
    }
}
