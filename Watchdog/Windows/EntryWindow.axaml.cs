using System.Diagnostics;
using System.Net.NetworkInformation;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Watchdog;

public partial class EntryWindow : Window
{
    private bool correctIp = false;
    private string ifaceName = "";
    private Button btn;
    private TextBlock box;

    public EntryWindow()
    {
        InitializeComponent();
        btn = this.Find<Button>("Button")!;
        btn.Click += Button_Click;
        box = this.Find<TextBlock>("IpInfo")!;

        checkIp();
    }

    private void Button_Click(object? sender, RoutedEventArgs e)
    {
        if (correctIp) {
            if (Avalonia.Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                desktop.MainWindow.Show();
            }
            Close();
        } else {
            // change IP with pkexec
            // you have iface name already!

            var proc = Process.Start("pkexec", $"ifconfig {ifaceName} 10.13.13.102 netmask 255.255.255.0");
            proc.WaitForExit();
            checkIp();
        }
    }

    private void checkIp()
    {
        btn.IsEnabled = false;
        box.FontWeight = Avalonia.Media.FontWeight.Normal;
        box.Foreground = Brushes.Black;
        box.Text = "Sprawdzam adres IP...";
        if ((ifaceName = getCorrectInterfaceName()) == "None")
        {
            box.FontWeight = Avalonia.Media.FontWeight.Bold;
            box.Foreground = Brushes.Red;
            box.Text = "Brak interfejsu sieciowego. Sprawdź dokumentację.";
            return;
        }
        if (isIpOk() == false)
        {
            box.FontWeight = Avalonia.Media.FontWeight.Bold;
            box.Foreground = Brushes.Red;
            box.Text = "Adres IP jest niepoprawny.";
            btn.IsEnabled = true;
            btn.Content = "Napraw adres IP";
        } else {
            correctIp = true;
            box.FontWeight = Avalonia.Media.FontWeight.Bold;
            box.Foreground = Brushes.DarkGreen;
            box.Text = "Adres IP jest poprawny.";

            btn.IsEnabled = true;
            btn.Content = "Rozpocznij";
        }
    }

    private bool isIpOk()
    {
        var interfaces = NetworkInterface.GetAllNetworkInterfaces();
        return interfaces.Any(iface => iface.GetIPProperties().UnicastAddresses.Any(a => a.IPv4Mask.ToString() == "255.255.255.0" && a.Address.ToString() == "10.13.13.102"));
    }

    private string getCorrectInterfaceName()
    {
        var interfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (var iface in interfaces)
            if (iface.Name.Contains("eth"))
                return iface.Name;
        return "None";
    }
}