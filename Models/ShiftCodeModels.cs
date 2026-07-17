namespace ERP_PAYROLLMOBILE.Models;

public class ShiftRegistrationDayModel
{
    public DateOnly WorkDate { get; set; }
    public string ShiftCode { get; set; } = "";
    public string RegisteredShiftCode { get; set; } = "";
    public bool HasShiftRegistration { get; set; }
    public bool IsSplitShift { get; set; }
    public TimeSpan? MorningEnd { get; set; }
    public TimeSpan? EveningStart { get; set; }

    public string EffectiveShiftCode => string.IsNullOrWhiteSpace(RegisteredShiftCode) ? ShiftCode : RegisteredShiftCode;
}
