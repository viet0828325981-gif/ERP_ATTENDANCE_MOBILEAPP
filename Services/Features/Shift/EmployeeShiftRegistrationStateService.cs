using ERP_PAYROLLMOBILE.Models;

namespace ERP_PAYROLLMOBILE.Services.Features.Shift;

public sealed class EmployeeShiftRegistrationStateService
{
    public DateOnly SelectedMonth { get; set; } = new(DateTime.Today.Year, DateTime.Today.Month, 1);
    public List<ShiftRegistrationDayModel> ShiftDays { get; set; } = new();
    public ShiftRegistrationOverviewModel Overview { get; set; } = new();
    public string EmployeeId { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
    public bool IsLoading { get; set; }
    public bool IsShiftDetailsOpen { get; set; }
    public int? SelectedDay { get; set; }
    public bool IsInitialized { get; set; }
}