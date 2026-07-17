using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using ERP_PAYROLLMOBILE.Services.Notifications;
using Firebase.Messaging;
using Microsoft.Maui.Storage;

[Service(Exported = false)]
[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
public class MyFirebaseMessagingService : FirebaseMessagingService
{
    public override async void OnNewToken(string token)
    {
        base.OnNewToken(token);
        await SecureStorage.Default.SetAsync(PushNotificationService.FcmTokenStorageKey, token);
        System.Diagnostics.Debug.WriteLine($"FCM Token của bạn: {token}");
    }

    public override void OnMessageReceived(RemoteMessage message)
    {
        base.OnMessageReceived(message);

        var notification = message.GetNotification();
        var title = notification?.Title ?? GetDataValue(message, "title") ?? ApplicationInfo?.LoadLabel(PackageManager);
        var body = notification?.Body ?? GetDataValue(message, "body") ?? GetDataValue(message, "message");

        if (!string.IsNullOrWhiteSpace(body))
        {
            ShowNotification(title?.ToString() ?? "Thông báo", body);
        }
    }

    private void ShowNotification(string title, string body)
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu
            && ContextCompat.CheckSelfPermission(this, global::Android.Manifest.Permission.PostNotifications) != Permission.Granted)
        {
            return;
        }

        var intent = PackageManager?.GetLaunchIntentForPackage(PackageName ?? string.Empty) ?? new Intent();
        intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);

        var pendingIntent = PendingIntent.GetActivity(
            this,
            0,
            intent,
            PendingIntentFlags.Immutable | PendingIntentFlags.UpdateCurrent);

        var notification = new NotificationCompat.Builder(this, "default_notification_channel_id")
            .SetSmallIcon(global::ERP_PAYROLLMOBILE.Resource.Mipmap.appicon)
            .SetContentTitle(title)
            .SetContentText(body)
            .SetStyle(new NotificationCompat.BigTextStyle().BigText(body))
            .SetAutoCancel(true)
            .SetPriority(NotificationCompat.PriorityHigh)
            .SetContentIntent(pendingIntent)
            .Build();

        NotificationManagerCompat.From(this).Notify(DateTimeOffset.UtcNow.Millisecond, notification);
    }

    private static string? GetDataValue(RemoteMessage message, string key)
    {
        return message.Data is not null && message.Data.TryGetValue(key, out var value) ? value : null;
    }
}
