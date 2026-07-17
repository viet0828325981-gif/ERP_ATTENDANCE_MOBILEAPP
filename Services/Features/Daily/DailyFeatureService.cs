using ERP_PAYROLLMOBILE.Api.EMPLOYEEROLE.EmployeeDaily;
using ERP_PAYROLLMOBILE.Api.MANAGERROLE;
using ERP_PAYROLLMOBILE.Models;
using ERP_PAYROLLMOBILE.Models.ManagerDaily;
using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerAttendance;

namespace ERP_PAYROLLMOBILE.Services.Features.Daily;

public sealed class DailyFeatureService : IDailyFeatureService
{
    private readonly AttendanceApiClient employeeApiClient;
    private readonly ManagerAttendanceDailyApiClient managerApiClient;

    public DailyFeatureService(AttendanceApiClient employeeApiClient, ManagerAttendanceDailyApiClient managerApiClient)
    {
        this.employeeApiClient = employeeApiClient;
        this.managerApiClient = managerApiClient;
    }

    public async Task<List<AttendanceDailyViewModel>> GetEmployeeDailyAsync(Guid companyId, string employeeId, DateOnly workDate, CancellationToken cancellationToken = default)
    {
        var record = await employeeApiClient.GetDailyAttendanceAsync(companyId, employeeId, workDate);
        return record is null ? new List<AttendanceDailyViewModel>() : new List<AttendanceDailyViewModel> { record };
    }

    public Task<List<AttendanceDailyViewModel>> GetEmployeePeriodAsync(Guid companyId, string employeeId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        return employeeApiClient.GetDailyBymonthAttendanceAsync(companyId, employeeId, from, to);
    }

    public Task<List<DepartmentAttendanceDailyApiModel>> GetManagerDailyAsync(Guid companyId, Guid departmentId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        return managerApiClient.GetDailyAsync(companyId, departmentId, from, to, cancellationToken);
    }

    public Task<List<AttendanceDailyViewModel>> GetManagerDailyByDateAsync(Guid companyId, DateOnly workDate, CancellationToken cancellationToken = default)
    {
        return managerApiClient.GetByDateAsync(companyId, workDate, cancellationToken);
    }

    public Task<List<AttendanceDailyViewModel>> GetManagerDailyByPeriodAsync(Guid companyId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        return managerApiClient.GetByPeriodAsync(companyId, from, to, cancellationToken);
    }

    public Task<string> SyncRangeAsync(Guid companyId, ManagerAttendanceDailySyncDto request, CancellationToken cancellationToken = default)
    {
        return managerApiClient.SyncRangeAsync(companyId, request, cancellationToken);
    }
}
