using ReactiveUI;

public class MainWindowContext : ReactiveObject
{
    // Screen 1
    private bool showDescriptions = true;
    public bool ShowDescriptions
    {
        get => showDescriptions;
        set => this.RaiseAndSetIfChanged(ref showDescriptions, value);
    }

    public Action OnRCEStart { get; init; }
    public Action OnBufferStart { get; init; }
    public Action OnSQLStart { get; init; }

    public MainWindowContext(Action? onRCEStart = null, Action? onBufferStart = null, Action? onSQLStart = null)
    {
        var empty = () => {};
        OnRCEStart = onRCEStart ?? empty;
        OnBufferStart = onBufferStart ?? empty;
        OnSQLStart = onSQLStart ?? empty;
    }
}