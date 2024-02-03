using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfPlugin;
using XAF.Modularity;
using XAF.UI.WPF.ViewComposition;

namespace WpfPluginApp.ViewModels;
public class SplashWindowViewModel : StartupViewModel
{
    public override async Task LoadAppAsync(IEnumerable<IModuleStartup> modules, CancellationToken cancellationToken)
    {
        await base.LoadAppAsync(modules, cancellationToken);
        await Task.Delay(3000);
    }
}
