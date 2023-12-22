using Microsoft.Extensions.Hosting;
using System.Windows;
using XAF.UI.Abstraction;
using XAF.UI.WPF.Hosting;

namespace XAF.UI.WPF.Hosting.Internal;
public class WpfApp : IHostedService
{
    private readonly IWpfThread _wpfThread;

    public WpfApp(IWpfThread wpfThread)
    {
        _wpfThread = wpfThread;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _wpfThread.StartAsync(cancellationToken).ConfigureAwait(false);
        await _wpfThread.WaitForAppStart().ConfigureAwait(false);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _wpfThread.StopAsync(cancellationToken);
    }
}
