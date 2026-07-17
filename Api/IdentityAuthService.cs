using System.Net.Http.Json;
using System.Text.Json;

namespace ERP_PAYROLLMOBILE.Api;

public sealed class IdentityAuthService
{
    private static readonly Guid CompanyId = Guid.Parse("A2F3FB40-7FA2-F011-9DB7-208810C238D7");
    private const string LoginPath = "api/user/login";
    private readonly HttpClient httpClient;
    private readonly LogInApiclient logInApiclient;

    public IdentityAuthService(IHttpClientFactory httpClientFactory, LogInApiclient logInApiclient)
    {
        httpClient = httpClientFactory.CreateClient("MoonIdentity");
        this.logInApiclient = logInApiclient;
    }

    public async Task<IdentityLoginResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(LoginPath, new
            {
                userName = request.UserName,
                password = request.Password,
                rememberMe = request.RememberMe
            }, cancellationToken);

            var responseText = await response.Content.ReadAsStringAsync(cancellationToken);
            using var jsonDocument = TryParseJson(responseText);

            if (!response.IsSuccessStatusCode)
            {
                return IdentityLoginResult.Failed(ReadFirstJsonString(jsonDocument?.RootElement, "message", "error", "title") ?? responseText);
            }

            var root = jsonDocument?.RootElement;
            var jwt = ReadFirstJsonString(root,
                "token",
                "jwt",
                "accessToken",
                "access_token",
                "data.token",
                "data.jwt",
                "data.accessToken",
                "result.token",
                "result.jwt",
                "result.accessToken");

            if (string.IsNullOrWhiteSpace(jwt))
            {
                return IdentityLoginResult.Failed("API không trả về JWT hợp lệ.");
            }

            var userId = ReadFirstJsonString(root, "userId") ?? request.UserName;
            var loginProfile = await logInApiclient.GetLoginAsync(userId, CompanyId);
            if (loginProfile is null || string.IsNullOrWhiteSpace(loginProfile.employeeId))
            {
                return IdentityLoginResult.Failed("Không lấy được employeeId sau khi đăng nhập.");
            }

            return IdentityLoginResult.Succeeded(jwt, userId, loginProfile.employeeId, CompanyId, loginProfile.role);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            return IdentityLoginResult.Failed($"Không thể kết nối API đăng nhập: {ex.Message}");
        }
    }

    private static JsonDocument? TryParseJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        try
        {
            return JsonDocument.Parse(json);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static string? ReadFirstJsonString(JsonElement? root, params string[] paths)
    {
        if (root is null)
        {
            return null;
        }

        foreach (var path in paths)
        {
            if (TryReadJsonString(root.Value, path, out var value) && !string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        return null;
    }

    private static bool TryReadJsonString(JsonElement root, string path, out string? value)
    {
        value = null;
        var current = root;

        foreach (var segment in path.Split('.'))
        {
            if (current.ValueKind != JsonValueKind.Object || !TryGetPropertyIgnoreCase(current, segment, out current))
            {
                return false;
            }
        }

        value = current.ValueKind switch
        {
            JsonValueKind.String => current.GetString(),
            JsonValueKind.Number or JsonValueKind.True or JsonValueKind.False => current.ToString(),
            _ => null
        };

        return value is not null;
    }

    private static bool TryGetPropertyIgnoreCase(JsonElement element, string propertyName, out JsonElement value)
    {
        foreach (var property in element.EnumerateObject())
        {
            if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                value = property.Value;
                return true;
            }
        }

        value = default;
        return false;
    }
}
