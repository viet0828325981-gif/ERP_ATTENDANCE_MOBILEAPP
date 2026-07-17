using ERP_PAYROLLMOBILE.Models;
using ERP_PAYROLLMOBILE.Models.EmployeeRequest;

namespace ERP_PAYROLLMOBILE.Services.Features.Daily;

public sealed class EmployeeDailyStateService
{
    public EmployeeDailyViewMode SearchMode { get; set; } = EmployeeDailyViewMode.Month;
    public DateOnly SelectedDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DateOnly SelectedMonth { get; set; } = new(DateTime.Today.Year, DateTime.Today.Month, 1);
    public List<EmployeeDailyAttendanceRecord> CurrentRecords { get; set; } = new();
    public Dictionary<DateOnly, List<AttendanceLogDetailModel>> DailyLogsByDate { get; } = new();
    public Dictionary<DateOnly, string> DailyLogErrorsByDate { get; } = new();
    public HashSet<DateOnly> LoadingLogDates { get; } = new();
    public HashSet<string> OpenSuggestionIds { get; } = new(StringComparer.OrdinalIgnoreCase);
    public bool IsInitialized { get; set; }
    public bool IsLoading { get; set; }
    public bool ShowSearchPanel { get; set; }
    public string? ErrorMessage { get; set; }
    public DateOnly? ExpandedRecordDate { get; set; }
    public DateOnly? PendingForgotCheckDate { get; set; }
    public DateOnly? SubmittingForgotCheckDate { get; set; }
    public ForgotCheckSuggestionModel? ForgotCheckSuggestion { get; set; }
}

public enum EmployeeDailyViewMode
{
    Day,
    Month
}

public sealed record EmployeeDailyAttendanceRecord(
    DateOnly WorkDate,
    string Date,
    string Shift,
    string Time,
    string WorkDayText,
    string LateEarlyText,
    string Status,
    bool IsOnTime,
    string Note,
    decimal WorkDayFactor,
    int LateMinutes,
    int EarlyLeaveMinutes,
    bool IsAbsent,
    bool HasCheckIn,
    bool HasCheckOut,
    int ForgotMinutes);