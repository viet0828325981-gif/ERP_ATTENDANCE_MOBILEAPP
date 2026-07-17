namespace ERP_PAYROLLMOBILE.Api;

public sealed class LoginRequest
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}

public sealed class IdentityLoginResult
{
    public bool Success { get; init; }

    public string? Jwt { get; init; }

    public string? UserId { get; init; }

    public string? EmployeeId { get; init; }

    public Guid CompanyId { get; init; }

    public string? Role { get; init; }

    public string? ErrorMessage { get; init; }

    public static IdentityLoginResult Failed(string errorMessage) => new()
    {
        Success = false,
        ErrorMessage = errorMessage
    };

    public static IdentityLoginResult Succeeded(string jwt, string userId, string employeeId, Guid companyId, string? role) => new()
    {
        Success = true,
        Jwt = jwt,
        UserId = userId,
        EmployeeId = employeeId,
        CompanyId = companyId,
        Role = role
    };
}
