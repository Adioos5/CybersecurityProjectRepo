namespace Scenarzysta.Lib;

public static class EternalblueUtils
{
    public static void TurnOnSMB()
    {
        var process = Utils.OpenWindow("DISM", "/Online /Enable-Feature /All /FeatureName:SMB1Protocol", true);
        process.WaitForExit();
    }

    public static void TurnOffFirewall()
    {
        var process = Utils.OpenWindow("netsh", "advfirewall set allprofiles state off", true);
        process.WaitForExit();
    }

    public static void CreateUser()
    {
        var process = Utils.OpenWindow("net", "user user password /add", true);
        process.WaitForExit();
    }
}
