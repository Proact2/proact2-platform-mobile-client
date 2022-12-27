using System;
namespace Proact.Mobile.Core {

    class TimeConversion {
        public static TimeSpan LocalToUTC( TimeSpan ts ) {
            DateTime dt = new DateTime( ts.Ticks ).AddDays( 1 );
            DateTime dtUtc = dt.ToUniversalTime();
            TimeSpan tsUtc = dtUtc.TimeOfDay;

            return tsUtc;
        }

        public static TimeSpan UTCToLocal( TimeSpan tsUtc ) {
            DateTime dtUtc = new DateTime( tsUtc.Ticks ).AddDays( 1 );
            DateTime dt = dtUtc.ToLocalTime();
            TimeSpan ts = dt.TimeOfDay;

            return ts;
        }
    }
}
