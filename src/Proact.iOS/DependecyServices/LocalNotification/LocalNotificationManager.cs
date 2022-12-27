using System;
using Foundation;
using Proact.Mobile.Core;
using Proact.Mobile.iOS;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof( LocalNotificationManager ) )]
namespace Proact.Mobile.iOS {
    public class LocalNotificationManager : ILocalNotificationManager {
        int messageId = 0;
        bool hasNotificationsPermission;
        public event EventHandler NotificationReceived;

        public void Initialize() {
            RequestAuthorization();
        }

        private void RequestAuthorization() {
            UNUserNotificationCenter.Current.RequestAuthorization(
              UNAuthorizationOptions.Alert, ( approved, err ) => {
                  hasNotificationsPermission = approved;
              } );
        }

        public void SendNotification(
            string title, string message, DateTime? notifyTime = null ) {
            if ( !hasNotificationsPermission ) {
                return;
            }

            messageId++;
            var content = new UNMutableNotificationContent() {
                Title = title,
                Subtitle = string.Empty,
                Body = message,
                Badge = 1
            };

            UNNotificationTrigger trigger;
            if ( notifyTime != null ) {
                trigger = UNCalendarNotificationTrigger
                    .CreateTrigger( GetNSDateComponents( notifyTime.Value ), false );
            }
            else {
                trigger = UNTimeIntervalNotificationTrigger
                    .CreateTrigger( 0.25, false );
            }

            var request = UNNotificationRequest
                .FromIdentifier( messageId.ToString(), content, trigger );
            UNUserNotificationCenter.Current
                .AddNotificationRequest( request, ( err ) => {
                if ( err != null ) {
                    throw new Exception( $"Failed to schedule notification: {err}" );
                }
            } );
        }

        public void ReceiveNotification( string title, string message ) {
            var args = new NotificationEventArgs() {
                Title = title,
                Message = message
            };
            NotificationReceived?.Invoke( null, args );
        }

        NSDateComponents GetNSDateComponents( DateTime dateTime ) {
            return new NSDateComponents {
                Month = dateTime.Month,
                Day = dateTime.Day,
                Year = dateTime.Year,
                Hour = dateTime.Hour,
                Minute = dateTime.Minute,
                Second = dateTime.Second
            };
        }
    }
}