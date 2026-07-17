namespace ERP_PAYROLLMOBILE.Components.Features.Shift.ViewModels;

public sealed class ShiftFilterVm
{
    public DateOnly SelectedMonth { get; set; } = new(DateTime.Today.Year, DateTime.Today.Month, 1);
    public DateOnly WeekStartDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public Guid? DepartmentId { get; set; }
    public string? EmployeeId { get; set; }
    public int? Status { get; set; }
}
