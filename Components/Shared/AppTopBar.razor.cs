using Microsoft.AspNetCore.Components;

namespace ERP_PAYROLLMOBILE.Components.Shared;

public partial class AppTopBar : ComponentBase
{
    [Parameter] public int NotificationCount { get; set; }
    [Parameter] public EventCallback OnLogout { get; set; }
}
