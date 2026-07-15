using Microsoft.AspNetCore.Components.WebView;
using Microsoft.AspNetCore.Components.WebView.Maui;
using ERP_PAYROLLMOBILE.Services.Ui;

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
    }
}
