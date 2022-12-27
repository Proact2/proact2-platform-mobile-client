using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public interface IMedicalTeamRequestService {
        Task<ResponseResult<List<MedicalTeamModel>>> GetCurrentUserMedicalTeam( Guid projectId );
    }
}
