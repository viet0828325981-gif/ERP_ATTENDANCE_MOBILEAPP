namespace ERP_PAYROLLMOBILE.Services.RoleScope;

public interface IRoleScopeService
{
    RoleScopeState Current { get; }
    event Action? Changed;
    Task<RoleScopeState> LoadAsync(CancellationToken cancellationToken = default);
}
