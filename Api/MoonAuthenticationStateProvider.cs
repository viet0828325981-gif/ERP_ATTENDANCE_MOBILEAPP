using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace ERP_PAYROLLMOBILE.Api;

public sealed class MoonAuthenticationStateProvider : AuthenticationStateProvider
{
    private const string UserIdKey = "moon_identity_user_id";
    private const string EmployeeIdKey = "moon_identity_employee_id";
    private const string CompanyIdKey = "moon_identity_company_id";
    private const string RoleKey = "moon_identity_role";
    private const string JwtKey = "moon_identity_access_token";
    private const string ExpiresUtcKey = "moon_identity_expires_utc";
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

        await SecureStorage.Default.SetAsync(UserIdKey, userId);
        await SecureStorage.Default.SetAsync(EmployeeIdKey, employeeId);
        await SecureStorage.Default.SetAsync(CompanyIdKey, companyId.ToString());
        await SecureStorage.Default.SetAsync(RoleKey, role ?? string.Empty);
        await SecureStorage.Default.SetAsync(JwtKey, jwt);
        await SecureStorage.Default.SetAsync(ExpiresUtcKey, expiresUtc.ToString("O", CultureInfo.InvariantCulture));

        var state = new AuthenticationState(CreatePrincipal(userId, employeeId, companyId, jwt, role));
        cachedState = state;
        NotifyAuthenticationStateChanged(Task.FromResult(state));
    }

    public void SignOut()
    {
        SecureStorage.Default.Remove(UserIdKey);
        SecureStorage.Default.Remove(EmployeeIdKey);
        SecureStorage.Default.Remove(CompanyIdKey);
        SecureStorage.Default.Remove(RoleKey);
        SecureStorage.Default.Remove(JwtKey);
        SecureStorage.Default.Remove(ExpiresUtcKey);

        var state = new AuthenticationState(Anonymous);
        cachedState = state;
        NotifyAuthenticationStateChanged(Task.FromResult(state));
    }

    private static async Task<ClaimsPrincipal> GetCurrentPrincipalAsync()
    {
        var userId = await SecureStorage.Default.GetAsync(UserIdKey);
        var employeeId = await SecureStorage.Default.GetAsync(EmployeeIdKey);
        var companyIdText = await SecureStorage.Default.GetAsync(CompanyIdKey);
        var role = await SecureStorage.Default.GetAsync(RoleKey);
        var jwt = await SecureStorage.Default.GetAsync(JwtKey);
        var expiresUtcText = await SecureStorage.Default.GetAsync(ExpiresUtcKey);

        if (string.IsNullOrWhiteSpace(userId)
            || string.IsNullOrWhiteSpace(employeeId)
            || string.IsNullOrWhiteSpace(jwt)
            || !Guid.TryParse(companyIdText, out var companyId)
            || IsExpired(expiresUtcText))
        {
            SecureStorage.Default.Remove(UserIdKey);
            SecureStorage.Default.Remove(EmployeeIdKey);
            SecureStorage.Default.Remove(CompanyIdKey);
            SecureStorage.Default.Remove(RoleKey);
            SecureStorage.Default.Remove(JwtKey);
            SecureStorage.Default.Remove(ExpiresUtcKey);
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
