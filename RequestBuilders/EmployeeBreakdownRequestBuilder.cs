using ERP_PAYROLLMOBILE.Models.EmployeeBreakdown;
using Microsoft.AspNetCore.Components.Authorization;

namespace ERP_PAYROLLMOBILE.RequestBuilders;

public sealed class EmployeeBreakdownRequestBuilder
{
    private readonly AuthenticationStateProvider authenticationStateProvider;

    public EmployeeBreakdownRequestBuilder(AuthenticationStateProvider authenticationStateProvider)
    {
        this.authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<EmployeeBreakdownSyncDto> BuildSyncAsync(DateOnly month)
    {
        var employeeId = await GetEmployeeIdAsync();
        return new EmployeeBreakdownSyncDto
        {
            EmployeeId = employeeId,
            Year = month.Year,
            Month = month.Month
        };
    }

    public async Task<EmployeeBreakdownQuery> BuildQueryAsync(Guid companyId, DateOnly month)
    {
        var employeeId = await GetEmployeeIdAsync();
        return new EmployeeBreakdownQuery(companyId, employeeId, month.Year, month.Month);
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

public sealed record EmployeeBreakdownQuery(Guid CompanyId, string EmployeeId, int Year, int Month);
