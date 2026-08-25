namespace ERP_PAYROLLMOBILE.Models;

public class ShiftRegistrationCreateModel
{
    public string EmployeeId { get; set; } = string.Empty;
    public IReadOnlyList<ShiftRegistrationCreateItemModel> Days { get; set; } = Array.Empty<ShiftRegistrationCreateItemModel>();
    public string? CreatedBy { get; set; }
    public string? Note { get; set; }
    public TimeSpan? MorningEnd { get; set; }
    public TimeSpan? EveningStart { get; set; }
}

public class ShiftRegistrationCreateItemModel
{
    public DateOnly WorkDate { get; set; }
    public string ShiftCode { get; set; } = string.Empty;
}
