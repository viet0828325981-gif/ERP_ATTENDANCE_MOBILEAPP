using ERP_PAYROLLMOBILE.Models.EmployeeRequest;

namespace ERP_PAYROLLMOBILE.Components.Features.Requests.ViewModels;

public sealed class RequestViewVm
{
    public List<WorkRequestModel> Requests { get; set; } = new();
    public List<ShiftModel> OnlineShifts { get; set; } = new();
    public bool IsLoading { get; set; }
    public string? ErrorMessage { get; set; }
}
