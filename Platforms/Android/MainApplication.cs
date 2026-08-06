using Android.App;
using Android.Runtime;
using System;

namespace ERP_PAYROLLMOBILE.RequestBuilders.Platforms.Android
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public static IServiceProvider? Services { get; private set; }

        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp()
        {
            var app = MauiProgram.CreateMauiApp();
            Services = app.Services;
            return app;
        }
    }
}
