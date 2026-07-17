using ERP_PAYROLLMOBILE.Models;
using ERP_PAYROLLMOBILE.Models.ManagerDaily;

namespace ERP_PAYROLLMOBILE.Components.Features.Daily.ViewModels;

public sealed class DailyViewVm
{
    public List<AttendanceDailyViewModel> EmployeeRecords { get; set; } = new();
    public List<DepartmentAttendanceDailyApiModel> DepartmentRecords { get; set; } = new();
    public bool IsLoading { get; set; }
    public string? ErrorMessage { get; set; }
}
