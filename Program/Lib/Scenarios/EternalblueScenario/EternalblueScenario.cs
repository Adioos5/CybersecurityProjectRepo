using System.Diagnostics;

namespace Scenarzysta.Lib;

public class EternalblueScenatio : Scenario
{
    public override string Name { get; init; } = "eternalblue";
    public override string FullName { get; init; } = "Eternalblue";
    public override string Description { get; init; } = @"Scenariusz wyłącza firewalla oraz włącza protokół SMBv1";

    public override void Initiate()
    {
        Log("Tworzę użytkownika...");
        EternalblueUtils.CreateUser();
        Log("Włączam SMB...");
        EternalblueUtils.TurnOnSMB();
        EternalblueUtils.TurnOffFirewall();
    }

    public override List<Process> Start()
    {
        return new();
    }
}
