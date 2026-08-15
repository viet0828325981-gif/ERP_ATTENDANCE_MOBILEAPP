using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;

#if ANDROID
using Android.Gms.Tasks;
using Firebase.Messaging;
#endif

namespace ERP_PAYROLLMOBILE.Services.Notifications;

public sealed class PushNotificationService : IPushNotificationService
{
    public const string FcmTokenStorageKey = "fcm_token";

    public async Task<string?> InitializeAsync(System.Threading.CancellationToken cancellationToken = default)
    {
#if ANDROID
        var permissionStatus = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
        if (permissionStatus != PermissionStatus.Granted)
        {
            permissionStatus = await Permissions.RequestAsync<Permissions.PostNotifications>();
        }

        if (permissionStatus != PermissionStatus.Granted)
        {
            return null;
        }

        var token = await GetFirebaseTokenAsync(cancellationToken);
        if (!string.IsNullOrWhiteSpace(token))
        {
            await SecureStorage.Default.SetAsync(FcmTokenStorageKey, token);
        }

        return token;
#else
        await Task.CompletedTask;
        return null;
#endif
    }

#if ANDROID
    private static Task<string?> GetFirebaseTokenAsync(System.Threading.CancellationToken cancellationToken)
    {
        var tokenTask = FirebaseMessaging.Instance.GetToken();
        var taskCompletionSource = new TaskCompletionSource<string?>();
        using var registration = cancellationToken.Register(() => taskCompletionSource.TrySetCanceled(cancellationToken));

        tokenTask.AddOnCompleteListener(new FirebaseTokenCompleteListener(taskCompletionSource));

        return taskCompletionSource.Task;
    }

    private sealed class FirebaseTokenCompleteListener : Java.Lang.Object, IOnCompleteListener
    {
        private readonly TaskCompletionSource<string?> taskCompletionSource;

        public FirebaseTokenCompleteListener(TaskCompletionSource<string?> taskCompletionSource)
        {
            this.taskCompletionSource = taskCompletionSource;
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                taskCompletionSource.TrySetResult(task.Result?.ToString());
                return;
            }

            taskCompletionSource.TrySetException(new InvalidOperationException(task.Exception?.Message ?? "Không thể lấy FCM token."));
        }
    }
#endif
}
