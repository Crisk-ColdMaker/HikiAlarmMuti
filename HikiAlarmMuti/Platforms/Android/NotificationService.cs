using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using HikiAlarmMuti.Services;
using Application = Android.App.Application;

namespace HikiAlarmMuti.Platforms.Android
{
    public class NotificationService : INotificationService
    {
        private readonly Context _context;

        public NotificationService()
        {
            _context = Application.Context;
        }

        public void ShowNotification(string title, string message)
        {
            var notificationManager = NotificationManagerCompat.From(_context);

            var notificationId = new Random().Next(1000, 9999);
            var channelId = "default_channel_id";

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelName = "Default Channel";
                var importance = NotificationImportance.Default;
                var channel = new NotificationChannel(channelId, channelName, importance);
                notificationManager.CreateNotificationChannel(channel);
            }

            var notificationBuilder = new NotificationCompat.Builder(_context, channelId)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.abc_star_black_48dp)
                .SetPriority(NotificationCompat.PriorityDefault)
                .SetAutoCancel(true);

            var notification = notificationBuilder.Build();
            notificationManager.Notify(notificationId, notification);
        }
    }
}
