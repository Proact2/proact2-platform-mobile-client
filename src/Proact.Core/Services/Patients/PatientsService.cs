using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public class PatientsService : IPatientsService {

        private INetworkRequestService _networkRequestService;
        private ILocalDataReadService _localDataReadService;

        private const string _getPatientsAssociatedWithMedicalTeamEndPoint = "Patient/{0}";
        private const string _getPatientDetailEndPoint = "Patient/{0}/{1}";


        public PatientsService(
           INetworkRequestService networkRequestService,
           ILocalDataReadService localDataReadService ) {
            _networkRequestService = networkRequestService;
            _localDataReadService = localDataReadService;
        }

        public async Task<ResponseResult<List<UserModel>>> GetPatientsAssociatedWithMedicalTeam() {
            string formattedEndpoit = string.Format(
                _getPatientsAssociatedWithMedicalTeamEndPoint,
                GetCurrentMedicalTeamId() );

            var result = await _networkRequestService
               .GetRequestAsync<List<UserModel>>( ProactServerConfigurations.ApiUrl, formattedEndpoit );
            return result;
        }

        public async Task<ResponseResult<UserModel>> GetPatientDetails( string userId ) {
            string formattedEndpoit = string.Format(
              _getPatientDetailEndPoint,
              GetCurrentMedicalTeamId(),
              userId );

            var result = await _networkRequestService
               .GetRequestAsync<UserModel>( ProactServerConfigurations.ApiUrl, formattedEndpoit );
            return result;
        }

        private string GetCurrentMedicalTeamId() {
            return  _localDataReadService
               .GetMedicalTeamModel().MedicalTeamId.ToString();
        }
    }
}
