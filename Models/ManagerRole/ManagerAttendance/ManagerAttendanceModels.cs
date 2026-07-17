using ERP_PAYROLLMOBILE.Models.ManagerDaily;

namespace ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerAttendance;

public sealed class ManagerAttendanceDailySyncDto
{
    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
}

public sealed class AttendanceMonthlyRecordModel
{
    public string EmployeeId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public Guid? DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public Dictionary<DateOnly, string> DailyRecords { get; set; } = new();
}
