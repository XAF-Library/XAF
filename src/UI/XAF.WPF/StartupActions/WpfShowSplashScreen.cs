using System.Windows;
using System.Diagnostics;
using XAF.Hosting.Abstraction;
using XAF.UI.Abstraction;
using XAF.UI.WPF.Hosting;
using XAF.Modularity.Abstraction.StartupActions;

namespace XAF.UI.WPF.StartupActions;
internal class WpfShowSplashScreen : IHostStartupAction
{
    private readonly IWpfThread _wpfThread;
    private readonly ISplashWindowViewModel _splashViewModel;

    public WpfShowSplashScreen(IWpfThread wpfThread, ISplashWindowViewModel splashViewModel)
    {
        _wpfThread = wpfThread;
        _splashViewModel = splashViewModel;
    }

    public StartupActionOrderRule ConfigureExecutionTime()
    {
        return StartupActionOrderRule.CreateFor<WpfShowSplashScreen>()
            .ExecuteBefore<StartModules>()
            .ExecuteAfter<StartHostedServices>();
    }

    public async Task Execute(CancellationToken cancellation)
    {
        if (_splashViewModel != null)
        {
            await _wpfThread.UiDispatcher!.InvokeAsync(() =>
            {
                var splashWindow = Activator.CreateInstance(_splashViewModel.WindowType) as Window ??
                    throw new NotSupportedException("the provided splashWindowType is not valid. The splash window must be an Window and " +
                    "it must contain a parameterless constructor");

                splashWindow.DataContext = _splashViewModel;
                if (!_wpfThread.AppCreated)
                {
                    throw new UnreachableException();
                }

                _wpfThread.SplashWindow = splashWindow;
                _wpfThread.Application.MainWindow = splashWindow;
                splashWindow.Show();
            });
            await _splashViewModel.OnAppStartAsync();
        }
    }
}

internal class WpfSplashVmExecuteAfterModuleInitialization : IHostStartupAction
{
    private readonly ISplashWindowViewModel _splashViewModel;

    public WpfSplashVmExecuteAfterModuleInitialization(ISplashWindowViewModel splashViewModel)
    {
        _splashViewModel = splashViewModel;
    }

    public async Task Execute(CancellationToken cancellation)
    {
        await _splashViewModel.AfterModuleInitializationAsync().ConfigureAwait(false);
    }

    public StartupActionOrderRule ConfigureExecutionTime()
    {
        return StartupActionOrderRule.CreateFor<WpfSplashVmExecuteAfterModuleInitialization>()
            .ExecuteAfter<StartModules>()
            .ExecuteBefore<WpfShowShell>();
    }
}
