using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;

namespace ERP_PAYROLLMOBILE.Api.MANAGERROLE;

public abstract class ManagerApiClientBase
{
    protected static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly string clientName;
    protected readonly HttpClient Http;

    protected ManagerApiClientBase(HttpClient http, string clientName)
    {
        Http = http;
        this.clientName = clientName;
    }

    protected async Task<string> SendAsync(HttpMethod method, string relativeUrl, HttpContent? content, string operationName, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, relativeUrl)
        {
            Content = content
        };
        using var response = await Http.SendAsync(request, cancellationToken);
        var responseText = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Không gọi được API manager ({operationName}): {responseText}");
        }

        return responseText;
    }

    protected Task<string> SendGetAsync(string relativeUrl, string operationName, CancellationToken cancellationToken = default)
    {
        return SendAsync(HttpMethod.Get, relativeUrl, null, operationName, cancellationToken);
    }

    protected async Task<string> SendJsonAsync<TRequest>(HttpMethod method, string relativeUrl, TRequest request, string operationName, CancellationToken cancellationToken = default)
    {
        using var content = JsonContent.Create(request, options: JsonOptions);
        return await SendAsync(method, relativeUrl, content, operationName, cancellationToken);
    }

    protected static T Deserialize<T>(string responseText, string dataName)
    {
        var data = JsonSerializer.Deserialize<T>(responseText, JsonOptions);
        return data ?? throw new Exception($"Không deserialize được dữ liệu {dataName}.");
    }

    protected static List<T> DeserializeList<T>(string responseText)
    {
        if (string.IsNullOrWhiteSpace(responseText))
        {
            return new List<T>();
        }

        return JsonSerializer.Deserialize<List<T>>(responseText, JsonOptions) ?? new List<T>();
    }

    protected static string DeserializeMessage(string responseText)
    {
        if (string.IsNullOrWhiteSpace(responseText))
        {
            return string.Empty;
        }

        try
        {
            return JsonSerializer.Deserialize<string>(responseText, JsonOptions) ?? responseText;
        }
        catch (JsonException)
        {
            return responseText;
        }
    }

    protected string ToAbsoluteUrl(string relativeUrl)
    {
        return Http.BaseAddress is null ? relativeUrl : new Uri(Http.BaseAddress, relativeUrl).ToString();
    }

    protected static string Escape(string value)
    {
        return Uri.EscapeDataString(value);
    }

    protected static string Format(int value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    protected static string Format(int? value)
    {
        return value?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
    }

    protected static string Format(DateOnly value)
    {
        return value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    protected static string Format(DateTime value)
    {
        return value.ToString("O", CultureInfo.InvariantCulture);
    }
}
