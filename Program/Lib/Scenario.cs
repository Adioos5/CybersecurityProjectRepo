using System.Diagnostics;

namespace Scenarzysta.Lib;
public abstract class Scenario
{
    public abstract string Name { get; init; }
    public abstract string FullName { get; init; }
    public abstract string Description { get; init; }
    public abstract void Initiate();
    public abstract List<Process> Start();
    public Action<string>? Logger { private get; set; }
    protected void Log(string Message)
    {
        if (Logger != null)
            Logger(Message);
    }
}
