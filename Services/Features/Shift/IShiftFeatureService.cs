using ERP_PAYROLLMOBILE.Models.EmployeeRequest;
using ERP_PAYROLLMOBILE.Models;
using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerShift;
using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerShiftRegistration;

namespace ERP_PAYROLLMOBILE.Services.Features.Shift;

public interface IShiftFeatureService
{
    Task<IReadOnlyList<ShiftRegistrationDayModel>> GetEmployeeShiftDaysAsync(Guid companyId, string employeeId, int month, int year, CancellationToken cancellationToken = default);
    Task<ShiftRegistrationOverviewModel> GetEmployeeShiftOverviewAsync(Guid companyId, string employeeId, int month, int year, CancellationToken cancellationToken = default);
    Task<List<ShiftModel>> GetShiftsAsync(Guid companyId, Guid departmentId, CancellationToken cancellationToken = default);
    Task<List<EmployeeShiftSummaryModel>> GetEmployeeSummariesAsync(Guid companyId, int year, int month, CancellationToken cancellationToken = default);
    Task<List<EmployeeDetailShiftCodeInMonthModel>> GetEmployeeDayDetailsAsync(Guid companyId, string employeeId, int month, int year, CancellationToken cancellationToken = default);
    Task<List<WeeklyShiftScheduleRow>> GetWeeklyScheduleAsync(Guid companyId, Guid departmentId, DateOnly weekStartDate, CancellationToken cancellationToken = default);
    Task<string> RegisterWeekAsync(Guid companyId, ManagerRegisterWeekShiftDto request, CancellationToken cancellationToken = default);
    Task<string> RegisterMonthAsync(Guid companyId, ManagerRegisterMonthShiftDto request, CancellationToken cancellationToken = default);
    Task<List<WorkRequestModel>> GetShiftRegistrationsAsync(Guid companyId, int year, int month, Guid? departmentId = null, int? status = null, CancellationToken cancellationToken = default);
    Task<List<ShiftRegistrationDayDetailModel>> GetRegistrationDaysAsync(Guid companyId, Guid requestId, CancellationToken cancellationToken = default);
    Task<string> ApproveRegistrationAsync(Guid companyId, ManagerApproveShiftRegistrationDto request, CancellationToken cancellationToken = default);
    Task<ManagerApproveAllShiftRegistrationResult> ApproveAllRegistrationsAsync(Guid companyId, ManagerApproveAllShiftRegistrationDto request, CancellationToken cancellationToken = default);
    Task<string> RejectRegistrationAsync(Guid companyId, ManagerRejectShiftRegistrationDto request, CancellationToken cancellationToken = default);
}
