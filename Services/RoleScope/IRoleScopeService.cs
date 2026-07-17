using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerDepartments;
using System.Security.Claims;

namespace ERP_PAYROLLMOBILE.Services.RoleScope;

public interface IRoleScopeService
{
    RoleScopeState Current { get; }
    event Action? Changed;
    Task<RoleScopeState> LoadAsync(CancellationToken cancellationToken = default);
    void SetDepartments(List<ManagerDepartmentModel> departments);
    void SetScope(Guid? departmentId, string? selectedEmployeeId);
}
