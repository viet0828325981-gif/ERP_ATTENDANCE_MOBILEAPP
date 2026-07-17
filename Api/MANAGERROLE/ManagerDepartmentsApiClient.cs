using ERP_PAYROLLMOBILE.Models.ManagerRole.ManagerDepartments;

namespace ERP_PAYROLLMOBILE.Api.MANAGERROLE;

public sealed class ManagerDepartmentsApiClient : ManagerApiClientBase
{
    public ManagerDepartmentsApiClient(HttpClient http)
        : base(http, nameof(ManagerDepartmentsApiClient))
    {
    }

    public async Task<List<ManagerDepartmentModel>> GetDepartmentsAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        var url = $"manager/departments?companyId={companyId}";
        var responseText = await SendGetAsync(url, "GetDepartments", cancellationToken);
        return DeserializeList<ManagerDepartmentModel>(responseText);
    }
}
