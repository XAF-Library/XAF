using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XAF.Hosting.Abstraction;
using XAF.Modularity.Abstraction;
using XAF.UI.Abstraction;

namespace WpfPluginApp.Initializers;
public class NavigationCallbackInitializer : IHostStartupAction
{
    private readonly INavigationService _navigationService;

    public int Priority => ModuleStartupActionPriorities.AfterModuleInitialization;
    public HostStartupActionExecution ExecutionTime => HostStartupActionExecution.AfterHostedServicesStarted;

    public NavigationCallbackInitializer(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public Task Execute(CancellationToken cancellation)
    {
        _navigationService.AddNavigationCallback("PageViews", (descriptor, vm) =>
        {
            var des = descriptor;
            var viewModel = vm;
        });

        return Task.CompletedTask;
    }
}
