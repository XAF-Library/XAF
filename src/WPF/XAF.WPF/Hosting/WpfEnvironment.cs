using System.Windows;
using System.Windows.Threading;

namespace XAF.WPF.Hosting;
public class WpfEnvironment
{
    public Application App { get; internal set; }

    public Dispatcher Dispatcher { get; internal set; }
}
