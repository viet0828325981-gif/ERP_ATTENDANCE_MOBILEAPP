namespace ERP_PAYROLLMOBILE.Models;

/// <summary>
/// Các lần chấm công thô trong ngày theo nhân viên.
/// Dùng để hiển thị popup chi tiết lịch sử quẹt thẻ/máy chấm công.
/// </summary>
public class AttendanceLogDetailModel
{
    public string EmployeeId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime TimeCheck { get; set; }
}
