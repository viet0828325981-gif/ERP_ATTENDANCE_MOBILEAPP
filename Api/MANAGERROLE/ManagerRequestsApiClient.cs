using ERP_PAYROLLMOBILE.Models;
using ERP_PAYROLLMOBILE.Models.EmployeeRequest;
using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerRequests;

namespace ERP_PAYROLLMOBILE.Api.MANAGERROLE;

public sealed class ManagerRequestsApiClient : ManagerApiClientBase
{
    public ManagerRequestsApiClient(HttpClient http)
        : base(http, nameof(ManagerRequestsApiClient))
    {
    }

    public async Task<List<WorkRequestModel>> GetRequestsAsync(Guid companyId,Guid departmentId, int year, int month, string? requestType = null, int? status = null, CancellationToken cancellationToken = default)
    {
        var url = $"manager/requests?companyId={companyId}&departmentId={departmentId}&year={Format(year)}&month={Format(month)}";
        if (!string.IsNullOrWhiteSpace(requestType))
        {
            url += $"&requestType={Escape(requestType)}";
        }

        if (status.HasValue)
        {
            url += $"&status={Format(status)}";
        }

        var responseText = await SendGetAsync(url, "GetRequests", cancellationToken);
        return DeserializeList<WorkRequestModel>(responseText);
    }

    public async Task<List<ManagerRequestTypeModel>> GetRequestTypesAsync(CancellationToken cancellationToken = default)
    {
        var responseText = await SendGetAsync("manager/requests/request-types", "GetRequestTypes", cancellationToken);
        return DeserializeList<ManagerRequestTypeModel>(responseText);
    }

    public async Task<List<ShiftModel>> GetOnlineShiftsAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        var url = $"manager/requests/online-shifts?companyId={companyId}";
        var responseText = await SendGetAsync(url, "GetOnlineShifts", cancellationToken);
        return DeserializeList<ShiftModel>(responseText);
    }

    public async Task<AttendanceDailyViewModel> GetDailyDetailAsync(Guid companyId, string employeeId, DateOnly workDate, CancellationToken cancellationToken = default)
    {
        var url = $"manager/requests/daily-detail?companyId={companyId}&employeeId={Escape(employeeId)}&workDate={Format(workDate)}";
        var responseText = await SendGetAsync(url, "GetDailyDetail", cancellationToken);
        return Deserialize<AttendanceDailyViewModel>(responseText, "chi tiết công theo ngày");
    }

    public async Task<List<WorkRequestModel>> GetOnlineGroupDetailAsync(Guid companyId, string employeeId, string requestType, DateTime createdAt, CancellationToken cancellationToken = default)
    {
        var url = $"manager/requests/online-group-detail?companyId={companyId}&employeeId={Escape(employeeId)}&requestType={Escape(requestType)}&createdAt={Escape(Format(createdAt))}";
        var responseText = await SendGetAsync(url, "GetOnlineGroupDetail", cancellationToken);
        return DeserializeList<WorkRequestModel>(responseText);
    }

    public async Task<WorkRequestModel> ApproveAsync(Guid companyId, ManagerApproveWorkRequestDto request, CancellationToken cancellationToken = default)
    {
        var url = $"manager/requests/approve?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "Approve", cancellationToken);
        return Deserialize<WorkRequestModel>(responseText, "yêu cầu đã duyệt");
    }

    public async Task<List<WorkRequestModel>> ApproveManyAsync(Guid companyId, ManagerApproveWorkRequestDto request, CancellationToken cancellationToken = default)
    {
        var url = $"manager/requests/approve?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "ApproveMany", cancellationToken);
        return DeserializeList<WorkRequestModel>(responseText);
    }

    public async Task<string> RejectAsync(Guid companyId, ManagerRejectWorkRequestDto request, CancellationToken cancellationToken = default)
    {
        var url = $"manager/requests/reject?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "Reject", cancellationToken);
        return DeserializeMessage(responseText);
    }

    public async Task<string> PostMakeupHourAsync(Guid companyId, ManagerMakeupWorkRequestDto request, CancellationToken cancellationToken = default)
    {
        var url = $"manager/requests/makeup-hour?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "MakeupHour", cancellationToken);
        return DeserializeMessage(responseText);
    }

    public async Task<string> PostWorkOnlineAsync(Guid companyId, ManagerOnlineWorkRequestDto request, CancellationToken cancellationToken = default)
    {
        var url = $"manager/requests/work-online?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "WorkOnline", cancellationToken);
        return DeserializeMessage(responseText);
    }

    public async Task<BulkWorkRequestResult> PostBulkAsync(Guid companyId, ManagerBulkWorkRequestDto request, CancellationToken cancellationToken = default)
    {
        var url = $"manager/requests/bulk?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "Bulk", cancellationToken);
        return Deserialize<BulkWorkRequestResult>(responseText, "kết quả tạo yêu cầu hàng loạt");
    }

    public async Task<ManagerMonthlyNightlyJobResult> RunMonthlyNightlyJobAsync(Guid companyId, ManagerMonthlyNightlyJobDto request, CancellationToken cancellationToken = default)
    {
        var url = $"manager/requests/run-monthly-nightly-job?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "RunMonthlyNightlyJob", cancellationToken);
        return Deserialize<ManagerMonthlyNightlyJobResult>(responseText, "kết quả nightly job");
    }
}
