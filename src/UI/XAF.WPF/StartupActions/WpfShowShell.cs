using XAF.Hosting.Abstraction;
using XAF.Modularity.Abstraction.StartupActions;
using XAF.UI.Abstraction;
using XAF.UI.WPF.Hosting;
using XAF.UI.WPF.ViewComposition;

namespace XAF.UI.WPF.StartupActions;
internal class WpfShowShell : IHostStartupAction
{
    private readonly IViewProvider _viewProvider;
    private readonly IWpfThread _wpfThread;

    public WpfShowShell(IViewProvider viewProvider, IWpfThread wpfThread)
    {
        _viewProvider = viewProvider;
        _wpfThread = wpfThread;
    }

    public StartupActionOrderRule ConfigureExecutionTime()
    {
        return StartupActionOrderRule.Create<WpfShowShell>()
            .ExecuteAfter<StartModules>();
    }

    public async Task Execute(CancellationToken cancellation)
    {
        await _wpfThread.UiDispatcher!.InvokeAsync(() =>
        {
            var shell = _viewProvider.GetShell();
            _wpfThread.Application!.MainWindow = shell;

            shell.Show();
            _wpfThread.SplashWindow?.Close();
        });
    }
}
