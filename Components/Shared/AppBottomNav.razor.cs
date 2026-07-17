using Microsoft.AspNetCore.Components;

namespace ERP_PAYROLLMOBILE.Components.Shared;

public partial class AppBottomNav : ComponentBase
{
    [Parameter] public string ActiveTab { get; set; } = "Chấm công";
    [Parameter] public EventCallback<string> OnTabChanged { get; set; }

    protected readonly string[] Tabs = { "Chấm công", "Yêu cầu", "Đăng ký ca", "Lương cơ bản" };

    protected async Task HandleTabClick(string tab)
    {
        if (tab == ActiveTab) return;
        ActiveTab = tab;

        if (OnTabChanged.HasDelegate)
            await OnTabChanged.InvokeAsync(tab);
    }
}
