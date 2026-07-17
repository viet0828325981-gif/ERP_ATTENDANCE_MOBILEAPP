using ERP_PAYROLLMOBILE.Api.EMPLOYEEROLE.EmployeeIShiftCode;
using ERP_PAYROLLMOBILE.Api.MANAGERROLE;
using ERP_PAYROLLMOBILE.Models;
using ERP_PAYROLLMOBILE.Models.EmployeeRequest;
using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerShift;
using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerShiftRegistration;

namespace ERP_PAYROLLMOBILE.Services.Features.Shift;

public sealed class ShiftFeatureService : IShiftFeatureService
{
    private readonly ShiftcodeApiClient employeeApiClient;
    private readonly ManagerShiftApiClient managerShiftApiClient;
    private readonly ManagerShiftRegistrationApiClient managerRegistrationApiClient;

    public ShiftFeatureService(
        ShiftcodeApiClient employeeApiClient,
        ManagerShiftApiClient managerShiftApiClient,
        ManagerShiftRegistrationApiClient managerRegistrationApiClient)
    {
        this.employeeApiClient = employeeApiClient;
        this.managerShiftApiClient = managerShiftApiClient;
        this.managerRegistrationApiClient = managerRegistrationApiClient;
    }

    public Task<IReadOnlyList<ShiftRegistrationDayModel>> GetEmployeeShiftDaysAsync(Guid companyId, string employeeId, int month, int year, CancellationToken cancellationToken = default)
    {
        return employeeApiClient.GetDailyAttendanceAsync(companyId, employeeId, month, year);
    }

    public Task<ShiftRegistrationOverviewModel> GetEmployeeShiftOverviewAsync(Guid companyId, string employeeId, int month, int year, CancellationToken cancellationToken = default)
    {
        return employeeApiClient.GetShiftRegistrationOverviewAsync(companyId, employeeId, month, year);
    }

    public Task<List<ShiftModel>> GetShiftsAsync(Guid companyId, Guid departmentId, CancellationToken cancellationToken = default)
    {
        return managerShiftApiClient.GetShiftsAsync(companyId, departmentId, cancellationToken);
    }

    public Task<List<EmployeeShiftSummaryModel>> GetEmployeeSummariesAsync(Guid companyId, int year, int month, CancellationToken cancellationToken = default)
    {
        return managerShiftApiClient.GetEmployeeSummariesAsync(companyId, year, month, cancellationToken);
    }

    public Task<List<EmployeeDetailShiftCodeInMonthModel>> GetEmployeeDayDetailsAsync(Guid companyId, string employeeId, int month, int year, CancellationToken cancellationToken = default)
    {
        return managerShiftApiClient.GetEmployeeDayDetailsAsync(companyId, employeeId, month, year, cancellationToken);
    }

    public Task<List<WeeklyShiftScheduleRow>> GetWeeklyScheduleAsync(Guid companyId, Guid departmentId, DateOnly weekStartDate, CancellationToken cancellationToken = default)
    {
        return managerShiftApiClient.GetWeeklyScheduleAsync(companyId, departmentId, weekStartDate, cancellationToken);
    }

    public Task<string> RegisterWeekAsync(Guid companyId, ManagerRegisterWeekShiftDto request, CancellationToken cancellationToken = default)
    {
        return managerShiftApiClient.RegisterWeekAsync(companyId, request, cancellationToken);
    }

    public Task<string> RegisterMonthAsync(Guid companyId, ManagerRegisterMonthShiftDto request, CancellationToken cancellationToken = default)
    {
        return managerShiftApiClient.RegisterMonthAsync(companyId, request, cancellationToken);
    }

    public Task<List<WorkRequestModel>> GetShiftRegistrationsAsync(Guid companyId, int year, int month, Guid? departmentId = null, int? status = null, CancellationToken cancellationToken = default)
    {
        return managerRegistrationApiClient.GetShiftRegistrationsAsync(companyId, year, month, departmentId, status, cancellationToken);
    }

    public Task<List<ShiftRegistrationDayDetailModel>> GetRegistrationDaysAsync(Guid companyId, Guid requestId, CancellationToken cancellationToken = default)
    {
        return managerRegistrationApiClient.GetRegistrationDaysAsync(companyId, requestId, cancellationToken);
    }

    public Task<string> ApproveRegistrationAsync(Guid companyId, ManagerApproveShiftRegistrationDto request, CancellationToken cancellationToken = default)
    {
        return managerRegistrationApiClient.ApproveAsync(companyId, request, cancellationToken);
    }

    public Task<ManagerApproveAllShiftRegistrationResult> ApproveAllRegistrationsAsync(Guid companyId, ManagerApproveAllShiftRegistrationDto request, CancellationToken cancellationToken = default)
    {
        return managerRegistrationApiClient.ApproveAllAsync(companyId, request, cancellationToken);
    }

    public Task<string> RejectRegistrationAsync(Guid companyId, ManagerRejectShiftRegistrationDto request, CancellationToken cancellationToken = default)
    {
        return managerRegistrationApiClient.RejectAsync(companyId, request, cancellationToken);
    }
}
