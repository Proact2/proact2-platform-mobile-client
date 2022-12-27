using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public class MedicalTeamRequestService : IMedicalTeamRequestService {

        private INetworkRequestService _networkRequestService;
        private string _myMedicalTeamApiEntpoint = "MedicalTeam/{0}/my";

        public MedicalTeamRequestService(INetworkRequestService networkRequestService) {
            _networkRequestService = networkRequestService;
        }

        public async Task<ResponseResult<List<MedicalTeamModel>>> GetCurrentUserMedicalTeam( Guid projectId ) {
            var endpoint = string.Format( _myMedicalTeamApiEntpoint, projectId );

            return await _networkRequestService
                .GetRequestAsync<List<MedicalTeamModel>>(
                   ProactServerConfigurations.ApiUrl, endpoint );
        }
    }
}
