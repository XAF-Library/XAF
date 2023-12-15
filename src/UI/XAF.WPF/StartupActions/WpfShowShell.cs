using System.Reactive.Concurrency;
using XAF.Hosting.Abstraction;
using XAF.Modularity.Abstraction.StartupActions;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.Hosting;
using XAF.UI.WPF.ViewComposition;

namespace XAF.UI.WPF.StartupActions;
public class WpfShowShell : IHostStartupAction
{
    private readonly IWindowService _windowService;
    private readonly IWpfThread _wpfThread;

    public WpfShowShell(IWindowService windowService, IWpfThread wpfThread)
    {
        _windowService = windowService;
        _wpfThread = wpfThread;
    }

    public StartupActionOrderRule ConfigureExecutionTime()
    {
        return StartupActionOrderRule.Create()
            .ExecuteAfter<StartModules>();
    }

    public async Task Execute(CancellationToken cancellation)
    {
        await _windowService.ShowShells();
        if(_wpfThread.SplashWindow != null)
        {
            Schedulers.MainScheduler.Schedule(_wpfThread.SplashWindow.Close);
        }
    }
}

public class WpfCreateShell : IHostStartupAction
{
    private readonly IWindowService _windowService;

    public WpfCreateShell(IWindowService windowService)
    {
        _windowService = windowService;
    }

    public StartupActionOrderRule ConfigureExecutionTime()
    {
        return StartupActionOrderRule.Create()
            .ExecuteBefore<StartModules>();
    }

    public async Task Execute(CancellationToken cancellationToken = default)
    {
        await _windowService.CreateShells();
    }
}