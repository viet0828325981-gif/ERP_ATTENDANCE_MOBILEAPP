using ERP_PAYROLLMOBILE.Models;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;

namespace ERP_PAYROLLMOBILE.Api.EMPLOYEEROLE.EmployeeIShiftCode;

public sealed class ShiftcodeApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient http;
    public ShiftcodeApiClient(HttpClient http)
    {
        this.http = http;
    }

    public async Task<IReadOnlyList<ShiftRegistrationDayModel>> GetDailyAttendanceAsync(Guid companyId, string employeeId, int month, int year)
    {
        var url = $"GetShiftCode?companyId={companyId}&employeeId={Uri.EscapeDataString(employeeId)}&month={month.ToString(CultureInfo.InvariantCulture)}&year={year.ToString(CultureInfo.InvariantCulture)}";
        var response = await http.GetAsync(url);
        var responseText = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Không tải được lịch đăng ký ca: {responseText}");
        }

        return DeserializeShiftDays(responseText);
    }

    public async Task<string> PostShiftRegistrationAsync(ShiftRegistrationCreateModel request, Guid companyId)
    {
        var response = await http.PostAsJsonAsync($"employeerole/shift-registration?companyId={companyId}", request);
        var responseText = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return responseText;
        }

        throw new Exception($"Không lưu được đăng ký ca: {responseText}");
    }

    public async Task<ShiftRegistrationOverviewModel> GetShiftRegistrationOverviewAsync(Guid companyId, string employeeId, int month, int year)
    {
        var url = $"GetShiftCode/Summary?companyId={companyId}&employeeId={Uri.EscapeDataString(employeeId)}&year={year.ToString(CultureInfo.InvariantCulture)}&month={month.ToString(CultureInfo.InvariantCulture)}";
        var response = await http.GetAsync(url);
        var responseText = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Không tải được tổng quan đăng ký ca: {responseText}");
        }

        if (string.IsNullOrWhiteSpace(responseText))
        {
            return new ShiftRegistrationOverviewModel();
        }

        using var document = JsonDocument.Parse(responseText);
        if (TryDeserializeOverview(document.RootElement, out var overview))
        {
            return overview;
        }

        if (TryFindItemsElement(document.RootElement, out var itemsElement)
            && TryDeserializeOverview(itemsElement, out var wrappedOverview))
        {
            return wrappedOverview;
        }

        return new ShiftRegistrationOverviewModel();
    }
    private static IReadOnlyList<ShiftRegistrationDayModel> DeserializeShiftDays(string responseText)
    {
        if (string.IsNullOrWhiteSpace(responseText))
        {
            return Array.Empty<ShiftRegistrationDayModel>();
        }

        using var document = JsonDocument.Parse(responseText);
        if (TryDeserializeList(document.RootElement, out var directList))
        {
            return directList;
        }

        if (TryFindItemsElement(document.RootElement, out var itemsElement)
            && TryDeserializeList(itemsElement, out var wrappedList))
        {
            return wrappedList;
        }

        if (TryDeserializeItem(document.RootElement, out var singleItem))
        {
            return new[] { singleItem };
        }

        if (TryFindItemsElement(document.RootElement, out var singleWrappedItem)
            && TryDeserializeItem(singleWrappedItem, out var nestedItem))
        {
            return new[] { nestedItem };
        }

        return Array.Empty<ShiftRegistrationDayModel>();
    }

    private static bool TryFindItemsElement(JsonElement element, out JsonElement value)
    {
        foreach (var propertyName in new[] { "data", "items", "result", "results", "value" })
        {
            if (TryGetPropertyIgnoreCase(element, propertyName, out value))
            {
                return true;
            }
        }

        value = default;
        return false;
    }

    private static bool TryDeserializeList(JsonElement element, out IReadOnlyList<ShiftRegistrationDayModel> items)
    {
        items = Array.Empty<ShiftRegistrationDayModel>();
        if (element.ValueKind != JsonValueKind.Array)
        {
            return false;
        }

        var data = JsonSerializer.Deserialize<List<ShiftRegistrationDayModel>>(element.GetRawText(), JsonOptions);
        items = data is null ? Array.Empty<ShiftRegistrationDayModel>() : data;
        return true;
    }

    private static bool TryDeserializeItem(JsonElement element, out ShiftRegistrationDayModel item)
    {
        item = new ShiftRegistrationDayModel();
        if (element.ValueKind != JsonValueKind.Object)
        {
            return false;
        }

        if (!TryGetPropertyIgnoreCase(element, nameof(ShiftRegistrationDayModel.WorkDate), out _))
        {
            return false;
        }

        item = JsonSerializer.Deserialize<ShiftRegistrationDayModel>(element.GetRawText(), JsonOptions) ?? new ShiftRegistrationDayModel();
        return true;
    }

    private static bool TryDeserializeOverview(JsonElement element, out ShiftRegistrationOverviewModel overview)
    {
        overview = new ShiftRegistrationOverviewModel();
        if (element.ValueKind != JsonValueKind.Object)
        {
            return false;
        }

        if (!TryGetPropertyIgnoreCase(element, nameof(ShiftRegistrationOverviewModel.RegisteredDays), out _)
            && !TryGetPropertyIgnoreCase(element, nameof(ShiftRegistrationOverviewModel.MaxDaysInMonth), out _))
        {
            return false;
        }

        overview = JsonSerializer.Deserialize<ShiftRegistrationOverviewModel>(element.GetRawText(), JsonOptions) ?? new ShiftRegistrationOverviewModel();
        return true;
    }

    private static bool TryGetPropertyIgnoreCase(JsonElement element, string propertyName, out JsonElement value)
    {
        foreach (var property in element.EnumerateObject())
        {
            if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                value = property.Value;
                return true;
            }
        }

        value = default;
        return false;
    }

    private string ToAbsoluteUrl(string relativeUrl)
    {
        return http.BaseAddress is null ? relativeUrl : new Uri(http.BaseAddress, relativeUrl).ToString();
    }
}
