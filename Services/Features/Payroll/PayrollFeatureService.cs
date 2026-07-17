using ERP_PAYROLLMOBILE.Api.EMPLOYEEROLE.EmployeeBreakdown;
using ERP_PAYROLLMOBILE.Models.EmployeeBreakdown;

namespace ERP_PAYROLLMOBILE.Services.Features.Payroll;

public sealed class PayrollFeatureService : IPayrollFeatureService
{
    private readonly EmployeeBreakdownApiClient employeeApiClient;

    public PayrollFeatureService(EmployeeBreakdownApiClient employeeApiClient)
    {
        this.employeeApiClient = employeeApiClient;
    }

    public Task<PayrollBreakdownModel> GetBreakdownAsync(Guid companyId, string employeeId, int year, int month, CancellationToken cancellationToken = default)
    {
        return employeeApiClient.GetBreakdownAsync(companyId, employeeId, year, month, cancellationToken);
    }

    public Task<ERPATT_CongphepduModel> GetLeaveBalanceAsync(string employeeId, int year, int month, CancellationToken cancellationToken = default)
    {
        return employeeApiClient.GetLeaveBalanceAsync(employeeId, year, month, cancellationToken);
    }

    public Task<List<ERPATT_CongphepduModel>> GetLeaveBalanceHistoryAsync(string employeeId, CancellationToken cancellationToken = default)
    {
        return employeeApiClient.GetLeaveBalanceHistoryAsync(employeeId, cancellationToken);
    }

    public Task<List<SpecialWorkClaimDetailModel>> GetSpecialWorkClaimsAsync(Guid companyId, string employeeId, int year, int month, CancellationToken cancellationToken = default)
    {
        return employeeApiClient.GetSpecialWorkClaimsAsync(companyId, employeeId, year, month, cancellationToken);
    }

    public Task<EmployeeShiftRegistrationSummaryDto> GetShiftRegistrationSummaryAsync(Guid companyId, string employeeId, int year, int month, CancellationToken cancellationToken = default)
    {
        return employeeApiClient.GetShiftRegistrationSummaryAsync(companyId, employeeId, year, month, cancellationToken);
    }

    public Task<EmployeeShiftRegistrationDetailResponseDto> GetShiftRegistrationDetailAsync(Guid companyId, string employeeId, int year, int month, CancellationToken cancellationToken = default)
    {
        return employeeApiClient.GetShiftRegistrationDetailAsync(companyId, employeeId, year, month, cancellationToken);
    }

    public Task<int> SyncAsync(Guid companyId, EmployeeBreakdownSyncDto request, CancellationToken cancellationToken = default)
    {
        return employeeApiClient.SyncAsync(companyId, request, cancellationToken);
    }
}
