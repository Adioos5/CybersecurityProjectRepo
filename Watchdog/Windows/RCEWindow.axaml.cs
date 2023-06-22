using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using ReactiveUI;

namespace Watchdog;

public partial class RCEWindow : Window
{
    private int step = 1;
    private Process? pyProcess = null, ncProcess = null;
    private Button next;

    public RCEWindow()
    {
        DataContext = new RCEWindowContext("");
        InitializeComponent();
        next = this.Find<Button>("Next")!;
        next.Click += Next_Click;

        this.Closing += (object? sender, CancelEventArgs args) =>
        {
            if (pyProcess != null && !pyProcess.HasExited)
                pyProcess.Kill(true);
            if (ncProcess != null && !ncProcess.HasExited)
                ncProcess.Kill(true);
        };
    }

    private void Next_Click(object? sender, RoutedEventArgs e)
    {
        // push the step up
        step++;
        switch (step)
        {
            case 2:
                this.Find<StackPanel>("Start")!.IsVisible = true;
                break;
            case 3:
                this.Find<TextBlock>("Compiling")!.IsVisible = true;
                next.IsEnabled = false;
                StartJavaHackerAndNetcat(); // TODO: other labels
                break;
            case 4:
                // case of waiting for netcat to end
                // TODO: Other labels
                break;
            case 5:
                next.IsEnabled = true;
                next.Content = "Powrót do menu";
                this.Find<StackPanel>("Final")!.IsVisible = true;
                // foreach (var msg in retrievedData)
                //     Console.Error.WriteLine(">>> " + msg);
                break;
            default:
                this.Close();
                break;
        }

        DispatcherTimer.RunOnce(() =>
        {
            this.Find<ScrollViewer>("Scroller")!.ScrollToEnd();
        }, TimeSpan.FromMilliseconds(100));
    }

    private void StartJavaHackerAndNetcat()
    {
        // Java compiling
        string text = File.ReadAllText("./RCE/exploit/Code.java");
        text = text.Replace("$HOSTIP", "10.13.13.102");
        File.WriteAllText("./RCE/Code.java", text);
        var javacProcess = Processes.Run("./RCE/jdk8/bin/javac", "./RCE/Code.java");
        javacProcess.WaitForExit();
        if (javacProcess.ExitCode != 0)
        {
            this.Find<TextBlock>("NotCompiled")!.IsVisible = true;
            Console.Error.Write("Compilation errors");
            return;
        }
        this.Find<TextBlock>("Compiled")!.IsVisible = true;

        // Starting hacker and netcat, both with output
        this.Find<TextBlock>("Compiled2")!.IsVisible = true;
        pyProcess = Processes.RunWithOutputCallback("./run_rce.sh", "", (sender, outputLine) =>
        {
            Console.Error.WriteLine("pyProcess: " + outputLine.Data ?? "NULL");
            if (outputLine.Data != null && outputLine.Data.StartsWith("Do przeslania do serwera: "))
            {
                var payload = outputLine.Data.Substring(26);
                OnPythonStart(payload);
            }
        });
        ncProcess = Processes.RunWithOutputCallback("nc", "-lvnp 9001", (sender, outputLine) =>
        {
            Console.Error.WriteLine("ncProcess: " + outputLine.Data ?? "NULL");
            if (outputLine.Data == null)
                OnNetcatFinish();
            else
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    bool bold = outputLine.Data.Contains("BLOB") || outputLine.Data.Contains("CONNECTION");
                    (DataContext as RCEWindowContext)!.Lines.Add(new RCEWindowContext.LineObject() { Line = outputLine.Data, Weight = bold ? "Bold" : "Regular" });
                });
            }
        });
        this.Find<TextBlock>("NetcatRunning")!.IsVisible = true;
        this.Find<Border>("NetcatRunning2")!.IsVisible = true;
    }

    private void OnPythonStart(string payload)
    {
        Console.Error.WriteLine("PythonStarted!");
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            DataContext = new RCEWindowContext(payload);

            // To next step
            Next_Click(null, new RoutedEventArgs());
            this.Find<StackPanel>("NetcatRunning3")!.IsVisible = true;
        });
    }

    private void OnNetcatFinish()
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            Next_Click(null, new RoutedEventArgs());
            if (pyProcess != null)
            {
                if (!pyProcess.HasExited)
                    pyProcess.Kill(true);
                pyProcess.Close();
            }
            if (ncProcess != null)
            {
                ncProcess.Close();
            }
            pyProcess = null;
            ncProcess = null;
        });
    }
}