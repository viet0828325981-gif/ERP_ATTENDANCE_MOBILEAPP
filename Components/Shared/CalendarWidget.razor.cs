using Microsoft.AspNetCore.Components;
using System.Linq;

namespace ERP_PAYROLLMOBILE.Components.Shared;

public partial class CalendarWidget : ComponentBase
{
    [Parameter] public int Year { get; set; } = DateTime.Now.Year;
    [Parameter] public int Month { get; set; } = DateTime.Now.Month;
    [Parameter] public int? SelectedDay { get; set; }
    [Parameter] public Dictionary<int, string> ShiftCodesByDay { get; set; } = new();
    [Parameter] public HashSet<int> EventDays { get; set; } = new();
    [Parameter] public EventCallback<DateTime> OnDaySelected { get; set; }

    protected List<List<DayCell>> Weeks { get; private set; } = new();

    protected string MonthName => new DateTime(Year, Month, 1).ToString("MMMM",
        System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));

    protected override void OnParametersSet()
    {
        Weeks = BuildWeeks(Year, Month);
    }

    private List<List<DayCell>> BuildWeeks(int year, int month)
    {
        var firstDay = new DateTime(year, month, 1);
        var daysInMonth = DateTime.DaysInMonth(year, month);
        int leadingEmpty = ((int)firstDay.DayOfWeek + 6) % 7;

        var cells = new List<DayCell>();

        for (int i = 0; i < leadingEmpty; i++)
            cells.Add(DayCell.Empty());

        for (int day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(year, month, day);

            cells.Add(new DayCell
            {
                Day = day,
                Date = date,
                IsSunday = date.DayOfWeek == DayOfWeek.Sunday,
                IsSelected = SelectedDay.HasValue && SelectedDay.Value == day,
                HasEvent = EventDays.Contains(day),
                ShiftCode = ShiftCodesByDay.TryGetValue(day, out var code) ? code : "--"
            });
        }

        var weeks = new List<List<DayCell>>();
        for (int i = 0; i < cells.Count; i += 7)
        {
            var week = cells.Skip(i).Take(7).ToList();
            while (week.Count < 7)
                week.Add(DayCell.Empty());

            weeks.Add(week);
        }

        return weeks;
    }

    protected async Task HandleDayClick(DayCell cell)
    {
        if (cell.IsEmpty) return;

        SelectedDay = cell.Day;

        if (OnDaySelected.HasDelegate)
            await OnDaySelected.InvokeAsync(cell.Date);
    }

    protected class DayCell
    {
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public string ShiftCode { get; set; } = "--";
        public bool IsSunday { get; set; }
        public bool IsSelected { get; set; }
        public bool HasEvent { get; set; }
        public bool IsEmpty { get; set; }

        public static DayCell Empty() => new() { IsEmpty = true };
    }
}
