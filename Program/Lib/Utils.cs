using System.Diagnostics;

namespace Scenarzysta.Lib;
public static class Utils
{
    public static Process StartProgram(string prog, string args, bool runAsAdmin = false, string cwd = "")
    {
        ProcessStartInfo procStartInfo = new ProcessStartInfo(prog, args);
        procStartInfo.RedirectStandardOutput = true;
        if (cwd != "")
            procStartInfo.WorkingDirectory = cwd;
        procStartInfo.RedirectStandardError = true;
        procStartInfo.UseShellExecute = false;
        if (runAsAdmin)
            procStartInfo.Verb = "runas";
        // procStartInfo.CreateNoWindow = true;
        Process proc = new Process();
        proc.StartInfo = procStartInfo;
        proc.Start();
        return proc;
    }

    public static Process OpenWindow(string prog, string args = "", bool runAsAdmin = false, string cwd = "")
    {
        ProcessStartInfo procStartInfo = new ProcessStartInfo(prog, args);
        procStartInfo.UseShellExecute = true;
        if (cwd != "")
            procStartInfo.WorkingDirectory = cwd;
        if (runAsAdmin)
            procStartInfo.Verb = "runas";
        // procStartInfo.CreateNoWindow = true;
        Process proc = new Process();
        proc.StartInfo = procStartInfo;
        proc.Start();
        return proc;
    }

    public static (string Output, string Error) GetProcessOutput(Process proc)
    {
        return (proc.StandardOutput.ReadToEnd(), proc.StandardError.ReadToEnd() ?? "");
    }
}
