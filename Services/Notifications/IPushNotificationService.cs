namespace ERP_PAYROLLMOBILE.Services.Notifications;

public interface IPushNotificationService
{
    Task<string?> InitializeAsync(CancellationToken cancellationToken = default);
}
