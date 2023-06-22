using System.Diagnostics;

namespace Scenarzysta.Lib;
public class RCEScenario : Scenario
{
    public override string Name { get; init; } = "rce";
    public override string FullName { get; init; } = "Remote Code Execution";
    public override string Description { get; init; } = @"Scenariusz tworzący serwer podatny na atak Remote Code Execution.
Zawiera program backendowy (adres 10.13.13.103:8080) oraz
aplikację frontendową (adres 10.13.13.103:3000).
Instaluje środowiska Java 8 oraz Node 17.";

    public override void Initiate()
    {
        // Check or install Java 8
        try
        {
            Log("Szukanie Java 8...");
            RCEUtils.CheckJava8();
            Log("Java 8 zainstalowana.");
        }
        catch
        {
            Log("Nie znaleziono. Instalowanie...");
            RCEUtils.InstallJava8();
            Log("Java 8 zainstalowana. Wyłącz Scenarzystę i włącz jeszcze raz.");
        }

        // Check or install Node
        try
        {
            Log("Szukanie Node 18...");
            RCEUtils.CheckNode18();
            Log("Node 18 zainstalowany.");
        }
        catch
        {
            Log("Nie znaleziono. Instalowanie...");
            RCEUtils.InstallNode18();
            Log("Node 18 zainstalowany. Wyłącz Scenarzystę i włącz jeszcze raz.");
        }

        // Check network
        try { Log("Sprawdzanie, czy adres karty sieciowej jest ustawiony na 10.13.13.103/24..."); RCEUtils.CheckIp(); }
        catch
        {
            throw new Exception("Upewnij się, że adres karty sieciowej jest ustawiony na 10.13.13.103/24.");
        }
        Log("Adres karty sieciowej poprawny.");
    }
    public override List<Process> Start()
    {
        Log("Otwieranie procesów Java i Node...");
        var backend = Utils.OpenWindow("cmd", "/k RCE\\backendstarter.bat");
        var frontend = Utils.StartProgram("node", "app.js", false, Path.Combine(Environment.CurrentDirectory, "RCE\\Front"));
        Log("Procesy otwarte.");

        var processes = new List<Process>
        {
            backend,
            frontend
        };
        return processes;
    }
}
