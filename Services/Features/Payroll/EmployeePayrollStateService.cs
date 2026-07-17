using ERP_PAYROLLMOBILE.Models.EmployeeBreakdown;

namespace ERP_PAYROLLMOBILE.Services.Features.Payroll;

public sealed class EmployeePayrollStateService
{
    public DateOnly SelectedMonth { get; set; } = new(DateTime.Today.Year, DateTime.Today.Month, 1);
    public PayrollBreakdownModel? Breakdown { get; set; }
    public ERPATT_CongphepduModel? LeaveBalance { get; set; }
    public EmployeeShiftRegistrationSummaryDto? ShiftSummary { get; set; }
    public EmployeeShiftRegistrationDetailResponseDto? ShiftDetail { get; set; }
    public List<ERPATT_CongphepduModel> LeaveHistory { get; set; } = new();
    public List<SpecialWorkClaimDetailModel> SpecialWorkClaims { get; set; } = new();
    public string? ErrorMessage { get; set; }
    public bool IsLoading { get; set; }
    public bool IsSyncing { get; set; }
    public bool ShowLeaveHistory { get; set; }
    public bool ShowShiftDetail { get; set; }
    public bool IsInitialized { get; set; }
}