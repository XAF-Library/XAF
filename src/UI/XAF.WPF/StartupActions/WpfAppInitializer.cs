using XAF.Hosting.Abstraction;
using XAF.WPF.Hosting;
using XAF.WPF.ViewComposition;

namespace XAF.WPF.StartupActions;
internal class WpfAppInitializer : IHostStartupAction
{
    private readonly ISplashWindowViewModel _splashViewModel;
    private readonly IViewProvider _viewProvider;
    private readonly IWpfThread _wpfThread;

    public WpfAppInitializer(ISplashWindowViewModel slpashViewModel, IViewProvider viewProvider, IWpfThread wpfThread)
    {
        _splashViewModel = slpashViewModel;
        _viewProvider = viewProvider;
        _wpfThread = wpfThread;
    }

    public int Priority => StartupActionPriority.ShowMainWindow;
    public HostStartupActionExecution ExecutionTime => HostStartupActionExecution.AfterHostedServicesStarted;

    public async Task Execute(CancellationToken cancellation)
    {
        await _wpfThread.UiDispatcher!.InvokeAsync(() =>
        {
            var shell = _viewProvider.GetShell();
            _wpfThread.Application!.MainWindow = shell;

            shell.Show();
            _splashViewModel.SplashWindow?.Close();
        });
    }
}
