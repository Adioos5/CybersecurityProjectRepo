using System.Net.NetworkInformation;
using Scenarzysta.Lib;

namespace Scenarzysta.ConsoleApp;

public class Program
{
    public static void PrintHelp()
    {
        Console.Write(@"
Polecenia:
    help
        Wyświetla tekst pomocy
    scenarios
        Wyświetla dostępne scenariusze
    exit
        Kończy działanie Scenarzysty
    initiate [ID]
        Inicjuje scenariusz o danym ID - instaluje wymagane programy.
    start [ID]
        Rozpoczyna scenariusz o danym ID.
");
    }
    public static void PrintScenarios()
    {
        foreach (var sc in ScenarioManager.Scenarios)
        {
            Console.WriteLine($"ID {sc.Name}");
            Console.WriteLine(sc.FullName);
            Console.WriteLine(sc.Description);
            Console.WriteLine();
        }
    }
    public static void Main(string[] args)
    {
        ScenarioManager.SetLogHandler(message => Console.WriteLine($">>> {message}"));

        // set correct IP on interface
        var ifName = getCorrectInterfaceName();
        if (ifName == "None")
        {
            Console.WriteLine("Zła konfiguracja sieci.");
            Console.ReadKey();
            return;
        }
        var process = Utils.OpenWindow("netsh", $"interface ipv4 set address name=\"{ifName}\" static 10.13.13.103 255.255.255.0 10.13.13.100", true);
        process.WaitForExit();

        Console.Write(@"   
   ###:                                                           #          
 #   .#                                                           #          
 #        ##:   ###   #:##:  .###.   #:##: #####  #.  #  :###:  #####  .###. 
 # .     #        :#  #  :#  #: :#   ##  #    :    : :   #: .#    #    #: :# 
   ##   #.     #   #  #   #      #   #       .#   :# #:  #:.      #        # 
      # #      #####  #   #  :####   #       #     # #   .###:    #    :#### 
      # #.     #      #   #  #:  #   #      #.     #        :#    #    #:  # 
 #.   #  #         #  #   #  #.  #   #     #:       #:   #. :#    #.   #.  # 
 :####.   ##:   ###:  #   #  :##:#   #     #####   :#    :###:    :##  :##:# 
                                                   :#                        
                                                   #:                        
                                                  ##                         
");
        Console.WriteLine();
        Console.WriteLine(@"Program do odtwarzania scenariuszy ataków na sys-");
        Console.WriteLine("temy  informatyczne. Tworzony  w ramach  projektu");
        Console.WriteLine("\"Eksperymenty dotyczące  typów podatności bezpie-");
        Console.WriteLine("czeństwa w oprogramowaniu\".");
        var bgcolor = Console.BackgroundColor;
        var fgcolor = Console.ForegroundColor;
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Jakub Bielecki, Michał Boguszewski, Adrian Cieśla");
        Console.BackgroundColor = bgcolor;
        Console.ForegroundColor = fgcolor;
        Console.WriteLine();
        Console.WriteLine("       Inżynieria Cyberbezpieczeństwa 2023       ");
        Console.WriteLine("-------------------------------------------------");

        PrintHelp();

        while (true)
        {
            Console.Write("# ");
            var line = Console.ReadLine();
            if (line == null)
                break;
            line = line.ToLowerInvariant().TrimEnd();

            bool doExit = false;
            switch (line)
            {
                case "help":
                    PrintHelp();
                    break;
                case "scenarios":
                    PrintScenarios();
                    break;
                case "exit":
                    doExit = true;
                    break;
                default:
                    var commands = line.Split(' ');
                    if (commands.Length != 2 || commands[0] != "initiate" && commands[0] != "start")
                    {
                        Console.WriteLine("Złe polecenie.");
                        break;
                    }
                    try
                    {
                        var sc = ScenarioManager.GetScenario(commands[1]);
                        if (sc == null)
                            throw new Exception("Scenariusz nie istnieje.");
                        if (commands[0] == "initiate")
                            sc.Initiate();
                        else 
                        {
                            var procs = sc.Start();
                            Console.WriteLine(">>> Scenariusz działa. Wciśnij ENTER, aby go zakończyć.");
                            Console.ReadLine();
                            Console.WriteLine(">>> Kończę...");
                            foreach (var p in procs)
                            {
                                if (!p.HasExited)
                                    p.Kill(true);
                                p.Close();
                            }
                            Console.WriteLine(">>> Scenariusz zakończony.");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("!!!");
                        Console.BackgroundColor = bgcolor;
                        Console.ForegroundColor = fgcolor;
                        Console.WriteLine(" " + e.Message);
                    }
                    break;
            }
            if (doExit)
                break;
        }
    }

    private static bool isIpOk()
    {
        var interfaces = NetworkInterface.GetAllNetworkInterfaces();
        return interfaces.Any(iface => iface.GetIPProperties().UnicastAddresses.Any(a => a.IPv4Mask.ToString() == "255.255.255.0" && a.Address.ToString() == "10.13.13.102"));
    }

    private static string getCorrectInterfaceName()
    {
        var interfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (var iface in interfaces)
            if (iface.Name.Contains("Ethernet"))
                return iface.Name;
        return "None";
    }
}
