using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using ReactiveUI;

namespace Watchdog;

public partial class BufferWindow : Window
{
    private BufferWindowContext context;
    private Process proc;

    public BufferWindow()
    {
        InitializeComponent();
        context = new BufferWindowContext();
        this.DataContext = context;
        context.Memory = "Przed rozpoczęciem pracy uruchom scenariusz w programie \"Scenarzysta\" poleceniamim:\ninitiate eternalblue\nstart eternalblue\n\n(Uruchamianie Metasploit...)\n";
        context.Changed.Subscribe((_) =>
        {
            DispatcherTimer.RunOnce(() => {
                this.Find<ScrollViewer>("ConsoleScroll")!.ScrollToEnd();
            }, TimeSpan.FromMilliseconds(100));
        });
        var runInGui = (Action action) => Dispatcher.UIThread.InvokeAsync(action);
        var removeEscapeSeq = (string str) => Regex.Replace(str, "\\x1b\\[[0-9;]*m", "");
        // var updateConsole = () => runInGui(() =>
        // {
        //     this.Find<TextBlock>("Console")!.Text = lines + "\n";
        // });

        proc = Processes.RunWithOutputCallback("msfconsole", "-qL", (sender, outputLine) =>
        {
            runInGui(() => {
                if (context.Initialized == false)
                {
                    context.Memory += "(Metasploit aktywny)\n";
                    context.Initialized = true;
                }
            });
            if (outputLine.Data == null)
            {
                runInGui(() => context.Memory += "(Proces zakończył działanie)\n");
                return;
            }
            runInGui(() => context.Memory += "1 > " + removeEscapeSeq(outputLine.Data) + "\n");
        }, (sender, outputLine) =>
        {
            if (outputLine.Data == null)
                return;
            runInGui(() => context.Memory += "2 > " + removeEscapeSeq(outputLine.Data) + "\n");
        });
        proc.StandardInput.AutoFlush = true;
        proc.StandardInput.WriteLine("version");

        this.Find<Button>("Execute")!.Click += (object? sender, RoutedEventArgs args) =>
        {
            var command = this.Find<TextBox>("Input")!.Text;
            this.Find<TextBox>("Input")!.Text = "";
            if (command == "!cl")
            {
                runInGui(() => context.Memory = "");
                return;
            }
            if (proc.HasExited)
            {
                runInGui(() => context.Memory += "(Proces zakończył działanie)\n");
                return;
            }
            runInGui(() => context.Memory += "0 < " + command + "\n");
            proc.StandardInput.WriteLine(command);
        };

        this.Closing += (object? sender, CancelEventArgs args) =>
        {
            if (!proc.HasExited)
                proc.Kill(true);
        };
    }
}