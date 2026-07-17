using ERP_PAYROLLMOBILE.Models.EmployeeRequest;

namespace ERP_PAYROLLMOBILE.Services.Features.Requests;

public sealed class EmployeeRequestStateService
{
    public DateOnly SelectedMonth { get; set; } = new(DateTime.Today.Year, DateTime.Today.Month, 1);
    public string SelectedRequestType { get; set; } = string.Empty;
    public List<EmployeeRequestTypeDto> RequestTypes { get; set; } = new();
    public List<WorkRequestModel> Requests { get; set; } = new();
    public List<ShiftModel> OnlineShifts { get; set; } = new();
    public List<DimCalendarModel> CalendarDays { get; set; } = new();
    public List<WorkRequestModel> GroupRequests { get; set; } = new();
    public string? ErrorMessage { get; set; }
    public string? GroupDetailErrorMessage { get; set; }
    public string GroupDetailTitle { get; set; } = string.Empty;
    public bool IsLoading { get; set; }
    public bool IsGroupLoading { get; set; }
    public bool IsSubmittingCreateRequest { get; set; }
    public bool IsCancelling { get; set; }
    public bool ShowGroupDetailModal { get; set; }
    public bool ShowCreateModal { get; set; }
    public Guid? CancellingRequestId { get; set; }
    public Guid? ExpandedRequestId { get; set; }
    public string CreateRequestType { get; set; } = string.Empty;
    public DateOnly FormTargetDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DateOnly FormSourceDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public int FormMinutesAffected { get; set; } = 60;
    public string FormShiftCode { get; set; } = string.Empty;
    public string? FormReason { get; set; }
    public HashSet<DateOnly> SelectedOnlineDates { get; set; } = new();
    public bool IsInitialized { get; set; }
}