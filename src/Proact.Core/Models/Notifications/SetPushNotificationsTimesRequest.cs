using System;
namespace Proact.Mobile.Core {
    public class SetPushNotificationsTimesRequest {
        public DateTime StartAtUtc { get; set; }
        public DateTime StopAtUtc { get; set; }
    }
}
