using ERP_PAYROLLMOBILE.Models.EmployeeRequest;
using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerShift;

namespace ERP_PAYROLLMOBILE.Api.MANAGERROLE;

public sealed class ManagerShiftApiClient : ManagerApiClientBase
{
    public ManagerShiftApiClient(HttpClient http)
        : base(http, nameof(ManagerShiftApiClient))
    {
    }

    public async Task<List<ShiftModel>> GetShiftsAsync(Guid companyId,Guid departmentId, CancellationToken cancellationToken = default)
    {
        var url = $"manager/shift/shifts?companyId={companyId}&departmentId={departmentId}";
        var responseText = await SendGetAsync(url, "GetShifts", cancellationToken);
        return DeserializeList<ShiftModel>(responseText);
    }

    public async Task<List<EmployeeShiftSummaryModel>> GetEmployeeSummariesAsync(Guid companyId, int year, int month, CancellationToken cancellationToken = default)
    {
        var url = $"manager/shift/employee-summaries?companyId={companyId}&year={Format(year)}&month={Format(month)}";
        var responseText = await SendGetAsync(url, "GetEmployeeSummaries", cancellationToken);
        return DeserializeList<EmployeeShiftSummaryModel>(responseText);
    }

    public async Task<List<EmployeeDetailShiftCodeInMonthModel>> GetEmployeeDayDetailsAsync(Guid companyId, string employeeId, int month, int year, CancellationToken cancellationToken = default)
    {
        var url = $"manager/shift/employee-day-details?companyId={companyId}&employeeId={Escape(employeeId)}&month={Format(month)}&year={Format(year)}";
        var responseText = await SendGetAsync(url, "GetEmployeeDayDetails", cancellationToken);
        return DeserializeList<EmployeeDetailShiftCodeInMonthModel>(responseText);
    }

    public async Task<List<WeeklyShiftScheduleRow>> GetWeeklyScheduleAsync(Guid companyId, Guid departmentId, DateOnly weekStartDate, CancellationToken cancellationToken = default)
    {
        var url = $"manager/shift/weekly-schedule?companyId={companyId}&departmentId={departmentId}&weekStartDate={Format(weekStartDate)}";
        var responseText = await SendGetAsync(url, "GetWeeklySchedule", cancellationToken);
        return DeserializeList<WeeklyShiftScheduleRow>(responseText);
    }

    public async Task<string> RegisterWeekAsync(Guid companyId, ManagerRegisterWeekShiftDto request, CancellationToken cancellationToken = default)
    {
        var url = $"manager/shift/register-week?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "RegisterWeek", cancellationToken);
        return DeserializeMessage(responseText);
    }

    public async Task<string> RegisterMonthAsync(Guid companyId, ManagerRegisterMonthShiftDto request, CancellationToken cancellationToken = default)
    {
        var url = $"manager/shift/register-month?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "RegisterMonth", cancellationToken);
        return DeserializeMessage(responseText);
    }
}
