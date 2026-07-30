using ERP_PAYROLLMOBILE.Api;
using Microsoft.Maui.Storage;

namespace ERP_PAYROLLMOBILE.Services.Notifications;

public sealed class MobileTokenRegistrationService
{
    private readonly MobileAppApiClient mobileAppApiClient;

    public MobileTokenRegistrationService(MobileAppApiClient mobileAppApiClient)
    {
        this.mobileAppApiClient = mobileAppApiClient;
    }

    public async Task<string> RegisterCurrentEmployeeTokenAsync(string? token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return "FCM token rỗng, không gọi API InsertEmployeeMobileTokenAsync.";
        }

        var employeeId = await SecureStorage.Default.GetAsync(IdentityStorageKeys.EmployeeIdKey);
        if (string.IsNullOrWhiteSpace(employeeId))
        {
            return "Không có employeeId đăng nhập, không gọi API InsertEmployeeMobileTokenAsync.";
        }

        var responseText = await mobileAppApiClient.InsertEmployeeMobileTokenAsync(new EmployeeMobileTokenRequestDto
        {
            EmployeeId = employeeId,
            Token = token
        }, cancellationToken);

        return $"InsertEmployeeMobileTokenAsync thành công. EmployeeId: {employeeId}. Response: {responseText}";
    }
}