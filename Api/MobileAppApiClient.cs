using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace ERP_PAYROLLMOBILE.Api;

public sealed class EmployeeMobileTokenRequestDto
{
    [JsonPropertyName("employeeId")]
    public string EmployeeId { get; set; } = string.Empty;

    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;
}

public sealed class MobileAppApiClient
{
    private readonly HttpClient http;

    public MobileAppApiClient(HttpClient http)
    {
        this.http = http;
    }

    public async Task<string> InsertEmployeeMobileTokenAsync(EmployeeMobileTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        using var response = await http.PostAsJsonAsync("MobileApp/employee-token", request, cancellationToken);
        var responseText = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Không gọi được API employee-token: {responseText}");
        }

        return responseText;
    }
}