using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.Modularity;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.WPF.ViewComposition;
public abstract class StartupViewModel : XafViewModel
{
    public virtual async Task PrepareAppAsync(IEnumerable<IModuleStartup> modules, CancellationToken cancellationToken)
    {
        foreach (var module in modules)
        {
            await module.PrepareAsync(cancellationToken);
        }
    }

    public virtual async Task LoadAppAsync(IEnumerable<IModuleStartup> modules, CancellationToken cancellationToken) 
    {
        foreach (var module in modules)
        {
            await module.StartAsync(cancellationToken);
        }
    }
}
