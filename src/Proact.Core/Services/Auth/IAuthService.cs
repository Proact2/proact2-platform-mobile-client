using System;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public interface IAuthService {
        void Init();
        Task<AuthDataModel> PerformAuthenticationAsync();
        Task<AuthDataModel> PerformSilentAuthenticationAsync();
        Task PerformTokenRefreshAsync();
        Task RemoveAuthenticationAsync();
        event EventHandler<ErrorModel> OnAuthenticationError;
    }
}
