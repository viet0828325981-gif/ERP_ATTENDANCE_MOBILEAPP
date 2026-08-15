using Microsoft.AspNetCore.Components.WebView;
using Microsoft.AspNetCore.Components.WebView.Maui;
using ERP_PAYROLLMOBILE.Services.Ui;
using Microsoft.Maui.ApplicationModel;
#if WINDOWS
using Microsoft.UI.Xaml.Controls;
#endif

namespace ERP_PAYROLLMOBILE
{
    public partial class MainPage : ContentPage
    {
        private readonly PullToRefreshService pullToRefreshService;

        public MainPage(PullToRefreshService pullToRefreshService)
        {
            InitializeComponent();
            this.pullToRefreshService = pullToRefreshService;

#if DEBUG && WINDOWS
            blazorWebView.BlazorWebViewInitialized += OnBlazorWebViewInitialized;
#endif
        }

        private async void OnRefreshing(object? sender, EventArgs e)
        {
            try
            {
                await pullToRefreshService.RequestRefreshAsync();
            }
            finally
            {
                if (sender is RefreshView refreshView)
                {
                    refreshView.IsRefreshing = false;
                }
            }
        }

#if DEBUG && WINDOWS
        private static void OnBlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs e)
        {
            if (e.WebView is WebView2 webView)
            {
                webView.CoreWebView2.OpenDevToolsWindow();
            }
        }
#endif

        // ==========================================
        // ĐOẠN MÃ XIN QUYỀN THÔNG BÁO ANDROID 
        // ==========================================
#if ANDROID
        private async Task RequestNotificationPermissionAsync()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.PostNotifications>();
            }

            if (status == PermissionStatus.Granted)
            {
                try
                {
                    // 1. Kiểm tra an toàn xem Firebase đã khởi tạo thành công chưa
                    if (Firebase.FirebaseApp.Instance == null)
                    {
                        await DisplayAlert("Lỗi", "Firebase chưa được khởi tạo! Hãy kiểm tra lại file google-services.json.", "OK");
                        return;
                    }

                    // 2. Tạo TaskCompletionSource để lấy Token an toàn không lo crash
                    var tcs = new TaskCompletionSource<string>();
                    var tokenTask = Firebase.Messaging.FirebaseMessaging.Instance.GetToken();

                    tokenTask.AddOnCompleteListener(new MyFcmCompleteListener(tcs));

                    await tcs.Task;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Lỗi Firebase", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Thông báo", "Quyền thông báo bị từ chối.", "OK");
            }
        }

        // Lớp bổ trợ lắng nghe sự kiện lấy Token từ Google Play Services
        public class MyFcmCompleteListener : Java.Lang.Object, Android.Gms.Tasks.IOnCompleteListener
        {
            private readonly TaskCompletionSource<string> _tcs;
            public MyFcmCompleteListener(TaskCompletionSource<string> tcs) => _tcs = tcs;

            public void OnComplete(Android.Gms.Tasks.Task task)
            {
                if (task.IsSuccessful)
                    _tcs.SetResult(task.Result.ToString());
                else
                    _tcs.SetException(task.Exception ?? new Exception("Không thể lấy Token từ Google Play Services"));
            }
        }
#endif

    }
}
