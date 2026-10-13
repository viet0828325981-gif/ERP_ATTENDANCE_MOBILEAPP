using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace ERP_PAYROLLMOBILE.Api;

public sealed class MoonAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());
    private static readonly TimeSpan RestoreTimeout = TimeSpan.FromSeconds(8);
    private static readonly string[] StorageKeys =
    [
        IdentityStorageKeys.UserIdKey,
        IdentityStorageKeys.EmployeeIdKey,
        IdentityStorageKeys.CompanyIdKey,
        IdentityStorageKeys.RoleKey,
        IdentityStorageKeys.JwtKey,
        IdentityStorageKeys.ExpiresUtcKey
    ];

    private readonly SemaphoreSlim syncRoot = new(1, 1);
    private readonly ILogger<MoonAuthenticationStateProvider> logger;
    private AuthenticationState? cachedState;

    public MoonAuthenticationStateProvider(ILogger<MoonAuthenticationStateProvider> logger)
    {
        this.logger = logger;
    }

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
                var principal = await RestorePrincipalOrAnonymousAsync();
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

        try
        {
            await SecureStorage.Default.SetAsync(IdentityStorageKeys.UserIdKey, userId);
            await SecureStorage.Default.SetAsync(IdentityStorageKeys.EmployeeIdKey, employeeId);
            await SecureStorage.Default.SetAsync(IdentityStorageKeys.CompanyIdKey, companyId.ToString());
            await SecureStorage.Default.SetAsync(IdentityStorageKeys.RoleKey, role ?? string.Empty);
            await SecureStorage.Default.SetAsync(IdentityStorageKeys.JwtKey, jwt);
            await SecureStorage.Default.SetAsync(IdentityStorageKeys.ExpiresUtcKey, expiresUtc.ToString("O", CultureInfo.InvariantCulture));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Không thể lưu phiên đăng nhập vào SecureStorage.");
            Console.Error.WriteLine($"[Authentication] SecureStorage write failed: {ex}");
            ClearStoredIdentity();
            throw new InvalidOperationException("Thiết bị không thể lưu phiên đăng nhập an toàn. Vui lòng thử lại.", ex);
        }

        var state = new AuthenticationState(CreatePrincipal(userId, employeeId, companyId, jwt, role));
        cachedState = state;
        NotifyAuthenticationStateChanged(Task.FromResult(state));
    }

    public void SignOut()
    {
        ClearStoredIdentity();

        var state = new AuthenticationState(Anonymous);
        cachedState = state;
        NotifyAuthenticationStateChanged(Task.FromResult(state));
    }

    private async Task<ClaimsPrincipal> RestorePrincipalOrAnonymousAsync()
    {
        try
        {
            return await GetCurrentPrincipalAsync().WaitAsync(RestoreTimeout);
        }
        catch (TimeoutException ex)
        {
            logger.LogError(ex, "Quá thời gian khôi phục phiên đăng nhập. Tiếp tục với trạng thái chưa đăng nhập.");
            Console.Error.WriteLine($"[Authentication] Session restore timed out: {ex}");
            return Anonymous;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Không thể khôi phục phiên đăng nhập. Tiếp tục với trạng thái chưa đăng nhập.");
            Console.Error.WriteLine($"[Authentication] Session restore failed: {ex}");
            ClearStoredIdentity();
            return Anonymous;
        }
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
            ClearStoredIdentity();
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

    private static void ClearStoredIdentity()
    {
        foreach (var key in StorageKeys)
        {
            try
            {
                SecureStorage.Default.Remove(key);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Authentication] SecureStorage remove failed for '{key}': {ex}");
            }
        }
    }
}
