using XAF.Hosting.Abstraction;
using XAF.UI.WPF.Hosting;
using XAF.Modularity.Abstraction.StartupActions;
using Microsoft.Extensions.DependencyInjection;
using XAF.UI.WPF.ViewComposition;

namespace XAF.UI.WPF.StartupActions;
public class WpfShowSplashScreen : IHostStartupAction
{
    private readonly IWpfThread _wpfThread;
    private readonly IServiceProvider _serviceProvider;

    public WpfShowSplashScreen(IWpfThread wpfThread, IServiceProvider serviceProvider)
    {
        _wpfThread = wpfThread;
        _serviceProvider = serviceProvider;
    }

    public StartupActionOrderRule ConfigureExecutionTime()
    {
        return StartupActionOrderRule.Create()
            .ExecuteBefore<StartModules>()
            .ExecuteAfter<StartHostedServices>();
    }

    public async Task Execute(CancellationToken cancellation)
    {
        await _wpfThread.WaitForAppCreation();
        var window = await _wpfThread.UiDispatcher!.InvokeAsync(() =>
        {
            var window = _serviceProvider.GetRequiredService<SplashWindow>();
            _wpfThread.Application!.MainWindow = window;
            _wpfThread.SplashWindow = window;
            window.Show();
            return window;
        });
        await window.OnAppLoadAsync();
    }
}
