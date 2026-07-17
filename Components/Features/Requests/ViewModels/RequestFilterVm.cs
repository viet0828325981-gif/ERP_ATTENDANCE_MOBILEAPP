namespace ERP_PAYROLLMOBILE.Components.Features.Requests.ViewModels;

public sealed class RequestFilterVm
{
    public DateOnly SelectedMonth { get; set; } = new(DateTime.Today.Year, DateTime.Today.Month, 1);
    public string? RequestType { get; set; }
    public int? Status { get; set; }
    public Guid? DepartmentId { get; set; }
    public string? EmployeeId { get; set; }
}
