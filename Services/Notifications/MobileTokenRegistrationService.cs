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

    public async Task RegisterCurrentEmployeeTokenAsync(string? token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return;
        }

        var employeeId = await SecureStorage.Default.GetAsync(IdentityStorageKeys.EmployeeIdKey);
        if (string.IsNullOrWhiteSpace(employeeId))
        {
            return;
        }

        await mobileAppApiClient.InsertEmployeeMobileTokenAsync(new EmployeeMobileTokenRequestDto
        {
            EmployeeId = employeeId,
            Token = token
        }, cancellationToken);
    }
}
