using Avalonia.Controls;

namespace Watchdog;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        MainWindowContext context = new MainWindowContext(onRCEStart: () => {
            var rce = new RCEWindow();
            rce.ShowDialog(this);
        }, onBufferStart: () => {
            var buf = new BufferWindow();
            buf.ShowDialog(this);
        }, onSQLStart: () => {
            var sql = new SQLWindow();
            sql.ShowDialog(this);
        });
        DataContext = context;
        InitializeComponent();
    }
}