using System;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public class ProtocolsService : IProtocolsService {

        private INetworkRequestService _networkRequestService;

        public ProtocolsService(
            INetworkRequestService networkRequestService ) {
            _networkRequestService = networkRequestService;
        }

        public async Task<ResponseResult<PatientProtocolsModel>> GetProtocols() {
            var endPoint = $"Protocols/me";

            return await _networkRequestService
                .GetRequestAsync<PatientProtocolsModel>(
                   ProactServerConfigurations.ApiUrl, endPoint );
        }
    }
}
