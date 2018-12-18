using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Firebase.Messaging;
using CABASUS;
using SQLite;
using System;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static Android.App.ActivityManager;

namespace RecibirNotificcion.Firebase
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";

        public override void OnMessageReceived(RemoteMessage message)
        {

            if (message.GetNotification() != null)
            {
                SendNotificationAsync(message.GetNotification().Body);
            }
            else
            {
                var data = message.Data.ToDictionary(i => i.Key, i => i.Value);
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                SendNotificationAsync(json);
            }
        }

        async Task SendNotificationAsync(string messageBody)
        {
            var root = JsonConvert.DeserializeObject<RootObject>(messageBody);
            var intent = new Intent(this, typeof(ActivityPrincipal));
            intent.AddFlags(ActivityFlags.ClearTop);

            var pendingIntent = PendingIntent.GetActivity(this, ActivityPrincipal.NOTIFICATION_ID, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, ActivityPrincipal.CHANNEL_ID)
                                      .SetSmallIcon(Resource.Drawable.bell_icon)
                                      .SetContentTitle("FCM Message")
                                      .SetContentText(root.men)
                                      .SetAutoCancel(true)
                                      .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(ActivityPrincipal.NOTIFICATION_ID, notificationBuilder.Build());
            
            if (isApplicationInTheBackground() == false)
            {
                CancelPush();
            }
        }

        private bool isApplicationInTheBackground()
        {
            bool isInBackground;

            RunningAppProcessInfo myProcess = new RunningAppProcessInfo();
            ActivityManager.GetMyMemoryState(myProcess);
            isInBackground = myProcess.Importance != Android.App.Importance.Foreground;

            return isInBackground;
        }

        public void CancelPush()
        {
            var notificationManager = Android.Support.V4.App.NotificationManagerCompat.From(Android.App.Application.Context);
            notificationManager.CancelAll();
        }

    }

    public class RootObject
    {
        public string id { get; set; }
        public string men { get; set; }
    }
}