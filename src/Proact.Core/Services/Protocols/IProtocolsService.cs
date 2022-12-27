using System;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public interface IProtocolsService {
        Task<ResponseResult<PatientProtocolsModel>> GetProtocols();
    }
}
