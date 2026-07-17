using ERP_PAYROLLMOBILE.Api.EMPLOYEEROLE.EmployeeRequest;
using ERP_PAYROLLMOBILE.Api.MANAGERROLE;
using ERP_PAYROLLMOBILE.Models;
using ERP_PAYROLLMOBILE.Models.EmployeeRequest;
using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerRequests;

namespace ERP_PAYROLLMOBILE.Services.Features.Requests;

public sealed class RequestFeatureService : IRequestFeatureService
{
    private readonly EmployeeRequestApiClient employeeApiClient;
    private readonly ManagerRequestsApiClient managerApiClient;

    public RequestFeatureService(EmployeeRequestApiClient employeeApiClient, ManagerRequestsApiClient managerApiClient)
    {
        this.employeeApiClient = employeeApiClient;
        this.managerApiClient = managerApiClient;
    }

    public Task<List<WorkRequestModel>> GetEmployeeRequestsAsync(Guid companyId, string employeeId, int year, int month, string? requestType = null, CancellationToken cancellationToken = default)
    {
        return employeeApiClient.GetRequestsAsync(companyId, employeeId, year, month, requestType, true, cancellationToken);
    }

    public Task<List<WorkRequestModel>> GetManagerRequestsAsync(Guid companyId, Guid departmentId, int year, int month, string? requestType = null, int? status = null, CancellationToken cancellationToken = default)
    {
        return managerApiClient.GetRequestsAsync(companyId, departmentId, year, month, requestType, status, cancellationToken);
    }

    public Task<List<ManagerRequestTypeModel>> GetManagerRequestTypesAsync(CancellationToken cancellationToken = default)
    {
        return managerApiClient.GetRequestTypesAsync(cancellationToken);
    }

    public Task<List<ShiftModel>> GetOnlineShiftsAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        return managerApiClient.GetOnlineShiftsAsync(companyId, cancellationToken);
    }

    public Task<AttendanceDailyViewModel> GetDailyDetailAsync(Guid companyId, string employeeId, DateOnly workDate, CancellationToken cancellationToken = default)
    {
        return managerApiClient.GetDailyDetailAsync(companyId, employeeId, workDate, cancellationToken);
    }

    public Task<List<WorkRequestModel>> GetOnlineGroupDetailAsync(Guid companyId, string employeeId, string requestType, DateTime createdAt, CancellationToken cancellationToken = default)
    {
        return managerApiClient.GetOnlineGroupDetailAsync(companyId, employeeId, requestType, createdAt, cancellationToken);
    }

    public Task<WorkRequestModel> ApproveAsync(Guid companyId, ManagerApproveWorkRequestDto request, CancellationToken cancellationToken = default)
    {
        return managerApiClient.ApproveAsync(companyId, request, cancellationToken);
    }

    public Task<string> RejectAsync(Guid companyId, ManagerRejectWorkRequestDto request, CancellationToken cancellationToken = default)
    {
        return managerApiClient.RejectAsync(companyId, request, cancellationToken);
    }

    public Task<BulkWorkRequestResult> BulkAsync(Guid companyId, ManagerBulkWorkRequestDto request, CancellationToken cancellationToken = default)
    {
        return managerApiClient.PostBulkAsync(companyId, request, cancellationToken);
    }

    public Task<ManagerMonthlyNightlyJobResult> RunMonthlyNightlyJobAsync(Guid companyId, ManagerMonthlyNightlyJobDto request, CancellationToken cancellationToken = default)
    {
        return managerApiClient.RunMonthlyNightlyJobAsync(companyId, request, cancellationToken);
    }
}
