using System.Collections.ObjectModel;
using ReactiveUI;

public class RCEWindowContext : ReactiveObject
{
    public class LineObject 
    {
        public string Line { get; set; } = "";
        public string Weight { get; set; } = "Regular";
    }

    private string dataPiece;
    public string Payload
    {
        get => dataPiece;
        set => this.RaiseAndSetIfChanged(ref dataPiece, value);
    }

    public RCEWindowContext(string initializer = "")
    {
        dataPiece = initializer;
    }

    private ObservableCollection<LineObject> lines = new();

    public ObservableCollection<LineObject> Lines 
    {
        get => lines;
        set => this.RaiseAndSetIfChanged(ref lines, value);
    }
}