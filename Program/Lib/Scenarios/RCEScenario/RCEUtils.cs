using System.Diagnostics;

namespace Scenarzysta.Lib;
public static class RCEUtils
{
    public static void CheckJava8()
    {
        var process = Utils.StartProgram("java", "-version");
        var data = Utils.GetProcessOutput(process);
        process.WaitForExit();

        if (!data.Error.Contains("1.8.0_181"))
        {
            throw new Exception("Java 8 not installed");
        }
    }

    public static void InstallJava8()
    {
        var process = Utils.OpenWindow(Path.Combine(Environment.CurrentDirectory, "RCE\\java8installer.exe"), "", true);
        process.WaitForExit();
    }

    public static void CheckNode18()
    {
        var process = Utils.StartProgram("node", "-v");
        var data = Utils.GetProcessOutput(process);
        process.WaitForExit();

        if (!data.Error.Contains("v18") && !data.Output.Contains("v18"))
        {
            throw new Exception("Node 18 not installed");
        }
    }

    public static void CheckIp()
    {
        var process = Utils.StartProgram("ipconfig", "");
        var data = Utils.GetProcessOutput(process);
        process.WaitForExit();

        if (!data.Error.Contains("10.13.13.103") && !data.Output.Contains("10.13.13.103"))
        {
            throw new Exception("Node 18 not installed");
        }
    }

    public static void InstallNode18()
    {
        var process = Utils.StartProgram("msiexec", "/i \"RCE\\node18installer.msi\"");
        process.WaitForExit();
    }
}
