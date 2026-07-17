using ERP_PAYROLLMOBILE.Models;
using System.Text.Json;

namespace ERP_PAYROLLMOBILE.Api;

public class LogInApiclient
{
    public readonly HttpClient http;

    public LogInApiclient(HttpClient http)
    {
        this.http = http;
    }

    public async Task<LogInModel?> GetLoginAsync(string userId, Guid companyid)
    {
        var url = $"LogIn?userId={Uri.EscapeDataString(userId)}&companyId={companyid}";
        var response = await http.GetAsync(url);
        var responseText = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var data = JsonSerializer.Deserialize<LogInModel>(responseText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return data;
        }

        return null;
    }
}
