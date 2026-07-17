namespace ERP_PAYROLLMOBILE.Components.Features.Daily.ViewModels;

public sealed class DailyFilterVm
{
    public DateOnly SelectedDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DateOnly SelectedMonth { get; set; } = new(DateTime.Today.Year, DateTime.Today.Month, 1);
    public Guid? DepartmentId { get; set; }
    public string? EmployeeId { get; set; }
}
