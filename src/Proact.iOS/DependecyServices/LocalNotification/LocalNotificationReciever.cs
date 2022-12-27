﻿using System;
using Proact.Mobile.Core;
using UserNotifications;
using Xamarin.Forms;

namespace Proact.Mobile.iOS {
    public class LocalNotificationReciever : UNUserNotificationCenterDelegate {
        // Called if app is in the foreground.
        public override void WillPresentNotification( UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler ) {
            ProcessNotification( notification );
            completionHandler( UNNotificationPresentationOptions.Alert );
        }

        // Called if app is in the background, or killed state.
        public override void DidReceiveNotificationResponse( UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler ) {
            if ( response.IsDefaultAction ) {
                ProcessNotification( response.Notification );
            }
            completionHandler();
        }

        void ProcessNotification( UNNotification notification ) {
            string title = notification.Request.Content.Title;
            string message = notification.Request.Content.Body;

            DependencyService.Get<ILocalNotificationManager>().ReceiveNotification( title, message );
        }
    }
}