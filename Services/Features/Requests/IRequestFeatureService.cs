using ERP_PAYROLLMOBILE.Models;
using ERP_PAYROLLMOBILE.Models.EmployeeRequest;
using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerRequests;

namespace ERP_PAYROLLMOBILE.Services.Features.Requests;

public interface IRequestFeatureService
{
    Task<List<WorkRequestModel>> GetEmployeeRequestsAsync(Guid companyId, string employeeId, int year, int month, string? requestType = null, CancellationToken cancellationToken = default);
    Task<List<WorkRequestModel>> GetManagerRequestsAsync(Guid companyId, Guid departmentId, int year, int month, string? requestType = null, int? status = null, CancellationToken cancellationToken = default);
    Task<List<ManagerRequestTypeModel>> GetManagerRequestTypesAsync(CancellationToken cancellationToken = default);
    Task<List<ShiftModel>> GetOnlineShiftsAsync(Guid companyId, CancellationToken cancellationToken = default);
    Task<AttendanceDailyViewModel> GetDailyDetailAsync(Guid companyId, string employeeId, DateOnly workDate, CancellationToken cancellationToken = default);
    Task<List<WorkRequestModel>> GetOnlineGroupDetailAsync(Guid companyId, string employeeId, string requestType, DateTime createdAt, CancellationToken cancellationToken = default);
    Task<WorkRequestModel> ApproveAsync(Guid companyId, ManagerApproveWorkRequestDto request, CancellationToken cancellationToken = default);
    Task<string> RejectAsync(Guid companyId, ManagerRejectWorkRequestDto request, CancellationToken cancellationToken = default);
    Task<BulkWorkRequestResult> BulkAsync(Guid companyId, ManagerBulkWorkRequestDto request, CancellationToken cancellationToken = default);
    Task<ManagerMonthlyNightlyJobResult> RunMonthlyNightlyJobAsync(Guid companyId, ManagerMonthlyNightlyJobDto request, CancellationToken cancellationToken = default);
}
