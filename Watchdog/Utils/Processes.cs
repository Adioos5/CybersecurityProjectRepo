
using System.Diagnostics;

public static class Processes
{
    public static Process Run(string name, string args)
    {
        ProcessStartInfo procStartInfo = new ProcessStartInfo(name, args);
        procStartInfo.RedirectStandardInput = true;
        procStartInfo.RedirectStandardOutput = true;
        procStartInfo.RedirectStandardError = true;
        procStartInfo.UseShellExecute = false;
        procStartInfo.CreateNoWindow = true;
        Process proc = new Process();
        proc.StartInfo = procStartInfo;
        proc.Start();
        return proc;
    }

    public static Process RunWithOutputCallback(string name, string args, DataReceivedEventHandler onOutput, DataReceivedEventHandler? onError = null)
    {
        ProcessStartInfo procStartInfo = new ProcessStartInfo(name, args);
        procStartInfo.RedirectStandardInput = true;
        procStartInfo.RedirectStandardOutput = true;
        procStartInfo.RedirectStandardError = true;
        procStartInfo.UseShellExecute = false;
        procStartInfo.CreateNoWindow = true;
        Process proc = new Process();
        proc.StartInfo = procStartInfo;
        proc.EnableRaisingEvents = true;
        proc.Start();
        proc.OutputDataReceived += onOutput;
        proc.BeginOutputReadLine();
        if (onError != null) 
        {
            proc.ErrorDataReceived += onError;
            proc.BeginErrorReadLine();
        }
        return proc;
    }
}