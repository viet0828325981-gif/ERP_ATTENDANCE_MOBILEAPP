using ERP_PAYROLLMOBILE.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ERP_PAYROLLMOBILE.RequestBuilders;

public sealed class AttendanceRequestBuilder
{
    private readonly AuthenticationStateProvider authenticationStateProvider;

    public AttendanceRequestBuilder(AuthenticationStateProvider authenticationStateProvider)
    {
        this.authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<AttendanceDailyRequest> BuildDailyAsync(Guid companyId, DateOnly workDate)
    {
        var employeeId = await GetEmployeeIdAsync();
        return new AttendanceDailyRequest(companyId, employeeId, workDate);
    }

    public async Task<AttendanceMonthlyRequest> BuildMonthlyAsync(Guid companyId, DateOnly month)
    {
        var employeeId = await GetEmployeeIdAsync();
        var from = new DateOnly(month.Year, month.Month, 1);
        var to = from.AddMonths(1).AddDays(-1);

        return new AttendanceMonthlyRequest(companyId, employeeId, from, to);
    }

    public async Task<AttendanceDetailRequest> BuildDetailAsync(Guid companyId, DateOnly workDate)
    {
        var employeeId = await GetEmployeeIdAsync();
        return new AttendanceDetailRequest(companyId, employeeId, workDate);
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
