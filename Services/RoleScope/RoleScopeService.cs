using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerDepartments;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace ERP_PAYROLLMOBILE.Services.RoleScope;

public sealed class RoleScopeService : IRoleScopeService
{
    private readonly AuthenticationStateProvider authenticationStateProvider;

    public RoleScopeService(AuthenticationStateProvider authenticationStateProvider)
    {
        this.authenticationStateProvider = authenticationStateProvider;
        this.authenticationStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
    }

    public RoleScopeState Current { get; } = new();

    public event Action? Changed;

    public async Task<RoleScopeState> LoadAsync(CancellationToken cancellationToken = default)
    {
        if (Current.IsLoaded)
        {
            return Current;
        }

        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        ApplyUser(authState.User);
        return Current;
    }

    public void SetDepartments(List<ManagerDepartmentModel> departments)
    {
        Current.Departments = departments;
        if (Current.DepartmentId.HasValue && departments.All(item => item.DepartmentId != Current.DepartmentId.Value))
        {
            Current.DepartmentId = null;
        }

        if (!Current.DepartmentId.HasValue && departments.Count > 0)
        {
            Current.DepartmentId = departments[0].DepartmentId;
        }

        Changed?.Invoke();
    }

    public void SetScope(Guid? departmentId, string? selectedEmployeeId)
    {
        Current.DepartmentId = departmentId;
        Current.SelectedEmployeeId = selectedEmployeeId;
        Changed?.Invoke();
    }

    private void OnAuthenticationStateChanged(Task<AuthenticationState> authenticationStateTask)
    {
        _ = UpdateFromAuthenticationAsync(authenticationStateTask);
    }

    private async Task UpdateFromAuthenticationAsync(Task<AuthenticationState> authenticationStateTask)
    {
        var authState = await authenticationStateTask;
        ApplyUser(authState.User);
        Changed?.Invoke();
    }

    private void ApplyUser(ClaimsPrincipal user)
    {
        if (Guid.TryParse(user.FindFirst("company_id")?.Value, out var companyId))
        {
            Current.CompanyId = companyId;
        }

        Current.EmployeeId = user.FindFirst("employee_id")?.Value ?? string.Empty;
        Current.Role = ResolveRole(user);
        Current.IsLoaded = true;

        if (!Current.CanUseManagerScope)
        {
            Current.DepartmentId = null;
            Current.SelectedEmployeeId = null;
            Current.Departments = new List<ManagerDepartmentModel>();
        }
    }

    private static AppRole ResolveRole(ClaimsPrincipal user)
    {
        var roles = user.FindAll(ClaimTypes.Role)
            .Concat(user.FindAll("role"))
            .Concat(user.FindAll("roles"))
            .Select(claim => claim.Value)
            .ToList();

        if (roles.Any(role => string.Equals(role, "Quản trị", StringComparison.OrdinalIgnoreCase)
            || string.Equals(role, "Administrator", StringComparison.OrdinalIgnoreCase)
            || string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase)))
        {
            return AppRole.Admin;
        }

        if (roles.Any(role => string.Equals(role, "Manager", StringComparison.OrdinalIgnoreCase)
            || string.Equals(role, "Quản lý", StringComparison.OrdinalIgnoreCase)))
        {
            return AppRole.Manager;
        }

        return AppRole.Employee;
    }
}
