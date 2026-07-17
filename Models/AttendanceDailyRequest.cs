namespace ERP_PAYROLLMOBILE.Models;

public sealed record AttendanceDailyRequest(
    Guid CompanyId,
    string EmployeeId,
    DateOnly WorkDate);
