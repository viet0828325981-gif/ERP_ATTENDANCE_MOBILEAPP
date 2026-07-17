namespace ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerDepartments;

public sealed class ManagerDepartmentModel
{
    public Guid CompanyId { get; set; }
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int Order { get; set; }
}
