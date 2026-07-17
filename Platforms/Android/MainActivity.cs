using Android.App;
using Android.Content.PM;
using Android.OS;

namespace ERP_PAYROLLMOBILE.RequestBuilders.Platforms.Android
{

    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        // Định nghĩa ID và tên của kênh thông báo (bạn có thể thay đổi tùy ý)
        internal static readonly string ChannelId = "default_notification_channel_id";
        internal static readonly string ChannelName = "Default Notifications";

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // ==========================================
            // ĐÃ THÊM: Ép buộc khởi tạo Firebase thủ công
            // ==========================================
            try
            {
                Firebase.FirebaseApp.InitializeApp(this);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khởi tạo Firebase: {ex.Message}");
            }

            CreateNotificationChannel();
        }

        private void CreateNotificationChannel()
        {
            // Chỉ tạo kênh thông báo nếu thiết bị chạy Android 8.0 (Oreo) trở lên
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var importance = NotificationImportance.High; // Độ ưu tiên cao để hiển thị dạng banner và phát âm thanh
                var channel = new NotificationChannel(ChannelId, ChannelName, importance)
                {
                    Description = "Kênh nhận thông báo chính của ứng dụng"
                };

                // Đăng ký kênh thông báo với hệ thống Android
                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager?.CreateNotificationChannel(channel);
            }
        }
    }
}
