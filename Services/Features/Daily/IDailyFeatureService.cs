using ERP_PAYROLLMOBILE.Models;
using ERP_PAYROLLMOBILE.Models.ManagerDaily;
using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerAttendance;

namespace ERP_PAYROLLMOBILE.Services.Features.Daily;

public interface IDailyFeatureService
{
    Task<List<AttendanceDailyViewModel>> GetEmployeeDailyAsync(Guid companyId, string employeeId, DateOnly workDate, CancellationToken cancellationToken = default);
    Task<List<AttendanceDailyViewModel>> GetEmployeePeriodAsync(Guid companyId, string employeeId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default);
    Task<List<AttendanceDailyViewModel>> GetManagerDailyByDateAsync(Guid companyId, DateOnly workDate, CancellationToken cancellationToken = default);
    Task<List<AttendanceDailyViewModel>> GetManagerDailyByPeriodAsync(Guid companyId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default);
    Task<List<DepartmentAttendanceDailyApiModel>> GetManagerDailyAsync(Guid companyId, Guid departmentId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default);
    Task<string> SyncRangeAsync(Guid companyId, ManagerAttendanceDailySyncDto request, CancellationToken cancellationToken = default);
}
