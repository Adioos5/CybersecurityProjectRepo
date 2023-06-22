using System.Diagnostics;

namespace Scenarzysta.Lib;
public class SQLScenario : Scenario
{
    public override string Name { get; init; } = "sql";
    public override string FullName { get; init; } = "SQL Injection";
    public override string Description { get; init; } = @"(brak opisu)"; // TODO

    public override void Initiate()
    {
        // Done by J:
        // - copy database from SQL\db\politicians2.db to .\politicians2.db
        // - copy file from SQL\appsettings.json to .\appsettings.json - not needed
        File.Copy(".\\db\\politicians2.db", ".\\politicians2.db", true);
        Log("SQL Injection zakończył inicjalizację.");
    }
    public override List<Process> Start()
    {
        Log("Otwieranie procesu ASP.NET...");
        var backend = Utils.OpenWindow("cmd", "/k SQL\\SQL_INJ_API.exe");
        Log("Proces otwarty.");

        var processes = new List<Process>
        {
            backend
        };
        return processes;
    }
}
