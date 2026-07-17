using ERP_PAYROLLMOBILE.Models;
using ERP_PAYROLLMOBILE.Models.EmployeeRequest;
using Microsoft.Maui.Networking;
using System.Net.Http;
using System.Text.Json;

namespace ERP_PAYROLLMOBILE.Api.EMPLOYEEROLE.EmployeeDaily;

public class AttendanceApiClient
{
    public readonly HttpClient http;

    public AttendanceApiClient(HttpClient http)
    {
        this.http = http;
    }

    public async Task<AttendanceDailyViewModel?> GetDailyAttendanceAsync(Guid compaynyId, string employeeId, DateOnly workdate)
    {
        var url = $"GetDaily?companyId={compaynyId}&employeeId={Uri.EscapeDataString(employeeId)}&workDate={Format(workdate)}";
        var response = await SendGetAsync(url, "GetDaily");
        var responseText = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var data = JsonSerializer.Deserialize<AttendanceDailyViewModel>(responseText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return data;
        }

        return null;
    }

    public async Task<List<AttendanceDailyViewModel>> GetDailyBymonthAttendanceAsync(Guid compaynyId, string employeeId, DateOnly from, DateOnly to)
    {
        var url = $"GetDaily/Range?companyId={compaynyId}&employeeId={Uri.EscapeDataString(employeeId)}&from={Format(from)}&to={Format(to)}";
        var response = await SendGetAsync(url, "GetDailyByMonth");
        var responseText = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var data = JsonSerializer.Deserialize<List<AttendanceDailyViewModel>>(responseText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return data ?? new List<AttendanceDailyViewModel>();
        }

        throw new Exception($"Error retrieving daily attendance: {responseText}");
    }

    public async Task<List<AttendanceLogDetailModel>> GetDailyDetailAttendanceAsync(Guid compaynyId, string employeeId, DateOnly workdate)
    {
        var url = $"GetDaily/detaildaily?companyId={compaynyId}&employeeId={Uri.EscapeDataString(employeeId)}&workdate={Format(workdate)}";
        var response = await SendGetAsync(url, "GetDailyDetail");
        var responseText = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var data = JsonSerializer.Deserialize<List<AttendanceLogDetailModel>>(responseText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return data ?? new List<AttendanceLogDetailModel>();
        }

        throw new Exception($"Error retrieving daily attendance: {responseText}");
    }

    private async Task<HttpResponseMessage> SendGetAsync(string relativeUrl, string operationName)
    {
        var absoluteUrl = http.BaseAddress is null
            ? relativeUrl
            : new Uri(http.BaseAddress, relativeUrl).ToString();

        try
        {
            return await http.GetAsync(relativeUrl);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            var networkAccess = Connectivity.Current.NetworkAccess;
            var profiles = string.Join(',', Connectivity.Current.ConnectionProfiles);
            throw new Exception(
                $"Không thể kết nối API chấm công ({operationName}). URL: {absoluteUrl}. NetworkAccess: {networkAccess}. ConnectionProfiles: {profiles}. Chi tiết: {ex.Message}",
                ex);
        }
    }

    public async Task<ForgotCheckSuggestionModel?> GetSuggestForgotCheck(Guid compaynyId, string employeeId, DateOnly from , DateOnly to)
    {
     var url = $"EmployeeRequest/suggest-forgotcheck?companyId={compaynyId}&employeeId={Uri.EscapeDataString(employeeId)}&from={Format(from)}&to={Format(to)}";
        var response = await SendGetAsync(url, "getsuggest-forgotcheck");
        var responseText = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            return ParseForgotCheckSuggestion(responseText);
        }

        return null;
    }

    private static ForgotCheckSuggestionModel? ParseForgotCheckSuggestion(string responseText)
    {
        if (string.IsNullOrWhiteSpace(responseText))
        {
            return null;
        }

        using var document = JsonDocument.Parse(responseText);
        var root = document.RootElement;
        var source = TryGetContainer(root, out var container) ? container : root;

        var model = new ForgotCheckSuggestionModel
        {
            EmployeeId = ReadString(source, "employeeId") ?? ReadString(root, "employeeId") ?? string.Empty,
            WorkDate = ReadDate(source, "workDate", "date", "targetDate", "missingDate")
                ?? ReadDate(root, "workDate", "date", "targetDate", "missingDate")
                ?? default,
            MissingMinutes = ReadInt(source, "missingMinutes") ?? ReadInt(root, "missingMinutes") ?? 0,
            Items = ExtractForgotCheckItems(source)
        };

        if (model.Items.Count == 0 && root.ValueKind == JsonValueKind.Array)
        {
            model.Items = ParseForgotCheckItems(root);
        }

        if (model.MissingMinutes <= 0 && model.Items.Count > 0)
        {
            model.MissingMinutes = model.Items.Sum(item => item.MissingMinutes);
        }

        if (model.Items.Count == 0 && model.WorkDate != default)
        {
            model.Items.Add(new ForgotCheckSuggestionItemModel
            {
                Date = model.WorkDate,
                MissingMinutes = model.MissingMinutes,
                Note = BuildForgotCheckNote(model.MissingMinutes)
            });
        }

        return model;
    }

    private static bool TryGetContainer(JsonElement root, out JsonElement container)
    {
        if (root.ValueKind == JsonValueKind.Object
            && TryGetPropertyIgnoreCase(root, "data", out var data)
            && data.ValueKind is JsonValueKind.Object or JsonValueKind.Array)
        {
            container = data;
            return true;
        }

        container = root;
        return false;
    }

    private static List<ForgotCheckSuggestionItemModel> ExtractForgotCheckItems(JsonElement root)
    {
        if (root.ValueKind == JsonValueKind.Array)
        {
            return ParseForgotCheckItems(root);
        }

        if (root.ValueKind != JsonValueKind.Object)
        {
            return new List<ForgotCheckSuggestionItemModel>();
        }

        foreach (var propertyName in new[] { "items", "dates", "missingDates", "suggestions", "details", "rows" })
        {
            if (TryGetPropertyIgnoreCase(root, propertyName, out var value))
            {
                var items = ParseForgotCheckItems(value);
                if (items.Count > 0)
                {
                    return items;
                }
            }
        }

        var singleItem = ParseForgotCheckItem(root);
        return singleItem is null ? new List<ForgotCheckSuggestionItemModel>() : new List<ForgotCheckSuggestionItemModel> { singleItem };
    }

    private static List<ForgotCheckSuggestionItemModel> ParseForgotCheckItems(JsonElement value)
    {
        var items = new List<ForgotCheckSuggestionItemModel>();

        if (value.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in value.EnumerateArray())
            {
                var parsed = ParseForgotCheckItem(item);
                if (parsed is not null)
                {
                    items.Add(parsed);
                }
            }
        }
        else if (value.ValueKind == JsonValueKind.Object)
        {
            var parsed = ParseForgotCheckItem(value);
            if (parsed is not null)
            {
                items.Add(parsed);
            }
        }

        return items;
    }

    private static ForgotCheckSuggestionItemModel? ParseForgotCheckItem(JsonElement value)
    {
        var date = ReadDate(value, "date", "workDate", "targetDate", "missingDate");
        if (!date.HasValue)
        {
            return null;
        }

        return new ForgotCheckSuggestionItemModel
        {
            Date = date.Value,
            MissingMinutes = ReadInt(value, "missingMinutes")
                ?? ReadInt(value, "minutes")
                ?? ReadInt(value, "forgotMinutes")
                ?? 0,
            Note = ReadString(value, "note")
                ?? ReadString(value, "message")
                ?? ReadString(value, "description")
                ?? BuildForgotCheckNote(ReadInt(value, "missingMinutes")
                    ?? ReadInt(value, "minutes")
                    ?? ReadInt(value, "forgotMinutes")
                    ?? 0)
        };
    }

    private static string BuildForgotCheckNote(int missingMinutes)
    {
        return missingMinutes > 0 ? $"Thiếu {missingMinutes} phút" : "Thiếu CheckOut";
    }

    private static DateOnly? ReadDate(JsonElement value, params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            if (!TryGetPropertyIgnoreCase(value, propertyName, out var propertyValue))
            {
                continue;
            }

            if (propertyValue.ValueKind == JsonValueKind.String)
            {
                var text = propertyValue.GetString();
                if (DateOnly.TryParse(text, out var parsedDate))
                {
                    return parsedDate;
                }

                if (DateTime.TryParse(text, out var parsedDateTime))
                {
                    return DateOnly.FromDateTime(parsedDateTime);
                }
            }
        }

        return null;
    }

    private static int? ReadInt(JsonElement value, string propertyName)
    {
        if (!TryGetPropertyIgnoreCase(value, propertyName, out var propertyValue))
        {
            return null;
        }

        if (propertyValue.ValueKind == JsonValueKind.Number && propertyValue.TryGetInt32(out var number))
        {
            return number;
        }

        if (propertyValue.ValueKind == JsonValueKind.String && int.TryParse(propertyValue.GetString(), out var parsed))
        {
            return parsed;
        }

        return null;
    }

    private static decimal? ReadDecimal(JsonElement value, string propertyName)
    {
        if (!TryGetPropertyIgnoreCase(value, propertyName, out var propertyValue))
        {
            return null;
        }

        if (propertyValue.ValueKind == JsonValueKind.Number && propertyValue.TryGetDecimal(out var number))
        {
            return number;
        }

        if (propertyValue.ValueKind == JsonValueKind.String && decimal.TryParse(propertyValue.GetString(), out var parsed))
        {
            return parsed;
        }

        return null;
    }

    private static string? ReadString(JsonElement value, string propertyName)
    {
        if (!TryGetPropertyIgnoreCase(value, propertyName, out var propertyValue))
        {
            return null;
        }

        return propertyValue.ValueKind == JsonValueKind.String ? propertyValue.GetString() : propertyValue.ToString();
    }

    private static bool TryGetPropertyIgnoreCase(JsonElement value, string propertyName, out JsonElement propertyValue)
    {
        if (value.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in value.EnumerateObject())
            {
                if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    propertyValue = property.Value;
                    return true;
                }
            }
        }

        propertyValue = default;
        return false;
    }

    private static string Format(DateOnly value)
    {
        return value.ToString("yyyy-MM-dd");
    }
}
