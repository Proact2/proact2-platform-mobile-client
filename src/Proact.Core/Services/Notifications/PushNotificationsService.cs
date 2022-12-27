using System;
using System.Threading.Tasks;
using OneSignalSDK.Xamarin;

namespace Proact.Mobile.Core.Services {

    public class PushNotificationsService : IPushNotificationsService {

        private const string _registerPlayerIdEndpoint = "Pushnotifications/register";
        private const string _removePlayerIdEndpoint = "Pushnotifications/remove/{0}";
        private const string _activeNotificationsEndpoint = "Pushnotifications/active";
        private const string _setNotificationsRangeEndpoint = "Pushnotifications/SetRange";
        private const string _getNotificationsSettingsEndpoint = "Pushnotifications/Settings";
        private const string _resetSettingsEndpoint = "Pushnotifications/reset";

        private INetworkRequestService _networkRequestService;

        private Action _OnPlayerIdRegistrationCallback;

        private string PushUserId {
            get {
                return OneSignal.Default.PushSubscriptionState.userId;
            }
        }

        private string PushToken {
            get {
                return OneSignal.Default.PushSubscriptionState.pushToken;
            }
        }

        public PushNotificationsService( INetworkRequestService networkRequestService ) {
            _networkRequestService = networkRequestService;
        }

        public void RegisterPlayerId() {
            RegisterPlayerIdWhenAvailable( PushUserId, PushToken );
        }

        public void RegisterPlayerId( Action onPlayerIdRegistration ) {
            _OnPlayerIdRegistrationCallback = onPlayerIdRegistration;
            RegisterPlayerIdWhenAvailable( PushUserId, PushToken );
        }

        public void RemovePlayerId() {
            RemovePlayerIdWhenAvailable( PushUserId, PushToken );
        }

        public async Task<bool> SetNotificationTimeRangeForAllDays(
            DateTime StartAtUtc, DateTime StopAtUtc ) {

            var request = new SetPushNotificationsTimesRequest() {
                StartAtUtc = StartAtUtc,
                StopAtUtc = StopAtUtc
            };

            var result = await _networkRequestService.PutRequestAsync<EmptyModel>(
                 ProactServerConfigurations.ApiUrl, _setNotificationsRangeEndpoint, request );
            return result.Success;
        }

        public async Task<bool> ActiveNotifications( bool active ) {
            var request = new ActivePushNotificationRequest() {
                Active = active
            };
            var result = await _networkRequestService.PutRequestAsync<EmptyModel>(
                 ProactServerConfigurations.ApiUrl, _activeNotificationsEndpoint, request );

            return result.Success;
        }

        private async void RegisterPlayerIdWhenAvailable( string playerID, string pushToken ) {
            var request = new SendPlayerIDRequest() {
                PlayerId = playerID
            };
            await _networkRequestService.PostRequestAsync<EmptyModel>(
                 ProactServerConfigurations.ApiUrl, _registerPlayerIdEndpoint, request );

            _OnPlayerIdRegistrationCallback?.Invoke();
        }

        private async void RemovePlayerIdWhenAvailable( string playerID, string pushToken ) {
            var endpoint = string.Format( _removePlayerIdEndpoint, playerID );
            await _networkRequestService.DeleteRequestAsync<EmptyModel>(
                ProactServerConfigurations.ApiUrl, endpoint );
        }

        public async Task<PushNotificationsSettingsModel> GetSettings() {
            var result = await _networkRequestService.GetRequestAsync<PushNotificationsSettingsModel>(
                ProactServerConfigurations.ApiUrl, _getNotificationsSettingsEndpoint );
            return result.data;
        }

        public async Task<bool> ResetSettings() {
            var result = await _networkRequestService.PutRequestAsync<EmptyModel>(
                ProactServerConfigurations.ApiUrl, _resetSettingsEndpoint, null );

            return result.Success;
        }

        public DateTime ConvertToUTC( TimeSpan timeSpan ) {

            var now = DateTime.Now;
            var dt = new DateTime(
                now.Year, now.Month, now.Day,
                timeSpan.Hours, timeSpan.Minutes, 0 );

            return TimeZoneInfo.ConvertTimeToUtc( dt );
        }
    }
}
