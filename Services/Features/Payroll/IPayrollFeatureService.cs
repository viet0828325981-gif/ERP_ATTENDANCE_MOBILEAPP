using ERP_PAYROLLMOBILE.Models.EmployeeBreakdown;

namespace ERP_PAYROLLMOBILE.Services.Features.Payroll;

public interface IPayrollFeatureService
{
    Task<PayrollBreakdownModel> GetBreakdownAsync(Guid companyId, string employeeId, int year, int month, CancellationToken cancellationToken = default);
    Task<ERPATT_CongphepduModel> GetLeaveBalanceAsync(string employeeId, int year, int month, CancellationToken cancellationToken = default);
    Task<List<ERPATT_CongphepduModel>> GetLeaveBalanceHistoryAsync(string employeeId, CancellationToken cancellationToken = default);
    Task<List<SpecialWorkClaimDetailModel>> GetSpecialWorkClaimsAsync(Guid companyId, string employeeId, int year, int month, CancellationToken cancellationToken = default);
    Task<EmployeeShiftRegistrationSummaryDto> GetShiftRegistrationSummaryAsync(Guid companyId, string employeeId, int year, int month, CancellationToken cancellationToken = default);
    Task<EmployeeShiftRegistrationDetailResponseDto> GetShiftRegistrationDetailAsync(Guid companyId, string employeeId, int year, int month, CancellationToken cancellationToken = default);
    Task<int> SyncAsync(Guid companyId, EmployeeBreakdownSyncDto request, CancellationToken cancellationToken = default);
}
