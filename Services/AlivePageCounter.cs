namespace BlazorServerAlivePageCount.Services;

public class AlivePageCounter
{
    public int AlivePageCount { get; private set; }

    public delegate ValueTask AlivePageCountChangedEventHandler(object? sender, EventArgs e);

    public event AlivePageCountChangedEventHandler? AlivePageCountChanged;

    public async ValueTask IncrementAsync()
    {
        this.AlivePageCount++;
        await this.InvokeAsync(AlivePageCountChanged, EventArgs.Empty);
    }

    public async ValueTask DecrementAsync()
    {
        this.AlivePageCount--;
        await this.InvokeAsync(AlivePageCountChanged, EventArgs.Empty);
    }

    private async ValueTask InvokeAsync(AlivePageCountChangedEventHandler? asyncEventHandler, EventArgs args)
    {
        if (asyncEventHandler == null) return;

        var asyncHandlerTasks = asyncEventHandler.GetInvocationList()
            .Cast<AlivePageCountChangedEventHandler>()
            .Select(handler => handler.Invoke(this, args).AsTask())
            .ToArray();
        await Task.WhenAll(asyncHandlerTasks);
    }
}
