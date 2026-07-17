namespace ERP_PAYROLLMOBILE.Components.Features.Payroll.ViewModels;

public sealed class PayrollFilterVm
{
    public DateOnly SelectedMonth { get; set; } = new(DateTime.Today.Year, DateTime.Today.Month, 1);
    public string? EmployeeId { get; set; }
    public Guid? DepartmentId { get; set; }
}
