namespace ERP_PAYROLLMOBILE.Models.EmployeeBreakdown;

public sealed class EmployeeBreakdownSyncDto
{
    public string EmployeeId { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Month { get; set; }
}

public sealed class PayrollBreakdownModel
{
    public Guid CompanyId { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal StandardDays { get; set; }
    public decimal ActualWorkDays { get; set; }
    public decimal RegularDays { get; set; }
    public decimal OvertimeDays { get; set; }
    public decimal WeekendDays { get; set; }
    public decimal HolidayDays { get; set; }
    public decimal MakeupDays { get; set; }
    public decimal WeightedWorkDays { get; set; }
    public decimal PaidLeaveDays { get; set; }
    public decimal EventPaidLeaveDays { get; set; }
    public decimal LeaveBalanceOpen { get; set; }
    public decimal LeaveEarned { get; set; }
    public decimal LeaveUsed { get; set; }
    public decimal LeaveBalance { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal OriginalBasicSalary { get; set; }
}

public sealed class ERPATT_CongphepduModel
{
    public string DepartmentId { get; set; } = string.Empty;
    public string? PhongBan { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TonDauThang { get; set; }
    public decimal SuDung { get; set; }
    public decimal TonTrongThang { get; set; }
    public decimal ConDu { get; set; }
}

public sealed class SpecialWorkClaimDetailModel
{
    public string LoaiCong { get; set; } = string.Empty;
    public decimal WorkDayFactor { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public DateTime SourceDate { get; set; }
}

public sealed class EmployeeShiftRegistrationSummaryDto
{
    public int FullTimeShiftCount { get; set; }
    public int PartTimeShiftCount { get; set; }
    public int TotalRegisteredShiftCount { get; set; }
    public int StandardShiftCapacity { get; set; }
    public int RemainingShiftCapacity { get; set; }
}

public sealed class EmployeeShiftRegistrationBreakdownDetailDto
{
    public DateOnly WorkDate { get; set; }
    public string ShiftCode { get; set; } = string.Empty;
}

public sealed class EmployeeShiftRegistrationDetailResponseDto
{
    public List<EmployeeShiftRegistrationBreakdownDetailDto> FullTimeItems { get; set; } = new();
    public List<EmployeeShiftRegistrationBreakdownDetailDto> PartTimeItems { get; set; } = new();
}

public sealed class EmployeeShiftRegistrationBreakdownDto
{
    public int FullTimeShiftCount { get; set; }
    public int PartTimeShiftCount { get; set; }
    public int TotalRegisteredShiftCount { get; set; }
    public int StandardShiftCapacity { get; set; }
    public int RemainingShiftCapacity { get; set; }
    public List<EmployeeShiftRegistrationBreakdownDetailDto> FullTimeItems { get; set; } = new();
    public List<EmployeeShiftRegistrationBreakdownDetailDto> PartTimeItems { get; set; } = new();
}
