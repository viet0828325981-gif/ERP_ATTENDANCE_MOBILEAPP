namespace ERP_PAYROLLMOBILE.Models;

public sealed record AttendanceMonthlyRequest(
    Guid CompanyId,
    string EmployeeId,
    DateOnly From,
    DateOnly To);
