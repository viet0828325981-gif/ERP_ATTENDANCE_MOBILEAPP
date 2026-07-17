using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using ERP_PAYROLLMOBILE.Models;
using ERP_PAYROLLMOBILE.Models.EmployeeRequest;

namespace ERP_PAYROLLMOBILE.Api.EMPLOYEEROLE.EmployeeRequest;

public sealed class EmployeeRequestApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient http;
    public EmployeeRequestApiClient(HttpClient http)
    {
        this.http = http;
    }

    public async Task<List<EmployeeRequestTypeDto>> GetRequestTypesAsync(CancellationToken cancellationToken = default)
    {
        var responseText = await SendGetAsync("EmployeeRequest/request-types", "GetRequestTypes", cancellationToken);
        return DeserializeList<EmployeeRequestTypeDto>(responseText);
    }

    public Task<List<WorkRequestModel>> GetRequestsAsync(EmployeeRequestQueryDto request, CancellationToken cancellationToken = default)
    {
        return GetRequestsAsync(
            request.CompanyId,
            request.EmployeeId,
            request.Year,
            request.Month,
            request.RequestType,
            request.GroupOnline,
            cancellationToken);
    }

    public Task<AttendanceDailyViewModel> GetDailyDetailAsync(EmployeeRequestDailyDetailQueryDto request, CancellationToken cancellationToken = default)
    {
        return GetDailyDetailAsync(request.CompanyId, request.EmployeeId, request.WorkDate, cancellationToken);
    }

    public Task<List<WorkRequestModel>> GetOnlineGroupDetailAsync(EmployeeRequestOnlineGroupDetailQueryDto request, CancellationToken cancellationToken = default)
    {
        return GetOnlineGroupDetailAsync(request.CompanyId, request.EmployeeId, request.RequestType, request.CreatedAt, cancellationToken);
    }

    public Task<List<DimCalendarModel>> GetCalendarAsync(EmployeeRequestCalendarQueryDto request, CancellationToken cancellationToken = default)
    {
        return GetCalendarAsync(request.Year, request.Month, cancellationToken);
    }

    public async Task<List<WorkRequestModel>> GetRequestsAsync(
        Guid companyId,
        string employeeId,
        int year,
        int month,
        string? requestType = null,
        bool groupOnline = true,
        CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeRequest?companyId={companyId}&employeeId={Escape(employeeId)}&year={Format(year)}&month={Format(month)}&groupOnline={Format(groupOnline)}";
        if (!string.IsNullOrWhiteSpace(requestType))
        {
            url += $"&requestType={Escape(requestType)}";
        }

        var responseText = await SendGetAsync(url, "GetRequests", cancellationToken);
        return DeserializeList<WorkRequestModel>(responseText);
    }

    public async Task<AttendanceDailyViewModel> GetDailyDetailAsync(Guid companyId, string employeeId, DateOnly workDate, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeRequest/daily-detail?companyId={companyId}&employeeId={Escape(employeeId)}&workDate={workDate:yyyy-MM-dd}";
        var responseText = await SendGetAsync(url, "GetDailyDetail", cancellationToken);
        return Deserialize<AttendanceDailyViewModel>(responseText, "chi tiết ngày công");
    }

    public async Task<List<WorkRequestModel>> GetOnlineGroupDetailAsync(Guid companyId, string employeeId, string requestType, DateTime createdAt, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeRequest/online-group-detail?companyId={companyId}&employeeId={Escape(employeeId)}&requestType={Escape(requestType)}&createdAt={Escape(createdAt.ToString("O", CultureInfo.InvariantCulture))}";
        var responseText = await SendGetAsync(url, "GetOnlineGroupDetail", cancellationToken);
        return DeserializeList<WorkRequestModel>(responseText);
    }

    public async Task<List<ShiftModel>> GetOnlineShiftsAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeRequest/online-shifts?companyId={companyId}";
        var responseText = await SendGetAsync(url, "GetOnlineShifts", cancellationToken);
        return DeserializeList<ShiftModel>(responseText);
    }

    public async Task<List<DimCalendarModel>> GetCalendarAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeRequest/calendar?year={Format(year)}&month={Format(month)}";
        var responseText = await SendGetAsync(url, "GetCalendar", cancellationToken);
        return DeserializeList<DimCalendarModel>(responseText);
    }

    public async Task<string> PostForgotCheckAsync(Guid companyId, EmployeeForgotCheckRequestDto request, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeRequest/forgot-check?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "PostForgotCheck", cancellationToken);
        return DeserializeMessage(responseText);
    }

    public async Task<string> PostWorkOnlineAsync(Guid companyId, EmployeeOnlineRequestDto request, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeRequest/work-online?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "PostWorkOnline", cancellationToken);
        return DeserializeMessage(responseText);
    }

    public async Task<string> PostWorkOnlineWeekendAsync(Guid companyId, EmployeeOnlineRequestDto request, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeRequest/work-online-weekend?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "PostWorkOnlineWeekend", cancellationToken);
        return DeserializeMessage(responseText);
    }

    public async Task<string> PostWorkOnlineHolidayAsync(Guid companyId, EmployeeOnlineRequestDto request, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeRequest/work-online-holiday?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "PostWorkOnlineHoliday", cancellationToken);
        return DeserializeMessage(responseText);
    }

    public async Task<string> PostMakeupHourAsync(Guid companyId, EmployeeMakeupHourRequestDto request, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeRequest/makeup-hour?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "PostMakeupHour", cancellationToken);
        return DeserializeMessage(responseText);
    }

    public async Task<string> PostWorkOvertimeAsync(Guid companyId, EmployeeOvertimeRequestDto request, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeRequest/work-overtime?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "PostWorkOvertime", cancellationToken);
        return DeserializeMessage(responseText);
    }

    public async Task<string> PostWorkHolidayAsync(Guid companyId, EmployeeSpecialDayRequestDto request, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeRequest/work-holiday?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "PostWorkHoliday", cancellationToken);
        return DeserializeMessage(responseText);
    }

    public async Task<string> PostWorkWeekendAsync(Guid companyId, EmployeeSpecialDayRequestDto request, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeRequest/work-weekend?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "PostWorkWeekend", cancellationToken);
        return DeserializeMessage(responseText);
    }

    public async Task<string> CancelAsync(EmployeeCancelRequestDto request, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeRequest/cancel/{request.RequestId}?companyId={request.CompanyId}&employeeId={Escape(request.EmployeeId)}";
        var responseText = await SendAsync(HttpMethod.Delete, url, null, "Cancel", cancellationToken);
        return DeserializeMessage(responseText);
    }

    public async Task<string> CancelOnlineGroupAsync(Guid companyId, EmployeeCancelOnlineGroupRequestDto request, CancellationToken cancellationToken = default)
    {
        var url = $"EmployeeRequest/cancel-online-group?companyId={companyId}";
        var responseText = await SendJsonAsync(HttpMethod.Post, url, request, "CancelOnlineGroup", cancellationToken);
        return DeserializeMessage(responseText);
    }

    private async Task<string> SendGetAsync(string relativeUrl, string operationName, CancellationToken cancellationToken)
    {
        return await SendAsync(HttpMethod.Get, relativeUrl, null, operationName, cancellationToken);
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
            throw new Exception($"Không gọi được API EmployeeRequest ({operationName}): {responseText}");
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

    private static string DeserializeMessage(string responseText)
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

    private static string Format(bool value)
    {
        return value.ToString().ToLowerInvariant();
    }
}
