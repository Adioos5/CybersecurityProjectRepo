using System.Collections.ObjectModel;
using ReactiveUI;

public class BufferWindowContext : ReactiveObject
{
    private string dataPiece = "";
    public string Memory
    {
        get => dataPiece;
        set => this.RaiseAndSetIfChanged(ref dataPiece, value);
    }

    private bool initialized = false;
    public bool Initialized
    {
        get => initialized;
        set => this.RaiseAndSetIfChanged(ref initialized, value);
    }
}