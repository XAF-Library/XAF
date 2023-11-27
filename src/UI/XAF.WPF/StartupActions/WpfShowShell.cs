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

    public WpfShowShell(IWindowService windowService)
    {
        _windowService = windowService;
    }

    public StartupActionOrderRule ConfigureExecutionTime()
    {
        return StartupActionOrderRule.Create()
            .ExecuteAfter<StartModules>();
    }

    public async Task Execute(CancellationToken cancellation)
    {
        await _windowService.ShowShell();
    }
}
