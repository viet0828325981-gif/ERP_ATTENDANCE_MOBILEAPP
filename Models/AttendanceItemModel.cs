namespace ERP_PAYROLLMOBILE.Models;

public sealed record AttendanceItemModel(
    DateOnly WorkDate,
    string Date,
    string Shift,
    string CheckInOut,
    string WorkMinutes,
    string TotalHours,
    string LateEarly,
    string Code,
    string AdjustedWork,
    string Status,
    bool IsApproved,
    decimal WorkDayFactor,
    bool IsMissingCheck);
