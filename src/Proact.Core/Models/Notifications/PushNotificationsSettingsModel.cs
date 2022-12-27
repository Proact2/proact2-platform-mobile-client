using System;
namespace Proact.Mobile.Core {
    public class PushNotificationsSettingsModel {

        public bool Active { get; set; }
        public bool AllDay { get; set; }
        public DateTime StartAtUtc { get; set; } 
        public DateTime StopAtUtc { get; set; }

        public TimeSpan LocalStartAt {
            get => TimeConversion.UTCToLocal( StartAtUtc.TimeOfDay );
        }

        public TimeSpan LocalStopAt {
            get => TimeConversion.UTCToLocal( StopAtUtc.TimeOfDay );
        }

        public string FormattedTimeFrom {
            get => LocalStartAt.ToString( @"hh\:mm" );
        }

        public string FormattedTimeTo {
            get => LocalStopAt.ToString( @"hh\:mm" );
        }
    }
}
