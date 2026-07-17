using ERP_PAYROLLMOBILE.Models.EmployeeRequest;
using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerShiftRegistration;

namespace ERP_PAYROLLMOBILE.Api.MANAGERROLE;

public sealed class ManagerShiftRegistrationApiClient : ManagerApiClientBase
{
    public ManagerShiftRegistrationApiClient(HttpClient http)
        : base(http, nameof(ManagerShiftRegistrationApiClient))
    {
    }

    public async Task<List<WorkRequestModel>> GetShiftRegistrationsAsync(Guid companyId, int year, int month, Guid? departmentId = null, int? status = null, CancellationToken cancellationToken = default)
    {
        var url = $"manager/shift-registration?companyId={companyId}&year={Format(year)}&month={Format(month)}";
        if (departmentId.HasValue)
        {
            url += $"&departmentId={departmentId.Value}";
        }

        if (status.HasValue)
        {
            url += $"&status={Format(status)}";
        }

        var responseText = await SendGetAsync(url, "GetShiftRegistrations", cancellationToken);
        return DeserializeList<WorkRequestModel>(responseText);
    }

    public async Task<List<ShiftRegistrationDayDetailModel>> GetRegistrationDaysAsync(Guid companyId, Guid requestId, CancellationToken cancellationToken = default)
    {
        var url = $"manager/shift-registration/registration-days?companyId={companyId}&requestId={requestId}";
        var responseText = await SendGetAsync(url, "GetRegistrationDays", cancellationToken);
        return DeserializeList<ShiftRegistrationDayDetailModel>(responseText);
    }

    public async Task<List<ShiftEditChangeDetailModel>> GetEditChangesAsync(Guid companyId, Guid requestId, CancellationToken cancellationToken = default)
    {
        var url = $"manager/shift-registration/edit-changes?companyId={companyId}&requestId={requestId}";
        var responseText = await SendGetAsync(url, "GetEditChanges", cancellationToken);
        return DeserializeList<ShiftEditChangeDetailModel>(responseText);
    }

    public async Task<string> ApproveAsync(Guid companyId, ManagerApproveShiftRegistrationDto request, CancellationToken cancellationToken = default)
    {
        var url = $"manager/shift-registration/approve?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "Approve", cancellationToken);
        return DeserializeMessage(responseText);
    }

    public async Task<ManagerApproveAllShiftRegistrationResult> ApproveAllAsync(Guid companyId, ManagerApproveAllShiftRegistrationDto request, CancellationToken cancellationToken = default)
    {
        var url = $"manager/shift-registration/approve-all?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "ApproveAll", cancellationToken);
        return Deserialize<ManagerApproveAllShiftRegistrationResult>(responseText, "kết quả duyệt tất cả đăng ký ca");
    }

    public async Task<string> RejectAsync(Guid companyId, ManagerRejectShiftRegistrationDto request, CancellationToken cancellationToken = default)
    {
        var url = $"manager/shift-registration/reject?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "Reject", cancellationToken);
        return DeserializeMessage(responseText);
    }
}
