namespace Scenarzysta.Lib;
public static class ScenarioManager
{
    public readonly static string Home;
    public readonly static string SaveFile;
    public readonly static string FullFolder;

    public readonly static Scenario[] Scenarios;

    private static void ensureFilesExist()
    {
        if (!Directory.Exists(FullFolder))
            Directory.CreateDirectory(FullFolder);

        if (!File.Exists(SaveFile))
            File.Create(SaveFile);
    }

    static ScenarioManager()
    {
        Home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        FullFolder = Path.Combine(Home, "Scenarzysta2023");
        SaveFile = Path.Combine(FullFolder, "config.txt");

        Scenarios = new Scenario[] { 
            new RCEScenario(),
            new EternalblueScenatio(),
            new SQLScenario()
        };

        ensureFilesExist();
    }

    public static Scenario? GetScenario(string name)
        => Scenarios.SingleOrDefault(sc => sc.Name == name);

    public static void SetLogHandler(Action<string> Handler)
    {
        foreach (var sc in Scenarios)
            sc.Logger = Handler;
    }
}
