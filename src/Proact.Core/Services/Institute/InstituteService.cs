using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proact.Mobile.Core.Models;

namespace Proact.Mobile.Core {
    public class InstituteService : IInstituteService {

        private INetworkRequestService _networkRequestService;

        public InstituteService(
              INetworkRequestService networkRequestService ) {
            _networkRequestService = networkRequestService;
        }

        public async Task<ResponseResult<InstituteModel>> GetMyInstitute() {
            var endPoint = "Institutes/me";

            return await _networkRequestService
                .GetRequestAsync<InstituteModel>(
                   ProactServerConfigurations.ApiUrl, endPoint );
        }
    }
}
