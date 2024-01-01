using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace XAF.UI.WPF.Hosting.Internal;
internal class WpfEnvironment : IWpfEnvironment
{
    public Application WpfApp { get; set; }

    public Dispatcher WpfDispatcher { get; set; }

    public ManualResetEvent AppStartLock { get; } = new(false);
    public ManualResetEvent AppShutDownLock { get; } = new(false);

    public Task WaitForAppShutDown()
    {
        AppShutDownLock.WaitOne();
        return Task.CompletedTask;
    }

    public Task WaitForAppStart()
    {
        AppStartLock.WaitOne();
        return Task.CompletedTask;
    }
}
