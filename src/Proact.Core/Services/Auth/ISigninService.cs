using Proact.Mobile.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public interface ISigninService {
        void Init();
        Task PerformAuthenticationAsync();
        Task<SigninResult> PerformSigninAsync();
        Task<AuthDataModel> PerformAutoSigninAsync();
        Task RetrieveAndSavePatientProjectAndMedicalTeamData();
        Task UpdateLocalProjectAndMedicalTeamData();
        event EventHandler<ErrorModel> OnSigninErrorError;
        Task PerformSignout();
    }
}
