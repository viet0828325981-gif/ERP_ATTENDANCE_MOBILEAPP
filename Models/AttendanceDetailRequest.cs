namespace ERP_PAYROLLMOBILE.Models;

public sealed record AttendanceDetailRequest(
    Guid CompanyId,
    string EmployeeId,
    DateOnly WorkDate);
