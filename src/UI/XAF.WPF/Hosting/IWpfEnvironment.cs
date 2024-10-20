using System.Windows;
using System.Windows.Threading;

namespace XAF.UI.WPF.Hosting;
public interface IWpfEnvironment
{
    Application WpfApp { get; }

    Dispatcher WpfDispatcher { get; }

    Task WaitForAppStart();

    Task WaitForAppShutDown();
}
