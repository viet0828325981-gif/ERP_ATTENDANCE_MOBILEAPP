namespace ERP_PAYROLLMOBILE.Services.RoleScope;

public sealed class RoleScopeState
{
    public Guid CompanyId { get; set; } = Guid.Parse("A2F3FB40-7FA2-F011-9DB7-208810C238D7");
    public AppRole Role { get; set; } = AppRole.Employee;
    public bool IsLoaded { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
}
