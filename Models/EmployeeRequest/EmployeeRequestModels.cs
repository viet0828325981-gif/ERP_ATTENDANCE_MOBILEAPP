namespace ERP_PAYROLLMOBILE.Models.EmployeeRequest;

public sealed class EmployeeRequestTypeDto
{
    public string RequestType { get; set; } = string.Empty;
    public string RequestName { get; set; } = string.Empty;
}

public sealed class EmployeeRequestQueryDto
{
    public Guid CompanyId { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Month { get; set; }
    public string? RequestType { get; set; }
    public bool GroupOnline { get; set; } = true;
}

public sealed class WorkRequestModel
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    public DateOnly TargetDate { get; set; }
    public DateOnly? SourceDate { get; set; }
    public int MinutesAffected { get; set; }
    public string? WorkType { get; set; }
    public string? ShiftCode { get; set; }
    public string? Reason { get; set; }
    public string? CreatedBy { get; set; }
    public int Status { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? ClaimInsertedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? FullName { get; set; }
    public string? DepartmentId { get; set; }
    public int ConfirmedMinutes { get; set; }
    public bool IsGroupedOnline { get; set; }
    public int GroupTotalDays { get; set; }
    public DateOnly? GroupFromDate { get; set; }
    public DateOnly? GroupToDate { get; set; }
    public int GroupPendingCount { get; set; }
    public int GroupApprovedCount { get; set; }
    public int GroupRejectedCount { get; set; }
}

public sealed class EmployeeRequestDailyDetailQueryDto
{
    public Guid CompanyId { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public DateOnly WorkDate { get; set; }
}

public sealed class EmployeeRequestOnlineGroupDetailQueryDto
{
    public Guid CompanyId { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public sealed class EmployeeRequestCalendarQueryDto
{
    public int Year { get; set; }
    public int Month { get; set; }
}

public sealed class EmployeeCancelRequestDto
{
    public Guid RequestId { get; set; }
    public Guid CompanyId { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
}

public sealed class EmployeeForgotCheckRequestDto
{
    public string EmployeeId { get; set; } = string.Empty;
    public DateOnly TargetDate { get; set; }
    public string? Reason { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public sealed class EmployeeMakeupHourRequestDto
{
    public string EmployeeId { get; set; } = string.Empty;
    public DateOnly TargetDate { get; set; }
    public DateOnly SourceDate { get; set; }
    public int MinutesAffected { get; set; }
    public string? Reason { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public sealed class EmployeeOvertimeRequestDto
{
    public string EmployeeId { get; set; } = string.Empty;
    public DateOnly SourceDate { get; set; }
    public int MinutesAffected { get; set; }
    public string? Reason { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public sealed class EmployeeSpecialDayRequestDto
{
    public string EmployeeId { get; set; } = string.Empty;
    public DateOnly SourceDate { get; set; }
    public string? Reason { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public sealed class EmployeeOnlineRequestDto
{
    public string EmployeeId { get; set; } = string.Empty;
    public List<DateOnly> OnlineDates { get; set; } = new();
    public string ShiftCode { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public sealed class EmployeeCancelOnlineGroupRequestDto
{
    public string EmployeeId { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public sealed class ShiftModel
{
    public string ShiftCode { get; set; } = string.Empty;
    public string ShiftName { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int StandardWorkMinutes { get; set; }
    public bool IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public sealed class DimCalendarModel
{
    public DateOnly WorkDate { get; set; }
    public int DayOfWeek { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal PaidLeaveDays { get; set; }

}

public class ForgotCheckSuggestionModel
{
    public string EmployeeId { get; set; } = "";
    public DateOnly WorkDate  { get; set; }
    public int MissingMinutes { get; set; }
    public List<ForgotCheckSuggestionItemModel> Items { get; set; } = new();
}

public sealed class ForgotCheckSuggestionItemModel
{
    public DateOnly Date { get; set; }
    public int MissingMinutes { get; set; }
    public string? Note { get; set; }
}

