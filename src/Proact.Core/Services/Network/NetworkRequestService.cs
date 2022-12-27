using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Proact.Mobile.Core {
    public class NetworkRequestService : INetworkRequestService {

        private ILocalDataReadService _localDataReadService;
        private IAuthService _authService;
        private ILanguageService _languageService;

        public NetworkRequestService(
            ILocalDataReadService localDataReadService,
            IAuthService authService,
            ILanguageService languageService ) {
            _localDataReadService = localDataReadService;
            _authService = authService;
            _languageService = languageService;
        }

        private string AddLanguageTagToBaseUrl( string baseUrl ) {
            var languageTag = _languageService.GetCurrentLanguageTag();
            return $"{baseUrl + languageTag}/";
        }

        private void PrintSendingRequestDataInConsole( 
            string baseUrl, string endPoint, string stringContent ) {
            Console.WriteLine( "\n>---------------------\n" );
            Console.WriteLine( "\n>Sending Request to: " + baseUrl + endPoint + "\n" );
            Console.WriteLine( "\n>Content: " + stringContent + "\n" );
            Console.WriteLine( "\n---------------------\n" );
        }

        private void PrintSendingGetDataInConsole( string baseUrl, string endPoint ) {
            Console.WriteLine( "> Sending Get Request: " + baseUrl + endPoint );

            var code = _languageService.GetCurrentLanguageTag();
        }

        private void PrintDeleteRequestInConsole( string baseUrl, string endPoint ) {
            Console.WriteLine( "> Sending Delete Request: " + baseUrl + endPoint );
        }

        public async Task<ResponseResult<T>> GetRequestAsync<T>( string baseUrl, string endPoint ) {
            try {
                await RefreshTokenIfExpired();
                baseUrl = AddLanguageTagToBaseUrl( baseUrl );

                PrintSendingGetDataInConsole( baseUrl, endPoint );

                string accessToken = _localDataReadService.GetAuthData().AccessToken;

                var request = new HttpClient();

                request.DefaultRequestHeaders.Authorization 
                    = new AuthenticationHeaderValue( "Bearer", accessToken );

                var responseMessage = await request.GetAsync( baseUrl + endPoint );
                var resultContent = await responseMessage.Content.ReadAsStringAsync();

                return await GetParsedResponseResult<T>( responseMessage );
            }
            catch ( Exception e ) {
                return GetResponseResultWithExpection<T>( e );
            }   
        }

        public async Task<ResponseResult<T>> PostRequestAsync<T>( 
            string baseUrl, string endPoint, object objectToSend ) {
            try {
                await RefreshTokenIfExpired();
                baseUrl = AddLanguageTagToBaseUrl( baseUrl );
                string stringContent = JsonConvert.SerializeObject( objectToSend );
                string accessToken = _localDataReadService.GetAuthData().AccessToken;

                var request = new HttpClient();

                request.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue( "Bearer", accessToken );

                HttpContent content = new StringContent(
                    stringContent, Encoding.UTF8, "application/json" );

                PrintSendingRequestDataInConsole( baseUrl, endPoint, stringContent );

                var responseMessage = await request.PostAsync( baseUrl + endPoint, content );

                return await GetParsedResponseResult<T>( responseMessage );
            }
            catch ( Exception e ) {
                return GetResponseResultWithExpection<T>( e );
            }
        }

        public async Task<ResponseResult<T>> PostRequestWithMultipartFormData<T>( 
            string baseUrl, string endPoint, string mediaFileParamName, Stream mediaFileStream ) {
            try {
                await RefreshTokenIfExpired();
                baseUrl = AddLanguageTagToBaseUrl( baseUrl );
                string accessToken = _localDataReadService.GetAuthData().AccessToken;
                var formData = new MultipartFormDataContent();

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue( "Bearer", accessToken );

                httpClient.Timeout = new TimeSpan( 1, 0, 0 );

                var streamContent = new StreamContent( mediaFileStream );

                string fileExtention = Path.GetExtension( ((FileStream)mediaFileStream).Name );
                string fileName = $"{Guid.NewGuid()}{fileExtention}";

                formData.Add( streamContent, mediaFileParamName, fileName );

                var responseMessage = await httpClient.PostAsync( baseUrl + endPoint, formData );

                return await GetParsedResponseResult<T>( responseMessage );
            }
            catch ( Exception e ) {
                return GetResponseResultWithExpection<T>( e );
            }
        }

        public async Task<ResponseResult<T>> PutRequestAsync<T>( 
            string baseUrl, string endPoint, object objectToSend ) {
            try {
                await RefreshTokenIfExpired();
                baseUrl = AddLanguageTagToBaseUrl( baseUrl );
                string stringContent = JsonConvert.SerializeObject( objectToSend );
                string accessToken = _localDataReadService.GetAuthData().AccessToken;

                var request = new HttpClient();

                request.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue( "Bearer", accessToken );

                HttpContent content = new StringContent(
                    stringContent, Encoding.UTF8, "application/json" );

                PrintSendingRequestDataInConsole( baseUrl, endPoint, stringContent );

                var responseMessage = await request.PutAsync( baseUrl + endPoint, content );

                return await GetParsedResponseResult<T>( responseMessage );
            }
            catch ( Exception e ) {
                return GetResponseResultWithExpection<T>( e );
            }
        }

        public async Task<ResponseResult<T>> DeleteRequestAsync<T>( string baseUrl, string endPoint ) {
            try {
                await RefreshTokenIfExpired();
                baseUrl = AddLanguageTagToBaseUrl( baseUrl );
                string accessToken = _localDataReadService.GetAuthData().AccessToken;

                var request = new HttpClient();

                request.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue( "Bearer", accessToken );

                PrintDeleteRequestInConsole( baseUrl, endPoint );

                var responseMessage = await request.DeleteAsync( baseUrl + endPoint );

                return await GetParsedResponseResult<T>( responseMessage );
            }
            catch ( Exception e ) {
                return GetResponseResultWithExpection<T>( e );
            }
        }

        private async Task<ResponseResult<T>> GetParsedResponseResult<T>( 
            HttpResponseMessage httpResponseMessage ) {
            var resultContent = await httpResponseMessage.Content.ReadAsStringAsync();

            ResponseResult<T> responseResult = new ResponseResult<T>();

            if ( httpResponseMessage.IsSuccessStatusCode ) {
                try {
                    responseResult.data = JsonConvert.DeserializeObject<T>( resultContent );
                    responseResult.httpResponseMessage = httpResponseMessage;
                }
                catch ( Exception e ) {
                    return GetResponseResultWithExpection<T>( e );
                }
            }
            else {
                responseResult = new ResponseResult<T>();
                responseResult.httpResponseMessage = httpResponseMessage;
            }

            return responseResult;
        }

        private ResponseResult<T> GetResponseResultWithExpection<T>( Exception exeption ) {
            ResponseResult<T> errResponseResult
                    = new ResponseResult<T>();

            errResponseResult.errorException = exeption;

            return errResponseResult;
        }

        private async Task RefreshTokenIfExpired() {
            var authData = _localDataReadService.GetAuthData();
            if ( authData.IsTokenExpired ) {
                await _authService.PerformTokenRefreshAsync();
            }
        }
    }
}