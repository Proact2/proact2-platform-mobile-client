using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Proact.Mobile.Core.Services;

namespace Proact.Mobile.Core {
    public class AuthService : IAuthService {

        public event EventHandler<ErrorModel> OnAuthenticationError;

        private ILocalDataWriteService _localDataWriteService;
        private IPublicClientApplication _authenticationClient;

        public AuthService( ILocalDataWriteService localDataWriteService ) {
            _localDataWriteService = localDataWriteService;
        }

        public void Init() {
            _authenticationClient = PublicClientApplicationBuilder
                .Create( Settings.ClientId )
                .WithIosKeychainSecurityGroup( Settings.IosKeychainSecurityGroups )
                .WithB2CAuthority( Settings.AuthoritySignin )
                .WithRedirectUri( $"msal{Settings.ClientId}://auth" )
                .Build();
        }

        public async Task<AuthDataModel> PerformAuthenticationAsync() {
            var authData = new AuthDataModel();
            try {
                var result = await _authenticationClient
                    .AcquireTokenInteractive( Settings.Scopes )
                    .WithPrompt( Prompt.SelectAccount )
                    .WithParentActivityOrWindow( App.UIParent )
                    .ExecuteAsync();

                authData = CreateAuthData( result );
                _localDataWriteService.SetAuthData( authData );

            }
            catch ( MsalClientException ex ) {
                PerformAuthenticationError( ex.ErrorCode, ex.Message );
            }
            catch ( Exception ex ) {
                Console.WriteLine( ex.Message );
            }
            return authData;
        }

        public async Task<AuthDataModel> PerformSilentAuthenticationAsync() {
            var authData = new AuthDataModel();
            try {
                IEnumerable<IAccount> accounts = await _authenticationClient
                    .GetAccountsAsync();

                AuthenticationResult result = await _authenticationClient
                    .AcquireTokenSilent( Settings.Scopes, accounts.FirstOrDefault() )
                    .ExecuteAsync();

                authData = CreateAuthData( result );
                _localDataWriteService.SetAuthData( authData );
            }
            catch ( MsalClientException ex ) {
                PerformAuthenticationError( ex.ErrorCode, ex.Message );
            }
            catch ( Exception ex ) {
                Console.WriteLine( ex.Message );
            }

            return authData;
        }

        public async Task PerformTokenRefreshAsync() {
            if(_authenticationClient == null ) {
                Init();
            }
            await PerformSilentAuthenticationAsync();
        }

        public async Task RemoveAuthenticationAsync() {
            IEnumerable<IAccount> accounts = await _authenticationClient.GetAccountsAsync();

            while ( accounts.Any() ) {
                await _authenticationClient.RemoveAsync( accounts.First() );
                accounts = await _authenticationClient.GetAccountsAsync();
            }

            _localDataWriteService.SetMedicalTeamData( null );
            _localDataWriteService.SetProjectData( null );
        }

        private AuthDataModel CreateAuthData( AuthenticationResult authResult ) {
            var authData = new AuthDataModel();
            authData.AccessToken = authResult.AccessToken;
            authData.AccountId = authResult.UniqueId;
            authData.ExpiresOn = authResult.ExpiresOn;
            return authData;
        }

        private void PerformAuthenticationError( string code, string message ) {
            if ( OnAuthenticationError != null ) {
                var errorModel = new ErrorModel() {
                    Code = code,
                    Message = message
                };
                OnAuthenticationError( this, errorModel );
            }
        }
    }
}
