namespace ERP_PAYROLLMOBILE.Models;

public class DTOGetDialy
{
    public Guid CompanyId { get; set; }
    public string? EmployeeId { get; set; } = string.Empty;
    public DateOnly WorkDate { get; set; }
}
