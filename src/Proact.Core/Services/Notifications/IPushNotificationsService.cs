using System;
using System.Threading.Tasks;

namespace Proact.Mobile.Core.Services {
    public interface IPushNotificationsService {

        void RegisterPlayerId();
        void RegisterPlayerId( Action onPlayerIdRegistration );
        void RemovePlayerId();
        Task<bool> ActiveNotifications( bool active );
        Task<bool> SetNotificationTimeRangeForAllDays( DateTime startAtUtc, DateTime stopAtUtc );
        Task<PushNotificationsSettingsModel> GetSettings();
        Task<bool> ResetSettings();
        DateTime ConvertToUTC( TimeSpan timeSpan );
    }
}
