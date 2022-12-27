using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public interface IPatientsService {
        Task<ResponseResult<List<UserModel>>> GetPatientsAssociatedWithMedicalTeam();
        Task<ResponseResult<UserModel>> GetPatientDetails( string userId );
    }
}
