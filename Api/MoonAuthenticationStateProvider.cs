using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace ERP_PAYROLLMOBILE.Api;

public sealed class MoonAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());
    private readonly SemaphoreSlim syncRoot = new(1, 1);
    private AuthenticationState? cachedState;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (cachedState is not null)
        {
            return cachedState;
        }

        await syncRoot.WaitAsync();
        try
        {
            if (cachedState is null)
            {
                var principal = await GetCurrentPrincipalAsync();
                cachedState = new AuthenticationState(principal);
            }

            return cachedState;
        }
        finally
        {
            syncRoot.Release();
        }
    }

    public async Task SignInAsync(string userId, string employeeId, Guid companyId, string jwt, string? role, bool rememberMe)
    {
        var expiresUtc = DateTimeOffset.UtcNow.Add(rememberMe ? TimeSpan.FromDays(7) : TimeSpan.FromHours(8));

        await SecureStorage.Default.SetAsync(IdentityStorageKeys.UserIdKey, userId);
        await SecureStorage.Default.SetAsync(IdentityStorageKeys.EmployeeIdKey, employeeId);
        await SecureStorage.Default.SetAsync(IdentityStorageKeys.CompanyIdKey, companyId.ToString());
        await SecureStorage.Default.SetAsync(IdentityStorageKeys.RoleKey, role ?? string.Empty);
        await SecureStorage.Default.SetAsync(IdentityStorageKeys.JwtKey, jwt);
        await SecureStorage.Default.SetAsync(IdentityStorageKeys.ExpiresUtcKey, expiresUtc.ToString("O", CultureInfo.InvariantCulture));

        var state = new AuthenticationState(CreatePrincipal(userId, employeeId, companyId, jwt, role));
        cachedState = state;
        NotifyAuthenticationStateChanged(Task.FromResult(state));
    }

    public void SignOut()
    {
        SecureStorage.Default.Remove(IdentityStorageKeys.UserIdKey);
        SecureStorage.Default.Remove(IdentityStorageKeys.EmployeeIdKey);
        SecureStorage.Default.Remove(IdentityStorageKeys.CompanyIdKey);
        SecureStorage.Default.Remove(IdentityStorageKeys.RoleKey);
        SecureStorage.Default.Remove(IdentityStorageKeys.JwtKey);
        SecureStorage.Default.Remove(IdentityStorageKeys.ExpiresUtcKey);

        var state = new AuthenticationState(Anonymous);
        cachedState = state;
        NotifyAuthenticationStateChanged(Task.FromResult(state));
    }

    private static async Task<ClaimsPrincipal> GetCurrentPrincipalAsync()
    {
        var userId = await SecureStorage.Default.GetAsync(IdentityStorageKeys.UserIdKey);
        var employeeId = await SecureStorage.Default.GetAsync(IdentityStorageKeys.EmployeeIdKey);
        var companyIdText = await SecureStorage.Default.GetAsync(IdentityStorageKeys.CompanyIdKey);
        var role = await SecureStorage.Default.GetAsync(IdentityStorageKeys.RoleKey);
        var jwt = await SecureStorage.Default.GetAsync(IdentityStorageKeys.JwtKey);
        var expiresUtcText = await SecureStorage.Default.GetAsync(IdentityStorageKeys.ExpiresUtcKey);

        if (string.IsNullOrWhiteSpace(userId)
            || string.IsNullOrWhiteSpace(employeeId)
            || string.IsNullOrWhiteSpace(jwt)
            || !Guid.TryParse(companyIdText, out var companyId)
            || IsExpired(expiresUtcText))
        {
            SecureStorage.Default.Remove(IdentityStorageKeys.UserIdKey);
            SecureStorage.Default.Remove(IdentityStorageKeys.EmployeeIdKey);
            SecureStorage.Default.Remove(IdentityStorageKeys.CompanyIdKey);
            SecureStorage.Default.Remove(IdentityStorageKeys.RoleKey);
            SecureStorage.Default.Remove(IdentityStorageKeys.JwtKey);
            SecureStorage.Default.Remove(IdentityStorageKeys.ExpiresUtcKey);
            return Anonymous;
        }

        return CreatePrincipal(userId, employeeId, companyId, jwt, role);
    }

    private static bool IsExpired(string? expiresUtcText)
    {
        return !DateTimeOffset.TryParse(expiresUtcText, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var expiresUtc)
            || expiresUtc <= DateTimeOffset.UtcNow;
    }

    private static ClaimsPrincipal CreatePrincipal(string userId, string employeeId, Guid companyId, string jwt, string? role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userId),
            new Claim("login_user_id", userId),
            new Claim("employee_id", employeeId),
            new Claim("company_id", companyId.ToString()),
            new Claim("access_token", jwt)
        };

        if (!string.IsNullOrWhiteSpace(role))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            claims.Add(new Claim("role", role));
        }

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "MoonIdentity"));
    }
}
