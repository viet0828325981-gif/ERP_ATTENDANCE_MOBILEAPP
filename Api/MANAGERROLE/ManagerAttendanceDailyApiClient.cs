using ERP_PAYROLLMOBILE.Models;
using ERP_PAYROLLMOBILE.Models.ManagerDaily;
using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerAttendance;

namespace ERP_PAYROLLMOBILE.Api.MANAGERROLE;

public sealed class ManagerAttendanceDailyApiClient : ManagerApiClientBase
{
    public ManagerAttendanceDailyApiClient(HttpClient http)
        : base(http, nameof(ManagerAttendanceDailyApiClient))
    {
    }

    public async Task<List<DepartmentAttendanceDailyApiModel>> GetDailyAsync(Guid companyId, Guid departmentId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        var url = $"manager/attendance-daily/GetDaily?companyId={companyId}&departmentId={departmentId}&from={Format(from)}&to={Format(to)}";
        var responseText = await SendGetAsync(url, "GetDaily", cancellationToken);
        return DeserializeList<DepartmentAttendanceDailyApiModel>(responseText);
    }

    public async Task<List<AttendanceDailyViewModel>> GetByDateAsync(Guid companyId, DateOnly workDate, CancellationToken cancellationToken = default)
    {
        var url = $"manager/attendance-daily/GetByDate?companyId={companyId}&workDate={Format(workDate)}";
        var responseText = await SendGetAsync(url, "GetByDate", cancellationToken);
        return DeserializeList<AttendanceDailyViewModel>(responseText);
    }

    public async Task<List<AttendanceDailyViewModel>> GetByPeriodAsync(Guid companyId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        var url = $"manager/attendance-daily/GetByPeriod?companyId={companyId}&from={Format(from)}&to={Format(to)}";
        var responseText = await SendGetAsync(url, "GetByPeriod", cancellationToken);
        return DeserializeList<AttendanceDailyViewModel>(responseText);
    }

    public async Task<List<AttendanceMonthlyRecordModel>> GetMonthlyRecordsAsync(Guid companyId, int year, int month, CancellationToken cancellationToken = default)
    {
        var url = $"manager/attendance-daily/GetMonthlyRecords?companyId={companyId}&year={Format(year)}&month={Format(month)}";
        var responseText = await SendGetAsync(url, "GetMonthlyRecords", cancellationToken);
        return DeserializeList<AttendanceMonthlyRecordModel>(responseText);
    }

    public async Task<string> SyncRangeAsync(Guid companyId, ManagerAttendanceDailySyncDto request, CancellationToken cancellationToken = default)
    {
        var url = $"manager/attendance-daily/SyncRange?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "SyncRange", cancellationToken);
        return DeserializeMessage(responseText);
    }
}
