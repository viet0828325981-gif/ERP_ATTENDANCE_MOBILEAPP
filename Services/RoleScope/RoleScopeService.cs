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

    private void ApplyUser(System.Security.Claims.ClaimsPrincipal user)
    {
        if (Guid.TryParse(user.FindFirst("company_id")?.Value, out var companyId))
        {
            Current.CompanyId = companyId;
        }

        Current.EmployeeId = user.FindFirst("employee_id")?.Value ?? string.Empty;
        Current.Role = AppRole.Employee;
        Current.IsLoaded = true;
    }
}
