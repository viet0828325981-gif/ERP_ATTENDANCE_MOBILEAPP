using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerShift;
using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerShiftRegistration;

namespace ERP_PAYROLLMOBILE.Components.Features.Shift.ViewModels;

public sealed class ShiftViewVm
{
    public List<EmployeeShiftSummaryModel> EmployeeSummaries { get; set; } = new();
    public List<EmployeeDetailShiftCodeInMonthModel> EmployeeDayDetails { get; set; } = new();
    public List<WeeklyShiftScheduleRow> WeeklySchedule { get; set; } = new();
    public List<ShiftRegistrationDayDetailModel> RegistrationDays { get; set; } = new();
    public bool IsLoading { get; set; }
    public string? ErrorMessage { get; set; }
}
