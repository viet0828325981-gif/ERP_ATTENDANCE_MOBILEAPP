using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ERP_PAYROLLMOBILE.Models.EmployeeBreakdown;

namespace ERP_PAYROLLMOBILE.Api.EMPLOYEEROLE.EmployeeBreakdown;

public sealed class EmployeeBreakdownApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient http;
    public EmployeeBreakdownApiClient(HttpClient http)
    {
        this.http = http;
    }

    public Task<int> SyncAsync(Guid companyId, EmployeeBreakdownSyncDto request, CancellationToken cancellationToken = default)
    {
        return SyncAsync(companyId, request.EmployeeId, request.Year, request.Month, cancellationToken);
    }

    public async Task<int> SyncAsync(Guid companyId, string employeeId, int year, int month, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeBreakdown/sync?companyId={companyId}&employeeId={Escape(employeeId)}&year={Format(year)}&month={Format(month)}";
        var responseText = await SendAsync(HttpMethod.Post, url, null, "Sync", cancellationToken);
        return Deserialize<int>(responseText, "sync payroll/breakdown");
    }

    public async Task<int> SyncWithBodyAsync(Guid companyId, EmployeeBreakdownSyncDto request, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeBreakdown/sync?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "SyncWithBody", cancellationToken);
        return Deserialize<int>(responseText, "sync payroll/breakdown");
    }

    public async Task<PayrollBreakdownModel> GetBreakdownAsync(Guid companyId, string employeeId, int year, int month, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeBreakdown?companyId={companyId}&employeeId={Escape(employeeId)}&year={Format(year)}&month={Format(month)}";
        var responseText = await SendAsync(HttpMethod.Get, url, null, "GetBreakdown", cancellationToken);
        return Deserialize<PayrollBreakdownModel>(responseText, "bảng công/lương chi tiết");
    }

    public async Task<ERPATT_CongphepduModel> GetLeaveBalanceAsync(string employeeId, int year, int month, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeBreakdown/leave-balance?employeeId={Escape(employeeId)}&year={Format(year)}&month={Format(month)}";
        var responseText = await SendAsync(HttpMethod.Get, url, null, "GetLeaveBalance", cancellationToken);
        return Deserialize<ERPATT_CongphepduModel>(responseText, "phép dư");
    }

    public async Task<List<ERPATT_CongphepduModel>> GetLeaveBalanceHistoryAsync(string employeeId, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeBreakdown/leave-balance-history?employeeId={Escape(employeeId)}";
        var responseText = await SendAsync(HttpMethod.Get, url, null, "GetLeaveBalanceHistory", cancellationToken);
        return DeserializeList<ERPATT_CongphepduModel>(responseText);
    }

    public async Task<List<SpecialWorkClaimDetailModel>> GetSpecialWorkClaimsAsync(Guid companyId, string employeeId, int year, int month, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeBreakdown/special-work-claims?companyId={companyId}&employeeId={Escape(employeeId)}&year={Format(year)}&month={Format(month)}";
        var responseText = await SendAsync(HttpMethod.Get, url, null, "GetSpecialWorkClaims", cancellationToken);
        return DeserializeList<SpecialWorkClaimDetailModel>(responseText);
    }

    public async Task<EmployeeShiftRegistrationSummaryDto> GetShiftRegistrationSummaryAsync(Guid companyId, string employeeId, int year, int month, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeBreakdown/shift-registration-summary?companyId={companyId}&employeeId={Escape(employeeId)}&year={Format(year)}&month={Format(month)}";
        var responseText = await SendAsync(HttpMethod.Get, url, null, "GetShiftRegistrationSummary", cancellationToken);
        return Deserialize<EmployeeShiftRegistrationSummaryDto>(responseText, "tổng quan đăng ký ca");
    }

    public async Task<EmployeeShiftRegistrationDetailResponseDto> GetShiftRegistrationDetailAsync(Guid companyId, string employeeId, int year, int month, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeBreakdown/shift-registration-detail?companyId={companyId}&employeeId={Escape(employeeId)}&year={Format(year)}&month={Format(month)}";
        var responseText = await SendAsync(HttpMethod.Get, url, null, "GetShiftRegistrationDetail", cancellationToken);
        return Deserialize<EmployeeShiftRegistrationDetailResponseDto>(responseText, "chi tiết đăng ký ca");
    }

    public async Task<EmployeeShiftRegistrationBreakdownDto> GetShiftRegistrationBreakdownAsync(Guid companyId, string employeeId, int year, int month, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeBreakdown/shift-registration-breakdown?companyId={companyId}&employeeId={Escape(employeeId)}&year={Format(year)}&month={Format(month)}";
        var responseText = await SendAsync(HttpMethod.Get, url, null, "GetShiftRegistrationBreakdown", cancellationToken);
        return Deserialize<EmployeeShiftRegistrationBreakdownDto>(responseText, "breakdown đăng ký ca");
    }

    private async Task<string> SendAsync(HttpMethod method, string relativeUrl, HttpContent? content, string operationName, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, relativeUrl)
        {
            Content = content
        };
        using var response = await http.SendAsync(request, cancellationToken);
        var responseText = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new EmployeeBreakdownApiException(operationName, response.StatusCode, responseText);
        }

        return responseText;
    }

    private async Task<string> SendJsonAsync<TRequest>(HttpMethod method, string relativeUrl, TRequest request, string operationName, CancellationToken cancellationToken)
    {
        using var content = JsonContent.Create(request, options: JsonOptions);
        return await SendAsync(method, relativeUrl, content, operationName, cancellationToken);
    }

    private static T Deserialize<T>(string responseText, string dataName)
    {
        var data = JsonSerializer.Deserialize<T>(responseText, JsonOptions);
        return data ?? throw new Exception($"Không deserialize được dữ liệu {dataName}.");
    }

    private static List<T> DeserializeList<T>(string responseText)
    {
        if (string.IsNullOrWhiteSpace(responseText))
        {
            return new List<T>();
        }

        return JsonSerializer.Deserialize<List<T>>(responseText, JsonOptions) ?? new List<T>();
    }

    private string ToAbsoluteUrl(string relativeUrl)
    {
        return http.BaseAddress is null ? relativeUrl : new Uri(http.BaseAddress, relativeUrl).ToString();
    }

    private static string Escape(string value)
    {
        return Uri.EscapeDataString(value);
    }

    private static string Format(int value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }
}

public sealed class EmployeeBreakdownApiException : Exception
{
    public EmployeeBreakdownApiException(string operationName, HttpStatusCode statusCode, string responseText)
        : base($"Không gọi được API EmployeeBreakdown ({operationName}): {responseText}")
    {
        OperationName = operationName;
        StatusCode = statusCode;
        ResponseText = responseText;
    }

    public string OperationName { get; }

    public HttpStatusCode StatusCode { get; }

    public string ResponseText { get; }

    public bool IsNotFound => StatusCode == HttpStatusCode.NotFound;
}
