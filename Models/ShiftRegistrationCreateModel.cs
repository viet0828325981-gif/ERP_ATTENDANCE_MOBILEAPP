namespace ERP_PAYROLLMOBILE.Models;

public class ShiftRegistrationCreateModel
{
    public string EmployeeId { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Month { get; set; }
    public string? ShiftCode { get; set; }
    public DateOnly WeekStartDate { get; set; }
    public string? MonShiftCode { get; set; }
    public string? TueShiftCode { get; set; }
    public string? WedShiftCode { get; set; }
    public string? ThuShiftCode { get; set; }
    public string? FriShiftCode { get; set; }
    public string? SatShiftCode { get; set; }
    public string? SunShiftCode { get; set; }
    public string? CreatedBy { get; set; }
    public string CreatedByRole { get; set; } = "employee";
    public string? Note { get; set; }
    public bool IsSplitShift { get; set; }
    public TimeSpan? MorningEnd { get; set; }
    public TimeSpan? EveningStart { get; set; }
}
