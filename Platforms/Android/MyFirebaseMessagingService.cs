using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using ERP_PAYROLLMOBILE.Services.Notifications;
using ERP_PAYROLLMOBILE.RequestBuilders.Platforms.Android;
using Firebase.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;

[Service(Exported = false)]
[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
public class MyFirebaseMessagingService : FirebaseMessagingService
{
    public override async void OnNewToken(string token)
    {
        base.OnNewToken(token);
        await SecureStorage.Default.SetAsync(PushNotificationService.FcmTokenStorageKey, token);

        try
        {
            if (MainApplication.Services is null)
            {
                return;
            }

            using var scope = MainApplication.Services.CreateScope();
            var mobileTokenRegistrationService = scope.ServiceProvider.GetRequiredService<MobileTokenRegistrationService>();
            await mobileTokenRegistrationService.RegisterCurrentEmployeeTokenAsync(token);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Không đồng bộ được FCM token: {ex.Message}");
        }
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

        // KHỞI TẠO CHANNEL ID (Bắt buộc từ Android 8+)
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channelId = "default_notification_channel_id";
            var channelName = "Default Channel";
            var channelDescription = "Kênh thông báo mặc định";

            var channel = new NotificationChannel(channelId, channelName, NotificationImportance.High)
            {
                Description = channelDescription
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager?.CreateNotificationChannel(channel);
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
