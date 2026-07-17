using ERP_PAYROLLMOBILE.Models.EmployeeRequest;
using Microsoft.AspNetCore.Components.Authorization;

namespace ERP_PAYROLLMOBILE.RequestBuilders;

public sealed class EmployeeRequestRequestBuilder
{
    private static readonly Dictionary<string, string> RequestTypeMappings = new(StringComparer.OrdinalIgnoreCase)
    {
        ["EDIT_SHIFT"] = "EDIT_SHIFT",
        ["FORGOT_CHECK"] = "FORGOT_CHECK",
        ["MAKEUP_DAY"] = "MAKEUP_DAY",
        ["MAKEUP_HOUR"] = "MAKEUP_HOUR",
        ["NO_ATTENDANCE"] = "NO_ATTENDANCE",
        ["REGISTERS_SHIFT"] = "REGISTERS_SHIFT",
        ["WORK_HOLIDAY"] = "WORK_HOLIDAY",
        ["WORK_ONLINE"] = "WORK_ONLINE",
        ["WORK_ONLINE_HOLIDAY"] = "WORK_ONLINE_HOLIDAY",
        ["WORK_ONLINE_WEEKEND"] = "WORK_ONLINE_WEEKEND",
        ["WORK_OVERTIME"] = "WORK_OVERTIME",
        ["WORK_WEEKEND"] = "WORK_WEEKEND",
        ["Sửa đăng ký ca"] = "EDIT_SHIFT",
        ["Đăng ký ca"] = "REGISTERS_SHIFT",
        ["Quên chấm công"] = "FORGOT_CHECK",
        ["Làm bù theo ngày công"] = "MAKEUP_DAY",
        ["Làm bù theo giờ"] = "MAKEUP_HOUR",
        ["Không chấm công"] = "NO_ATTENDANCE",
        ["Làm ngày lễ"] = "WORK_HOLIDAY",
        ["Làm Online"] = "WORK_ONLINE",
        ["Làm Online ngày lễ"] = "WORK_ONLINE_HOLIDAY",
        ["Làm Online ngày l?"] = "WORK_ONLINE_HOLIDAY",
        ["Làm Online ngày cuối tuần"] = "WORK_ONLINE_WEEKEND",
        ["Làm Online ngày cu?i tu?n"] = "WORK_ONLINE_WEEKEND",
        ["Làm thêm giờ"] = "WORK_OVERTIME",
        ["Làm cuối tuần"] = "WORK_WEEKEND"
    };

    private static readonly Dictionary<string, string> RequestTypeDisplayNames = new(StringComparer.OrdinalIgnoreCase)
    {
        ["EDIT_SHIFT"] = "Sửa đăng ký ca",
        ["FORGOT_CHECK"] = "Quên chấm công",
        ["MAKEUP_DAY"] = "Làm bù theo ngày công",
        ["MAKEUP_HOUR"] = "Làm bù theo giờ",
        ["NO_ATTENDANCE"] = "Không chấm công",
        ["REGISTERS_SHIFT"] = "Đăng ký ca",
        ["WORK_HOLIDAY"] = "Làm ngày lễ",
        ["WORK_ONLINE"] = "Làm Online",
        ["WORK_ONLINE_HOLIDAY"] = "Làm Online ngày lễ",
        ["WORK_ONLINE_WEEKEND"] = "Làm Online ngày cuối tuần",
        ["WORK_OVERTIME"] = "Làm thêm giờ",
        ["WORK_WEEKEND"] = "Làm cuối tuần"
    };

    private static readonly HashSet<string> SupportedCreateRequestTypes =
    [
        "FORGOT_CHECK",
        "MAKEUP_HOUR",
        "WORK_OVERTIME",
        "WORK_HOLIDAY",
        "WORK_WEEKEND",
        "WORK_ONLINE",
        "WORK_ONLINE_HOLIDAY",
        "WORK_ONLINE_WEEKEND"
    ];

    private readonly AuthenticationStateProvider authenticationStateProvider;

    public EmployeeRequestRequestBuilder(AuthenticationStateProvider authenticationStateProvider)
    {
        this.authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<EmployeeRequestQueryDto> BuildQueryAsync(Guid companyId, DateOnly month, string? requestType, bool groupOnline)
    {
        var employeeId = await GetEmployeeIdAsync();
        return new EmployeeRequestQueryDto
        {
            CompanyId = companyId,
            EmployeeId = employeeId,
            Year = month.Year,
            Month = month.Month,
            RequestType = NormalizeRequestType(requestType),
            GroupOnline = groupOnline
        };
    }

    public async Task<EmployeeRequestDailyDetailQueryDto> BuildDailyDetailQueryAsync(Guid companyId, DateOnly workDate)
    {
        var employeeId = await GetEmployeeIdAsync();
        return new EmployeeRequestDailyDetailQueryDto
        {
            CompanyId = companyId,
            EmployeeId = employeeId,
            WorkDate = workDate
        };
    }

    public async Task<EmployeeRequestOnlineGroupDetailQueryDto> BuildOnlineGroupDetailQueryAsync(Guid companyId, string requestType, DateTime createdAt)
    {
        var employeeId = await GetEmployeeIdAsync();
        return new EmployeeRequestOnlineGroupDetailQueryDto
        {
            CompanyId = companyId,
            EmployeeId = employeeId,
            RequestType = NormalizeRequestType(requestType) ?? string.Empty,
            CreatedAt = createdAt
        };
    }

    public EmployeeRequestCalendarQueryDto BuildCalendarQuery(DateOnly month)
    {
        return new EmployeeRequestCalendarQueryDto
        {
            Year = month.Year,
            Month = month.Month
        };
    }

    public async Task<EmployeeCancelRequestDto> BuildCancelRequestAsync(Guid companyId, Guid requestId)
    {
        var employeeId = await GetEmployeeIdAsync();
        return new EmployeeCancelRequestDto
        {
            RequestId = requestId,
            CompanyId = companyId,
            EmployeeId = employeeId
        };
    }

    public async Task<EmployeeCancelOnlineGroupRequestDto> BuildCancelOnlineGroupRequestAsync(string requestType, DateTime createdAt)
    {
        var employeeId = await GetEmployeeIdAsync();
        return new EmployeeCancelOnlineGroupRequestDto
        {
            EmployeeId = employeeId,
            RequestType = NormalizeRequestType(requestType) ?? string.Empty,
            CreatedAt = createdAt
        };
    }

    public async Task<EmployeeForgotCheckRequestDto> BuildForgotCheckRequestAsync(DateOnly targetDate, string? reason)
    {
        var employeeId = await GetEmployeeIdAsync();
        return new EmployeeForgotCheckRequestDto
        {
            EmployeeId = employeeId,
            TargetDate = targetDate,
            Reason = NormalizeText(reason),
            CreatedBy = employeeId
        };
    }

    public async Task<EmployeeMakeupHourRequestDto> BuildMakeupHourRequestAsync(DateOnly targetDate, DateOnly sourceDate, int minutesAffected, string? reason)
    {
        var employeeId = await GetEmployeeIdAsync();
        return new EmployeeMakeupHourRequestDto
        {
            EmployeeId = employeeId,
            TargetDate = targetDate,
            SourceDate = sourceDate,
            MinutesAffected = minutesAffected,
            Reason = NormalizeText(reason),
            CreatedBy = employeeId
        };
    }

    public async Task<EmployeeOvertimeRequestDto> BuildOvertimeRequestAsync(DateOnly sourceDate, int minutesAffected, string? reason)
    {
        var employeeId = await GetEmployeeIdAsync();
        return new EmployeeOvertimeRequestDto
        {
            EmployeeId = employeeId,
            SourceDate = sourceDate,
            MinutesAffected = minutesAffected,
            Reason = NormalizeText(reason),
            CreatedBy = employeeId
        };
    }

    public async Task<EmployeeSpecialDayRequestDto> BuildSpecialDayRequestAsync(DateOnly sourceDate, string? reason)
    {
        var employeeId = await GetEmployeeIdAsync();
        return new EmployeeSpecialDayRequestDto
        {
            EmployeeId = employeeId,
            SourceDate = sourceDate,
            Reason = NormalizeText(reason),
            CreatedBy = employeeId
        };
    }

    public async Task<EmployeeOnlineRequestDto> BuildOnlineRequestAsync(IEnumerable<DateOnly> onlineDates, string shiftCode, string? reason)
    {
        var employeeId = await GetEmployeeIdAsync();
        return new EmployeeOnlineRequestDto
        {
            EmployeeId = employeeId,
            OnlineDates = onlineDates.Distinct().OrderBy(item => item).ToList(),
            ShiftCode = shiftCode.Trim(),
            Reason = NormalizeText(reason),
            CreatedBy = employeeId
        };
    }

    public List<EmployeeRequestTypeDto> NormalizeRequestTypes(IEnumerable<EmployeeRequestTypeDto> requestTypes)
    {
        return requestTypes
            .Select(item =>
            {
                var normalizedCode = NormalizeRequestType(item.RequestType) ?? NormalizeRequestType(item.RequestName) ?? item.RequestType;
                return new EmployeeRequestTypeDto
                {
                    RequestType = normalizedCode,
                    RequestName = GetRequestTypeDisplayName(normalizedCode, item.RequestName)
                };
            })
            .GroupBy(item => item.RequestType, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.First())
            .OrderBy(item => item.RequestName, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public bool IsCreateSupported(string? requestType)
    {
        var normalizedRequestType = NormalizeRequestType(requestType);
        return !string.IsNullOrWhiteSpace(normalizedRequestType)
            && SupportedCreateRequestTypes.Contains(normalizedRequestType);
    }

    public string GetRequestTypeDisplayName(string? requestType, string? fallbackName = null)
    {
        var normalizedRequestType = NormalizeRequestType(requestType);
        if (!string.IsNullOrWhiteSpace(normalizedRequestType)
            && RequestTypeDisplayNames.TryGetValue(normalizedRequestType, out var displayName))
        {
            return displayName;
        }

        return string.IsNullOrWhiteSpace(fallbackName) ? requestType ?? string.Empty : fallbackName;
    }

    public string? NormalizeRequestType(string? requestType)
    {
        if (string.IsNullOrWhiteSpace(requestType))
        {
            return null;
        }

        var normalized = requestType.Trim();
        return RequestTypeMappings.TryGetValue(normalized, out var mappedValue)
            ? mappedValue
            : normalized;
    }

    private static string? NormalizeText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private async Task<string> GetEmployeeIdAsync()
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        return user.FindFirst("employee_id")?.Value
            ?? user.FindFirst("login_user_id")?.Value
            ?? user.Identity?.Name
            ?? string.Empty;
    }
}
