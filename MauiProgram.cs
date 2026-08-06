using ERP_PAYROLLMOBILE.Api;
using ERP_PAYROLLMOBILE.Api.EMPLOYEEROLE.EmployeeBreakdown;
using ERP_PAYROLLMOBILE.Api.EMPLOYEEROLE.EmployeeDaily;
using ERP_PAYROLLMOBILE.Api.EMPLOYEEROLE.EmployeeIShiftCode;
using ERP_PAYROLLMOBILE.Api.EMPLOYEEROLE.EmployeeRequest;
using ERP_PAYROLLMOBILE.Api.MANAGERROLE;
using ERP_PAYROLLMOBILE.RequestBuilders;
using ERP_PAYROLLMOBILE.Services.Features.Daily;
using ERP_PAYROLLMOBILE.Services.Features.Payroll;
using ERP_PAYROLLMOBILE.Services.Features.Requests;
using ERP_PAYROLLMOBILE.Services.Features.Shift;
using ERP_PAYROLLMOBILE.Services.Notifications;
using ERP_PAYROLLMOBILE.Services.RoleScope;
using ERP_PAYROLLMOBILE.Services.Ui;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using System.Net.Security;

namespace ERP_PAYROLLMOBILE
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddSingleton<PullToRefreshService>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddScoped<IRoleScopeService, RoleScopeService>();
            builder.Services.AddScoped<IdentityAuthService>();
            builder.Services.AddScoped<MoonAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<MoonAuthenticationStateProvider>());
            builder.Services.AddHttpClient("MoonIdentity", client =>
            {
                client.BaseAddress = new Uri("https://identity.moon.vn/");
            });

            builder.Services.AddScoped(sp =>
            {
                var handler = new HttpClientHandler();
#if DEBUG
                handler.ServerCertificateCustomValidationCallback = static (message, _, _, errors) =>
                {
                    var host = message?.RequestUri?.Host;
                    if (string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(host, "10.0.2.2", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    return errors == SslPolicyErrors.None;
                };
#endif

                return new HttpClient(handler)
                {
                    BaseAddress = GetApiBaseAddress()
                };
            });
            builder.Services.AddScoped<AttendanceRequestBuilder>();
            builder.Services.AddScoped<AttendanceApiClient>();
            builder.Services.AddScoped<ShiftcodeApiClient>();
            builder.Services.AddScoped<EmployeeBreakdownApiClient>();
            builder.Services.AddScoped<EmployeeRequestApiClient>();
            builder.Services.AddScoped<ManagerAttendanceDailyApiClient>();
            builder.Services.AddScoped<ManagerDepartmentsApiClient>();
            builder.Services.AddScoped<ManagerRequestsApiClient>();
            builder.Services.AddScoped<ManagerShiftApiClient>();
            builder.Services.AddScoped<ManagerShiftRegistrationApiClient>();
            builder.Services.AddScoped<IDailyFeatureService, DailyFeatureService>();
            builder.Services.AddScoped<EmployeeDailyStateService>();
            builder.Services.AddScoped<EmployeePayrollStateService>();
            builder.Services.AddScoped<EmployeeRequestStateService>();
            builder.Services.AddScoped<EmployeeShiftRegistrationStateService>();
            builder.Services.AddScoped<IRequestFeatureService, RequestFeatureService>();
            builder.Services.AddScoped<IShiftFeatureService, ShiftFeatureService>();
            builder.Services.AddScoped<IPayrollFeatureService, PayrollFeatureService>();
            builder.Services.AddScoped<IPushNotificationService, PushNotificationService>();
            builder.Services.AddScoped<MobileAppApiClient>();
            builder.Services.AddScoped<MobileTokenRegistrationService>();
            builder.Services.AddScoped<LogInApiclient>();
            builder.Services.AddScoped<EmployeeBreakdownRequestBuilder>();
            builder.Services.AddScoped<EmployeeRequestRequestBuilder>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static Uri GetApiBaseAddress()
        {
            return new Uri("https://salary83.moon.vn/api/");
        }
    }
}