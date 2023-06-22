using System.Collections.ObjectModel;
using ReactiveUI;

public class SQLWindowContext : ReactiveObject
{
    private bool running = false;
    public bool Running
    {
        get => running;
        set => this.RaiseAndSetIfChanged(ref running, value);
    }

    private string lastResult = "";
    public string LastResult
    {
        get => lastResult;
        set => this.RaiseAndSetIfChanged(ref lastResult, value);
    }
}