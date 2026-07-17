namespace ERP_PAYROLLMOBILE.Services.Ui;

public sealed class PullToRefreshService
{
    public event Func<Task>? RefreshRequested;

    public async Task RequestRefreshAsync()
    {
        var handlers = RefreshRequested;
        if (handlers is null)
        {
            return;
        }

        var delegates = handlers.GetInvocationList()
            .Cast<Func<Task>>();

        foreach (var handler in delegates)
        {
            await handler();
        }
    }
}