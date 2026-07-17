using ERP_PAYROLLMOBILE.Models.EmployeeRequest;

namespace ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerRequests;

public sealed class ManagerRequestTypeModel
{
    public string RequestType { get; set; } = string.Empty;
    public string RequestName { get; set; } = string.Empty;
}

public sealed class ManagerApproveWorkRequestDto
{
    public Guid RequestId { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    public DateOnly TargetDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}

public sealed class ManagerRejectWorkRequestDto
{
    public Guid RequestId { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string RejectedBy { get; set; } = string.Empty;
}

public sealed class ManagerMakeupWorkRequestDto
{
    public string EmployeeId { get; set; } = string.Empty;
    public DateOnly TargetDate { get; set; }
    public DateOnly SourceDate { get; set; }
    public int MinutesAffected { get; set; }
    public string? Reason { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public sealed class ManagerOnlineWorkRequestDto
{
    public string EmployeeId { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    public string ShiftCode { get; set; } = string.Empty;
    public List<DateOnly> OnlineDates { get; set; } = new();
    public string? Reason { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public sealed class ManagerBulkWorkRequestDto
{
    public List<string> EmployeeIds { get; set; } = new();
    public string RequestType { get; set; } = string.Empty;
    public DateOnly TargetDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public sealed class ManagerMonthlyNightlyJobDto
{
    public int Year { get; set; }
    public int Month { get; set; }
}

public sealed class BulkWorkRequestResult
{
    public List<BulkWorkRequestItem> Created { get; set; } = new();
    public List<BulkWorkRequestItem> Overridden { get; set; } = new();
    public List<BulkWorkRequestItem> Skipped { get; set; } = new();
}

public sealed class BulkWorkRequestItem
{
    public string EmployeeId { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string Message { get; set; } = string.Empty;
}

public sealed class ManagerMonthlyNightlyJobResult
{
    public int InsertedCount { get; set; }
}
