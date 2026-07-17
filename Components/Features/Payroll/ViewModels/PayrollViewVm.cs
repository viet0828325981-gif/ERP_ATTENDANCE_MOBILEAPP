using ERP_PAYROLLMOBILE.Models.EmployeeBreakdown;

namespace ERP_PAYROLLMOBILE.Components.Features.Payroll.ViewModels;

public sealed class PayrollViewVm
{
    public PayrollBreakdownModel? Breakdown { get; set; }
    public ERPATT_CongphepduModel? LeaveBalance { get; set; }
    public List<ERPATT_CongphepduModel> LeaveHistory { get; set; } = new();
    public List<SpecialWorkClaimDetailModel> SpecialWorkClaims { get; set; } = new();
    public EmployeeShiftRegistrationSummaryDto? ShiftSummary { get; set; }
    public EmployeeShiftRegistrationDetailResponseDto? ShiftDetail { get; set; }
    public bool IsLoading { get; set; }
    public string? ErrorMessage { get; set; }
}
