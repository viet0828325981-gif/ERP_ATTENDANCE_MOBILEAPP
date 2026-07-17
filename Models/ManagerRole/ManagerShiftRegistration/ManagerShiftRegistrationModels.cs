using ERP_PAYROLLMOBILE.Models.EmployeeRequest;

namespace ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerShiftRegistration;

public sealed class ManagerApproveShiftRegistrationDto
{
    public Guid RequestId { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}

public sealed class ManagerApproveAllShiftRegistrationDto
{
    public Guid? DepartmentId { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}

public sealed class ManagerRejectShiftRegistrationDto
{
    public Guid RequestId { get; set; }
    public string RejectedBy { get; set; } = string.Empty;
}

public sealed class ShiftRegistrationDayDetailModel
{
    public Guid RequestId { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public DateOnly WorkDate { get; set; }
    public string ShiftCode { get; set; } = string.Empty;
    public string ShiftName { get; set; } = string.Empty;
    public decimal WorkHours { get; set; }
    public TimeSpan? MorningEnd { get; set; }
    public TimeSpan? EveningStart { get; set; }
    public bool IsSplitShift { get; set; }
}

public sealed class ShiftEditChangeDetailModel
{
    public Guid RequestId { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public DateOnly WorkDate { get; set; }
    public string? OldShiftCode { get; set; }
    public string? NewShiftCode { get; set; }
    public string? OldShiftName { get; set; }
    public string? NewShiftName { get; set; }
}

public sealed class ManagerApproveAllShiftRegistrationResult
{
    public int ApprovedCount { get; set; }
}
