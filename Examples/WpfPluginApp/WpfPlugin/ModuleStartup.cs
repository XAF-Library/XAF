using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Navigation;
using WpfPlugin.ViewModels;
using XAF.Modularity;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.Modules;

namespace WpfPlugin;

public class WpfModule : WpfModule<Startup>
{
}

public class Startup : ModuleStartup
{
    private readonly IViewService _viewService;
    private readonly INavigationService _navigationService;

    public Startup(IViewService viewService, INavigationService navigationService)
    {
        _viewService = viewService;
        _navigationService = navigationService;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _viewService.AddViewAsync<ViewAViewModel>("PageViews");
        await _viewService.AddViewAsync<ViewBViewModel>("PageViews");
    }
}
