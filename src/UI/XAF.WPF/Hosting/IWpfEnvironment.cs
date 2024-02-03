using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
