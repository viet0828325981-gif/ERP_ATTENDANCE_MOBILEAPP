namespace ERP_PAYROLLMOBILE.Models;

public sealed record AttendanceDetailItemModel(
    string EmployeeId,
    string FullName,
    DateTime TimeCheck);
