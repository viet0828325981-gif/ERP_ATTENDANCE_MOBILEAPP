namespace ERP_PAYROLLMOBILE.Models;

/// <summary>
/// Model cho dbo.AttendanceDailyView (§6.4).
/// = ERPATT_Daily JOIN ERPATT_WorkClaim (Active).
/// Downstream (Timesheet, Payroll, ERPATT_Congphepdu) dùng model này.
/// </summary>
public class AttendanceDailyViewModel
{
    public Guid CompanyId { get; set; }
    public string EmployeeId { get; set; } = "";
    public string FullName { get; set; } = "";
    public Guid? DepartmentId { get; set; }
    public DateTime WorkDate { get; set; }
    public string AttendanceCode { get; set; } = "";
    public string? ShiftCode { get; set; }
    public decimal WorkDayFactor { get; set; }
    public decimal? ResolvedWorkDayFactor { get; set; }
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public int? WorkMinutes { get; set; }
    public int? LateMinutes { get; set; }
    public int? EarlyLeaveMinutes { get; set; }
    public string? WorkType { get; set; }
    public int OvertimeMinutes { get; set; }
    public decimal OvertimeFactor { get; set; }
    public int MakeupMinutes { get; set; }
    public decimal MakeupFactor { get; set; }
    public int ForgotMinutes { get; set; }
    public decimal ForgotFactor { get; set; }
    public decimal SpecialDayFactor { get; set; }
    public decimal HolidayDayFactor { get; set; }
    public decimal WeekendDayFactor { get; set; }
    public bool HasPendingRequest { get; set; }
    public string PendingRequestTypes { get; set; } = "";
    public bool HasApprovedRequest { get; set; }
    public string ApprovedRequestTypes { get; set; } = "";
    public bool HasActiveClaim { get; set; }
    public string ClaimRequestTypes { get; set; } = "";
    public bool HasShiftRegistration { get; set; }
    public string RegisteredShiftCode { get; set; } = "";
    public bool IsSplitShift { get; set; }
    public TimeSpan? MorningEnd { get; set; }
    public TimeSpan? EveningStart { get; set; }
}
