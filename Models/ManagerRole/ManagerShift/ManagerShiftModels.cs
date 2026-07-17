namespace ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerShift;

public sealed class ManagerRegisterWeekShiftDto
{
    public List<string> EmployeeIds { get; set; } = new();
    public DateOnly WeekStartDate { get; set; }
    public string? MonShiftCode { get; set; }
    public string? TueShiftCode { get; set; }
    public string? WedShiftCode { get; set; }
    public string? ThuShiftCode { get; set; }
    public string? FriShiftCode { get; set; }
    public string? SatShiftCode { get; set; }
    public string? SunShiftCode { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? Note { get; set; }
    public bool IsSplitShift { get; set; }
    public TimeSpan? MorningEnd { get; set; }
    public TimeSpan? EveningStart { get; set; }
}

public sealed class ManagerRegisterMonthShiftDto
{
    public List<string> EmployeeIds { get; set; } = new();
    public int Year { get; set; }
    public int Month { get; set; }
    public string? ShiftCode { get; set; }
    public bool IncludeSunday { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? Note { get; set; }
    public bool IsSplitShift { get; set; }
    public TimeSpan? MorningEnd { get; set; }
    public TimeSpan? EveningStart { get; set; }
}

public sealed class EmployeeShiftSummaryModel
{
    public string EmployeeId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public Guid? DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public int RegisteredShiftCount { get; set; }
    public decimal EstimatedShiftCount { get; set; }
    public decimal EstimatedWorkHours { get; set; }
    public Dictionary<DateOnly, string> ShiftDays { get; set; } = new();
    public List<EmployeeShiftDayDetailModel> ShiftDayDetails { get; set; } = new();
}

public sealed class EmployeeShiftDayDetailModel
{
    public DateOnly Date { get; set; }
    public string ShiftCode { get; set; } = string.Empty;
    public string ShiftName { get; set; } = string.Empty;
    public decimal WorkHours { get; set; }
}

public sealed class ManagerShiftEmployeeOption
{
    public string EmployeeId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public Guid? DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
}

public sealed class WeeklyShiftScheduleRow
{
    public string EmployeeName { get; set; } = string.Empty;
    public int? Salary { get; set; }
    public DateOnly WeekStartDate { get; set; }
    public DateOnly WeekEndDate { get; set; }
    public string? MonShiftCode { get; set; }
    public string? TueShiftCode { get; set; }
    public string? WedShiftCode { get; set; }
    public string? ThuShiftCode { get; set; }
    public string? FriShiftCode { get; set; }
    public string? SatShiftCode { get; set; }
    public string? SunShiftCode { get; set; }
}

public sealed class EmployeeDetailShiftCodeInMonthModel
{
    public string EmployeeId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public string ShiftCode { get; set; } = string.Empty;
    public string DayOfWeek { get; set; } = string.Empty;
    public string ShiftName { get; set; } = string.Empty;
    public decimal WorkHours { get; set; }
}
